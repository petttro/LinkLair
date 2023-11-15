namespace LinkLair.Common.Exceptions;

public class ForbiddenException : BaseInputException
{
    public static readonly CustomErrorCode DefaultCustomErrorCode = CustomErrorCode.ForbiddenDefault;

    public ForbiddenException(string message)
        : base(message, DefaultCustomErrorCode)
    {
    }
}
