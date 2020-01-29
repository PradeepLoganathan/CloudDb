using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CloudDb
{
    class DynamoDriver
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public DynamoDriver(IAmazonDynamoDB dynamoDbClient, IConfiguration Configuration)
        {
            
            _dynamoDbClient = dynamoDbClient;
            //Configuration.GetAWSOptions().CreateServiceClient;
        }

        public async void CreateTableAsync(string tableName)
        {
            try
            {
                var tableResponse = await _dynamoDbClient.ListTablesAsync();
                if (tableResponse.TableNames.Contains(tableName))
                    return;

                var request = new CreateTableRequest
                {
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "Id",
                            AttributeType = ScalarAttributeType.N
                        },
                        new AttributeDefinition
                        {
                            AttributeName = "ReplyDateTime",
                            AttributeType = ScalarAttributeType.N
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = KeyType.HASH // Partition Key
                        },
                        new KeySchemaElement
                        {
                            AttributeName = "ReplyDateTime",
                            KeyType = KeyType.RANGE// Sort Key
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    },
                    TableName = tableName
                };

                var response = await _dynamoDbClient.CreateTableAsync(request);

                try
                {
                    bool isTableAvailable = false;
                    while (!isTableAvailable)
                    {
                        Thread.Sleep(5000);
                        var tableStatus = await _dynamoDbClient.DescribeTableAsync(tableName);
                        isTableAvailable = tableStatus.Table.TableStatus == "ACTIVE";
                    }
                }
                catch (ResourceNotFoundException resourceNotFoundException)
                {
                    Console.WriteLine(resourceNotFoundException.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
