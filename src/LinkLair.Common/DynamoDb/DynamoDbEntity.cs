using Amazon.DynamoDBv2.DataModel;

namespace LinkLair.Common.DynamoDb;

[DynamoDBTable(TableName)]
public class DynamoDbEntity
{
    public const string TableName = "PA_LinkLair";

    [DynamoDBHashKey]
    public virtual string PartitionKey { get; set; }

    [DynamoDBRangeKey]
    public virtual string SortKey { get; set; }
}
