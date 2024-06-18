using Application.Core.Entities;
using Microsoft.Azure.Cosmos;

namespace Application.Infrastructure.Data;

public interface ICosmosDbRepository<T> where T : BaseEntity
{
    Task AddItemAsync(T item);
    Task<T?> GetItemAsync(string id);
    Task<IEnumerable<T>> GetItemsAsync(QueryDefinition queryDefinition);
    Task UpdateItemAsync(string id, T item);
}