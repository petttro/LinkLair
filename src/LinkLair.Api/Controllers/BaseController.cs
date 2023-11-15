using System.Net;
using LinkLair.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkLair.Api.Controllers;

[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
public abstract class BaseController : Controller
{
    protected IActionResult StatusCode(HttpStatusCode statusCode, object value = null)
    {
        return StatusCode((int)statusCode, value);
    }
}
