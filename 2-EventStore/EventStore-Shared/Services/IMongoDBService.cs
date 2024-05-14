using MongoDB.Driver;

namespace EventStore_Shared.Services;

public interface IMongoDBService
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
    IMongoDatabase GetDatabase(string databaseName, string connectionString);
}