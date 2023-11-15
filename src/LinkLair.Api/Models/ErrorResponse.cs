using System.Collections.Generic;
using LinkLair.Common.Exceptions;

namespace LinkLair.Api.Models;

public class ErrorResponse
{
    public CustomErrorCode Code { get; set; }

    public string Message { get; set; }

    public string SplunkLink { get; set; }

    public IDictionary<string, object> InputErrors { get; set; }
}
