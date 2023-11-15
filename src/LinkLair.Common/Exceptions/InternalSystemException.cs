using System;

namespace LinkLair.Common.Exceptions;

public class InternalSystemException : Exception
{
    public InternalSystemException()
    {
    }

    public InternalSystemException(string message)
        : base(message)
    {
    }

    public InternalSystemException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public string CustomOverrideMessage { get; set; }
}
