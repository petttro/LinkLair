using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Timeout;

namespace LinkLair.Common.DynamoDb;

public class DynamoDbContext : IDynamoDbContext
{
    private readonly DynamoDbConfig _dynamoDbConfig;
    private readonly AsyncPolicy _dynamoPolicy;

    private IDynamoDbConnection Connection { get; }

    private string TableNamePrefix => $"{_dynamoDbConfig.TablePrefix}_";

    protected ILogger Logger { get; }

    public DynamoDbContext(ILoggerFactory loggerFactory, IDynamoDbConnection connection, IOptions<DynamoDbConfig> options)
    {
        Logger = loggerFactory.CreateLogger(GetType());
        Connection = connection;
        _dynamoDbConfig = options.Value;
        _dynamoPolicy = BuildPolicy();
    }

    public async Task SaveAsync<TEntity>(TEntity entity)
        where TEntity : DynamoDbEntity
    {
        Logger.LogTrace($"Calling {nameof(SaveAsync)} EntityType={typeof(TEntity).Name} PartitionKey={entity.PartitionKey} SortKey={entity.SortKey}" +
                        $" TablePrefix={TableNamePrefix}");

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var dynamoDbOperationConfig = new DynamoDBOperationConfig { TableNamePrefix = TableNamePrefix };
            await _dynamoPolicy.ExecuteAsync(
                async (ctx, ct) => await Connection.Context.SaveAsync(entity, dynamoDbOperationConfig, ct),
                new Context(nameof(SaveAsync)),
                CancellationToken.None);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Exceeded retries Method={nameof(SaveAsync)} Type={typeof(TEntity).Name} " +
                               $"PartitionKey={entity.PartitionKey} SortKey={entity.SortKey}";
            Logger.LogError(ex, errorMessage);
            throw new DynamoDbRetryExceededException(errorMessage);
        }
        finally
        {
            stopwatch.Stop();

            Logger.LogTrace($"Finished DynamoDb Request Method={nameof(SaveAsync)} Type={typeof(TEntity).Name} " +
                            $"PartitionKey={entity.PartitionKey} SortKey={entity.SortKey} DurationMillis={stopwatch.ElapsedMilliseconds}");
        }
    }

    public async Task<TEntity> GetItemAsync<TEntity>(string partitionKey, object sortKey, bool consistentRead)
        where TEntity : DynamoDbEntity
    {
        Logger.LogTrace($"Calling {nameof(GetItemAsync)} EntityType={typeof(TEntity).Name} PartitionKey={partitionKey} SortKey={sortKey}" +
                        $" TablePrefix={TableNamePrefix}");

        var config = new DynamoDBOperationConfig
        {
            ConsistentRead = consistentRead,
            TableNamePrefix = TableNamePrefix,
        };

        var stopwatch = Stopwatch.StartNew();

        try
        {
            return await _dynamoPolicy.ExecuteAsync(
                async (ctx, ct) => (await Connection.Context.QueryAsync<TEntity>(
                        partitionKey, QueryOperator.Equal, new[] { sortKey }, config).GetRemainingAsync(ct))
                    .FirstOrDefault(),
                new Context(nameof(GetItemAsync)),
                CancellationToken.None);
        }
        catch (AmazonServiceException ase)
        {
            Logger.LogError(GetDetailsFromAmazonServiceException(ase));
        }
        catch (Exception ex)
        {
            var errorMessage = $"Exceeded retries Method={nameof(SaveAsync)} Type={typeof(TEntity).Name} " +
                               $"PartitionKey={partitionKey} SortKey={sortKey}";
            Logger.LogError(ex, errorMessage);
            throw new DynamoDbRetryExceededException(errorMessage);
        }
        finally
        {
            stopwatch.Stop();
            Logger.LogTrace($"Finished DynamoDb Request Method={nameof(SaveAsync)} Type={typeof(TEntity).Name} " +
                            $"PartitionKey={partitionKey} SortKey={sortKey} DurationMillis={stopwatch.ElapsedMilliseconds}");
        }

        throw new Exception();
    }

    public virtual async Task DeleteAsync<TEntity>(TEntity entity)
        where TEntity : DynamoDbEntity
    {
        Logger.LogTrace($"Calling {nameof(DeleteAsync)} EntityType={typeof(TEntity).Name} PartitionKey={entity.PartitionKey} SortKey={entity.SortKey}" +
                        $" TablePrefix={TableNamePrefix}");

        var stopwatch = Stopwatch.StartNew();

        var operationConfig = new DynamoDBOperationConfig { TableNamePrefix = TableNamePrefix };

        try
        {
            await _dynamoPolicy.ExecuteAsync(
                async (ctx, ct) => await Connection.Context.DeleteAsync(entity, operationConfig, ct),
                new Context(nameof(DeleteAsync)),
                CancellationToken.None);
        }
        catch (AmazonServiceException ase)
        {
            Logger.LogError(GetDetailsFromAmazonServiceException(ase));
        }
        catch (Exception ex)
        {
            var errorMessage = $"Exceeded retries Method={nameof(SaveAsync)} PartitionKey={entity.PartitionKey} SortKey={entity.SortKey}";
            Logger.LogError(ex, errorMessage);
            throw new DynamoDbRetryExceededException(errorMessage);
        }
        finally
        {
            stopwatch.Stop();
            Logger.LogTrace($"Finished DynamoDb Request Method={nameof(DeleteAsync)}" +
                            $"PartitionKey={entity.PartitionKey} SortKey={entity.SortKey} DurationMillis={stopwatch.ElapsedMilliseconds}");
        }
    }

    private static string GetDetailsFromAmazonServiceException(AmazonServiceException ase)
    {
        return $"ErrorMessage={ase.Message}, HTTPStatus={ase.StatusCode}, AWSErrorCode={ase.ErrorCode}, ErrorType={ase.ErrorType}, " +
               $"RequestID={ase.RequestId}";
    }

    private AsyncPolicy BuildPolicy()
    {
        // Timeout policy
        var timeoutPolicy = Policy.TimeoutAsync(
            timeout: _dynamoDbConfig.Timeout,
            timeoutStrategy: TimeoutStrategy.Pessimistic,
            onTimeoutAsync: (context, span, arg3) =>
            {
                var timeoutMessage = $"DynamoDb request timeout Span={span} Method={context.OperationKey}";
                throw new TimeoutException(timeoutMessage);
            });

        // The policy to retry on exceptions
        var retryPolicy = Policy.Handle<Exception>().RetryAsync(
            retryCount: Constants.DynamoDb.Retries,
            onRetry: (exception, retryCount, context) =>
            {
                if (exception is AmazonServiceException ase)
                {
                    var logErrorMessage = $"AmazonServiceException: {GetDetailsFromAmazonServiceException(ase)} " +
                                          $"on DynamoDb operation, retrying, OperationKey={context.OperationKey} RetryCount={retryCount}";
                    Logger.LogError(ase, logErrorMessage);
                    return;
                }

                Logger.LogError($"{exception.GetType().Name}: {exception.Message} on DynamoDb operation, retrying, " +
                                $"OperationKey={context.OperationKey} RetryCount={retryCount}");
            });

        return Policy.WrapAsync(retryPolicy, timeoutPolicy);
    }
}

public interface IDynamoDbContext
{
    Task SaveAsync<TEntity>(TEntity entity)
        where TEntity : DynamoDbEntity;

    Task<TEntity> GetItemAsync<TEntity>(string partitionKey, object sortKey, bool consistentRead)
        where TEntity : DynamoDbEntity;

    Task DeleteAsync<TEntity>(TEntity entity)
        where TEntity : DynamoDbEntity;
}
