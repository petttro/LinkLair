using LinkLair.Common.DomainObjects;
using LinkLair.Common.DynamoDb;
using LinkLair.Data.DataEntities;

namespace LinkLair.Data.Repositories;

public class ClientResourceRepository : IClientResourceRepository
{
    private readonly IDynamoDbContext _dynamoDbContext;
    private const string sortKeyValue = "clientResource";

    public ClientResourceRepository(IDynamoDbContext dynamoDbContext)
    {
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<ClientResource> GetByIdAsync(string resourceKey)
    {
        var partitionKey = $"resourcekey_{resourceKey}".ToLower();
        var sortKey = sortKeyValue;

        var clientResourceDataEntity = await _dynamoDbContext.GetItemAsync<ClientResourceDataEntity>(partitionKey, sortKey, false);
        if (clientResourceDataEntity == null)
        {
            return null;
        }

        var clientResource = new ClientResource
        {
            ResourceKey = clientResourceDataEntity.ResourceKey,
            Payload = clientResourceDataEntity.Payload
        };

        return clientResource;
    }

    public async Task SaveAsync(ClientResource clientResource)
    {
        var subscriptionEntity = new ClientResourceDataEntity
        {
            PartitionKey = $"resourcekey_{clientResource.ResourceKey}".ToLower(),
            SortKey = sortKeyValue,
            Payload = clientResource.Payload,
            Updated = DateTime.UtcNow
        };

        await _dynamoDbContext.SaveAsync(subscriptionEntity);
    }

    public async Task DeleteByIdAsync(string resourceKey)
    {
        var deletingEntity = new ClientResourceDataEntity
        {
            PartitionKey = $"resourcekey_{resourceKey}".ToLower(),
            SortKey = sortKeyValue
        };
        await _dynamoDbContext.DeleteAsync(deletingEntity);
    }
}
