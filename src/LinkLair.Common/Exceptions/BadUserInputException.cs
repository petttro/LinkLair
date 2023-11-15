namespace LinkLair.Common.Exceptions;

public class BadUserInputException : BaseInputException
{
    public BadUserInputException(string message)
        : base(message)
    {
    }
}
