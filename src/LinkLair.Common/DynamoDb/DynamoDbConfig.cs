using System;

namespace LinkLair.Common.DynamoDb;

public class DynamoDbConfig
{
    public string TablePrefix => Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.DynamoDbTablePrefix);

    public TimeSpan Timeout = TimeSpan.FromSeconds(Constants.DynamoDb.TimeoutInSeconds);
}
