using System.Collections;
// using SampleTest.Api.DTOs;
using SampleTest.Api.Entiites;

namespace SampleTest.Api.Repositories
{
    public interface IInMemItemsRepository
    {
        // Item GetItem(Guid Id);
        // IEnumerable<Item> GetItems();
        // void CreateItem(Item item);
        // void UpdateItem(Item item);
        // void DeleteItem(Guid Id);

        Task<Item> GetItemAsync(Guid Id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid Id);
    }
}

 