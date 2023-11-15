using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LinkLair.Common.DynamoDb;

public class DynamoDbConnection : IDynamoDbConnection
{
    private AmazonDynamoDBClient _client;
    private readonly DynamoDbConfig _dynamoDbConfig;

    public DynamoDbConnection(ILogger<DynamoDbConnection> logger, IOptions<DynamoDbConfig> dynamoDbConfigOptions)
    {
        logger.LogInformation("Creating new DynamoDbContext");

        _dynamoDbConfig = dynamoDbConfigOptions.Value;

        Context = new DynamoDBContext(Client);
    }

    public IDynamoDBContext Context { get; }

    public AmazonDynamoDBClient Client
    {
        get
        {
            if (_client != null)
            {
                return _client;
            }

            var config = new AmazonDynamoDBConfig
            {
                Timeout = TimeSpan.FromSeconds(Constants.DynamoDb.TimeoutInSeconds),
                MaxErrorRetry = Constants.DynamoDb.Retries,
                LogMetrics = false
            };

            if (_dynamoDbConfig.TablePrefix == Constants.DynamoDb.LocalDynamoDbTablePrefix)
            {
                config.ServiceURL = Constants.DynamoDb.LocalDynamoDbUrl;
            }

            _client = new AmazonDynamoDBClient(config);

            return _client;
        }
    }
}
