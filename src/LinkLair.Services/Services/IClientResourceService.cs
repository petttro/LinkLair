using System.Threading.Tasks;
using LinkLair.Common.DomainObjects;

namespace LinkLair.Services.Services;

public interface IClientResourceService
{
    Task UpdateAsync(ClientResource clientResource);

    Task DeleteAsync(string resourceKey);

    Task<ClientResource> GetAsync(string resourceKey);
}
