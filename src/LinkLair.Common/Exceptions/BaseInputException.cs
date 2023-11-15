using System;
using System.Collections.Generic;
using LinkLair.Common.Extensions;

namespace LinkLair.Common.Exceptions;

public class BaseInputException : Exception
{
    public readonly IDictionary<string, object> InputErrors;
    public CustomErrorCode CustomErrorCode = CustomErrorCode.BadUserInputDefault;

    public BaseInputException(string message, Exception innerException, CustomErrorCode customErrorCode)
        : base(message, innerException)
    {
        CustomErrorCode = customErrorCode;
    }

    public BaseInputException(string message)
        : base(message)
    {
    }

    public BaseInputException(string message, CustomErrorCode customErrorCode)
        : base(message)
    {
        CustomErrorCode = customErrorCode;
    }

    public BaseInputException(CustomErrorCode customErrorCode)
        : base(customErrorCode.GetEnumDescription())
    {
        CustomErrorCode = customErrorCode;
    }

    public BaseInputException(IDictionary<string, object> inputErrors)
        : base(CustomErrorCode.BadUserInputDefault.GetEnumDescription())
    {
        InputErrors = inputErrors;
    }

    public BaseInputException()
    {
    }

    public BaseInputException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public string CustomOverrideMessage { get; set; }
}
