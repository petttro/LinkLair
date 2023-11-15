using System;

namespace LinkLair.Common.DynamoDb;

public class DynamoDbRetryExceededException : Exception
{
    public DynamoDbRetryExceededException(string message)
        : base(message)
    {
    }
}