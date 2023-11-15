using LinkLair.Common.DomainObjects;

namespace LinkLair.Data.Repositories;

public interface IClientResourceRepository
{
    Task<ClientResource> GetByIdAsync(string resourceKey);

    Task SaveAsync(ClientResource clientResource);

    Task DeleteByIdAsync(string resourceKey);
}
