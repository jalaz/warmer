using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Warmer.AuthorizationServer.Repositories
{
    public class AzureTableUserRepository
    {
        private readonly string tableName;
        private readonly CloudStorageAccount storageAccount;

        public AzureTableUserRepository(string connectionString, string tableName)
        {
            this.tableName = tableName;
            this.storageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public async Task Register(User user, string password)
        {
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            var userEntity = new UserEntity(user.UserId, user.Username, user.Email);
            var operation = TableOperation.Insert(userEntity);
            await table.ExecuteAsync(operation);
        }

        public User GetUser(Guid userId)
        {
            return new User(userId, "test", "test");
        }

        public bool HasUser(string email, string password)
        {
            return false;
        }
    }

    public class UserEntity : TableEntity
    {
        public UserEntity(Guid userId, string username, string email)
        {
            this.PartitionKey = "users";
            this.RowKey = email;
            this.UserId = userId;
            this.Username = username;
            this.Email = email;
        }

        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class User
    {
        public User(Guid userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }

        public Guid UserId { get; }
        public string Username { get; }
        public string Email { get; }
    }
}
