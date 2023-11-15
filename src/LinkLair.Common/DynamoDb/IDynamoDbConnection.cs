using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace LinkLair.Common.DynamoDb;

public interface IDynamoDbConnection
{
    IDynamoDBContext Context { get; }

    AmazonDynamoDBClient Client { get; }
}
