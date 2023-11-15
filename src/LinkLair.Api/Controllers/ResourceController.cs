using System.Threading.Tasks;
using LinkLair.Common.DomainObjects;
using LinkLair.Common.Exceptions;
using LinkLair.Security.Authorization.Policies;
using LinkLair.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LinkLair.Api.Controllers;

[Route("api/[controller]")]
public class ResourceController : BaseController
{
    private readonly IClientResourceService _clientResourceService;

    public ResourceController(IClientResourceService clientResourceService)
    {
        _clientResourceService = clientResourceService;
    }

    [HttpGet]
    [Authorize(AllowClientsWithLinkLairReadAccessPolicy.Name)]
    [Route("{**resourceKey}")]
    public async Task<ActionResult> GetAsync(string resourceKey)
    {
        var clientResource = await _clientResourceService.GetAsync(resourceKey);
        var responsePayload = JsonConvert.DeserializeObject(clientResource.Payload);

        return Ok(responsePayload);
    }

    [HttpPut]
    [Authorize(AllowClientsWithLinkLairChangeAccessPolicy.Name)]
    [Route("{**resourceKey}")]
    public async Task<ActionResult> UpsertAsync(string resourceKey, [FromBody] dynamic payload)
    {
        if (payload is null)
        {
            throw new BadUserInputException("Request body should be a string or valid JSON object");
        }

        var clientResource = new ClientResource
        {
            ResourceKey = resourceKey,
            Payload = JsonConvert.SerializeObject(payload)
        };

        await _clientResourceService.UpdateAsync(clientResource);

        return Ok();
    }

    [HttpDelete]
    [Authorize(AllowClientsWithLinkLairDeleteAccessPolicy.Name)]
    [Route("{**resourceKey}")]
    public async Task<ActionResult> DeleteAsync(string resourceKey)
    {
        await _clientResourceService.DeleteAsync(resourceKey);

        return Ok();
    }
}
