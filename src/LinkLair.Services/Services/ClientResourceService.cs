using System;
using System.Threading.Tasks;
using LinkLair.Common.DomainObjects;
using LinkLair.Common.Exceptions;
using LinkLair.Data.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LinkLair.Services.Services;

public class ClientResourceService : IClientResourceService
{
    private readonly ILogger<ClientResourceService> _logger;
    private readonly IClientResourceRepository _clientResourceRepository;
    private readonly IMemoryCache _memoryCache;

    private TimeSpan cacheTtl = TimeSpan.FromSeconds(5);

    public ClientResourceService(ILogger<ClientResourceService> logger, IClientResourceRepository clientResourceRepository, IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _clientResourceRepository = clientResourceRepository;
    }

    public async Task UpdateAsync(ClientResource clientResource)
    {
        _logger.LogTrace($"Calling {nameof(UpdateAsync)} with ResourceKey={clientResource.ResourceKey}");

        await _clientResourceRepository.SaveAsync(clientResource);
    }

    public async Task<ClientResource> GetAsync(string resourceKey)
    {
        _logger.LogTrace($"Calling {nameof(GetAsync)} with ResourceKey={resourceKey}");

        var clientResource = _memoryCache.Get<ClientResource>(resourceKey);
        if (clientResource != null)
        {
            _logger.LogDebug($"Got from cache ResourceKey={resourceKey}");
            return clientResource;
        }

        clientResource = await _clientResourceRepository.GetByIdAsync(resourceKey);
        if (clientResource == null)
        {
            throw new NotFoundException($"ResourceKey={resourceKey} not found in DB");
        }

        _memoryCache.Set(resourceKey, clientResource, cacheTtl);

        return clientResource;
    }

    public async Task DeleteAsync(string resourceKey)
    {
        _logger.LogTrace($"Calling {nameof(DeleteAsync)} with ResourceKey={resourceKey}");

        await _clientResourceRepository.DeleteByIdAsync(resourceKey);
    }
}
