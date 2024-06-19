using Application.Core.Entities;
using Microsoft.Azure.Cosmos;

namespace Application.Infrastructure.Data;

public class CosmosDbRepository<T> : ICosmosDbRepository<T> where T : BaseEntity
{
    private readonly Container _container;

    public CosmosDbRepository(CosmosClient cosmosClient)
    {
        Database database = cosmosClient.GetDatabase("DynamicProgram");
        _container = cosmosClient.GetContainer(database.Id, typeof(T).Name);
    }

    public async Task AddItemAsync(T item) =>
        await _container.CreateItemAsync(item, new PartitionKey(item.Id));

    public async Task<T?> GetItemAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async Task<List<TSlim>> GetItemsAsync<TSlim>(QueryDefinition queryDefinition)
    {
        try
        {
            var query = _container.GetItemQueryIterator<TSlim>(queryDefinition);
            List<TSlim> results = [];
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return [];
        }
    }

    public async Task<List<T>> GetItemsAsync(QueryDefinition queryDefinition)
    {
        try
        {
            var query = _container.GetItemQueryIterator<T>(queryDefinition);
            List<T> results = [];
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return [];
        }
    }

    public async Task UpdateItemAsync(T item) => await _container.UpsertItemAsync(item);
}
