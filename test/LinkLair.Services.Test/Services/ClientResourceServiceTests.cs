using System.Threading.Tasks;
using LinkLair.Common.DomainObjects;
using LinkLair.Common.Exceptions;
using LinkLair.Common.Test;
using LinkLair.Data.Repositories;
using LinkLair.Services.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace LinkLair.Services.Tests.Services;

public class ClientResourceServiceTests : MockStrictBehaviorTest
{
    private readonly Mock<IClientResourceRepository> _subscriptionRepository;
    private readonly ClientResourceService _clientResourceService;

    private readonly IMemoryCache _memoryCacheMock;

    public ClientResourceServiceTests()
    {
        _subscriptionRepository = _mockRepository.Create<IClientResourceRepository>(MockBehavior.Strict);
        _memoryCacheMock = new MockMemoryCache();

        _clientResourceService = new ClientResourceService(new NullLogger<ClientResourceService>(), _subscriptionRepository.Object, _memoryCacheMock);
    }

    [Fact]
    public async Task GetAsync_Success()
    {
        var ResourceKey = "subscription_id";

        var clientResource = new ClientResource
        {
            ResourceKey = ResourceKey
        };

        _subscriptionRepository
            .Setup(c => c.GetByIdAsync(ResourceKey))
            .ReturnsAsync(clientResource);

        await _clientResourceService.GetAsync(ResourceKey);
    }

    [Fact]
    public async Task GetAsync_NotFound_ThrowsException()
    {
        var ResourceKey = "subscription_id";

        _subscriptionRepository
            .Setup(c => c.GetByIdAsync(ResourceKey))
            .ReturnsAsync((ClientResource)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _clientResourceService.GetAsync(ResourceKey));
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        var ResourceKey = "subscription_id";

        var clientResource = new ClientResource
        {
            ResourceKey = ResourceKey
        };

        _subscriptionRepository
            .Setup(c => c.SaveAsync(clientResource))
            .Returns(Task.CompletedTask);

        await _clientResourceService.UpdateAsync(clientResource);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        var ResourceKey = "subscription_id";

        _subscriptionRepository
            .Setup(c => c.DeleteByIdAsync(ResourceKey))
            .Returns(Task.CompletedTask);

        await _clientResourceService.DeleteAsync(ResourceKey);
    }
}
