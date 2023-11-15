using Amazon.DynamoDBv2.DataModel;
using LinkLair.Common.DynamoDb;

namespace LinkLair.Data.DataEntities;

public class ClientResourceDataEntity : DynamoDbEntity
{
    [DynamoDBProperty(AttributeName = "Payload")]
    public string Payload { get; set; }

    [DynamoDBProperty(AttributeName = "ResourceKey")]
    public string ResourceKey { get; set; }

    [DynamoDBProperty(AttributeName = "Updated")]
    public DateTime Updated { get; set; } = default(DateTime);
}
