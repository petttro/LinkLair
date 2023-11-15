namespace LinkLair.Common;

public static class Constants
{
    public static class DynamoDb
    {
        public const int Retries = 3;
        public const int TimeoutInSeconds = 3;
        public const string LocalDynamoDbUrl = "http://localhost:8000";
        public const string LocalDynamoDbTablePrefix = "local";
    }

    public static class EnvironmentVariables
    {
        public const string DynamoDbTablePrefix = "DYNAMODB_TABLE_PREFIX";
    }
}
