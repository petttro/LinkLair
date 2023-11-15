using System;

namespace LinkLair.Common.Exceptions;

public class UnauthorizedException : BaseInputException
{
    public static readonly CustomErrorCode DefaultCustomErrorCode = CustomErrorCode.UnauthorizedDefault;

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException, DefaultCustomErrorCode)
    {
    }

    public UnauthorizedException(string message, Exception innerException, CustomErrorCode customErrorCode)
        : base(message, innerException, customErrorCode)
    {
    }

    public UnauthorizedException(string message)
        : base(message, DefaultCustomErrorCode)
    {
    }

    public UnauthorizedException(string message, CustomErrorCode customErrorCode)
        : base(message, customErrorCode)
    {
    }

    public UnauthorizedException(CustomErrorCode customErrorCode)
        : base(customErrorCode)
    {
    }

    public UnauthorizedException()
    {
    }
}
