using LinkLair.Common.DomainObjects;
using LinkLair.Common.DynamoDb;
using LinkLair.Common.Test;
using LinkLair.Data.DataEntities;
using LinkLair.Data.Repositories;
using Moq;
using Xunit;

namespace LinkLair.Data.Test.Repositories;

public class ClientResourceRepositoryTests : MockStrictBehaviorTest
{
    private readonly IClientResourceRepository _clientResourceRepository;
    private readonly Mock<IDynamoDbContext> _dynamoDbContextMock;

    private string _sortKey = "clientResource";

    public ClientResourceRepositoryTests()
    {
        _dynamoDbContextMock = _mockRepository.Create<IDynamoDbContext>();
        _clientResourceRepository = new ClientResourceRepository(_dynamoDbContextMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_Success()
    {
        var resourceKey = "google/v1/moon/US";
        var partitionKey = $"resourcekey_{resourceKey}".ToLower();

        var clientResourceDataEntity = new ClientResourceDataEntity()
        {
            Payload = "sadfk",
            PartitionKey = partitionKey,
            SortKey = _sortKey,
            ResourceKey = resourceKey,
            Updated = DateTime.UtcNow
        };

        _dynamoDbContextMock
            .Setup(c => c.GetItemAsync<ClientResourceDataEntity>(partitionKey, _sortKey, false))
            .ReturnsAsync(clientResourceDataEntity);

        var result = await _clientResourceRepository.GetByIdAsync(resourceKey);

        Assert.Equal(resourceKey, result.ResourceKey);
        Assert.Equal(clientResourceDataEntity.Payload, result.Payload);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        var resourceKey = "google/v1/moon/US";
        var partitionKey = $"resourcekey_{resourceKey}".ToLower();

        _dynamoDbContextMock
            .Setup(c => c.GetItemAsync<ClientResourceDataEntity>(partitionKey, _sortKey, false))
            .ReturnsAsync((ClientResourceDataEntity)null);

        var result = await _clientResourceRepository.GetByIdAsync(resourceKey);

        Assert.Null(result);
    }

    [Fact]
    public async Task SaveAsync_Success()
    {
        var resourceKey = "google/v1/moon/US";
        var partitionKey = $"resourcekey_{resourceKey}".ToLower();

        var clientResource = new ClientResource
        {
            Payload = "sadfk",
            ResourceKey = resourceKey
        };

        _dynamoDbContextMock
            .Setup(c => c.SaveAsync(It.Is<ClientResourceDataEntity>(e => e.PartitionKey.Equals(partitionKey) && e.Payload.Equals(clientResource.Payload))))
            .Returns(Task.CompletedTask);

        await _clientResourceRepository.SaveAsync(clientResource);
    }

    [Fact]
    public async Task DeleteByIdAsync_Success()
    {
        var resourceKey = "google/v1/moon/US";
        var partitionKey = $"resourcekey_{resourceKey}".ToLower();

        _dynamoDbContextMock
            .Setup(c => c.DeleteAsync(It.Is<ClientResourceDataEntity>(e => e.PartitionKey.Equals(partitionKey) && e.SortKey.Equals(_sortKey))))
            .Returns(Task.CompletedTask);

        await _clientResourceRepository.DeleteByIdAsync(resourceKey);
    }
}
