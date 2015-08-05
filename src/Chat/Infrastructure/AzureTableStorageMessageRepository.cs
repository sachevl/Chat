namespace Chat
{
    using System;
    using System.Threading.Tasks;

    using Chat.Core.Repositories;

    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class AzureTableStorageMessageRepository : IMessageRepository
    {
        private readonly Lazy<CloudTable> messageTable;

        public AzureTableStorageMessageRepository(ICloudTableFactory cloudTableFactory)
        {
            this.messageTable = new Lazy<CloudTable>(() => cloudTableFactory.Create("message"));
        }

        public async Task Add(Guid userId, string userName, string message)
        {
            var insertOperation = TableOperation.Insert(
                new MessageEntity(userId, userName, message, DateTime.UtcNow));
            await this.messageTable.Value.ExecuteAsync(insertOperation);
        }

        public class MessageEntity : TableEntity
        {
            public MessageEntity(Guid userId, string userName, string message, DateTime createdTime)
            {
                this.PartitionKey = (createdTime.Ticks / TimeSpan.TicksPerHour).ToString();
                this.RowKey = createdTime.Ticks + "-" + userId;

                this.UserId = userId;
                this.UserName = userName;
                this.Message = message;
                this.CreatedTime = createdTime;
            }

            public DateTime CreatedTime { get; set; }

            public string Message { get; set; }

            public string UserName { get; set; }

            public Guid UserId { get; set; }
        }
    }

    public interface ICloudTableFactory
    {
        CloudTable Create(string tableName);
    }

    public class CloudTableFactory : ICloudTableFactory
    {
        public CloudTable Create(string tableName)
        {
            var storageAccount =
                CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var tableClient = storageAccount.CreateCloudTableClient();

            var cloudTable = tableClient.GetTableReference(tableName);
            cloudTable.CreateIfNotExists();
            return cloudTable;
        }
    }
}