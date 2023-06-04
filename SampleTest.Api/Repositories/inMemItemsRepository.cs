using System.Collections;
using SampleTest.Api.Entiites;

namespace SampleTest.Api.Repositories
{
    public class inMemItemsRepository : IInMemItemsRepository
    {
        // They are readonly because we do not expect the instance of the list to change after they are constructed
        private readonly List<Item> items = new()
        {
            new Item {Id = Guid.NewGuid(), Name="Potion", Price=9, CreatedDate=DateTimeOffset.UtcNow},
            new Item {Id = Guid.NewGuid(), Name="Sword", Price=20, CreatedDate=DateTimeOffset.UtcNow},
            new Item {Id = Guid.NewGuid(), Name="Shield", Price=15, CreatedDate=DateTimeOffset.UtcNow}
        };

        // Get methods
        // IEnumerable - basic interface used to return a collection of items
        // Controller - used for routing, class that receives e request sent by the client and handles it properly
        // When you inherit from the ControllerBase, it automatically converts it into a controller base
        // You also add an [ApiController] attribute
        // You also add the Route Atribute [Route("[Controller]")] which just takes in the name of the controller or specify the actual route [Route("items")]

        // // GetItems
        // public IEnumerable<Item> GetItems()
        // {
        //     return items;

        // }

        // // GetItem
        // public Item GetItem(Guid Id)
        // {
        //     return items.Where(x => x.Id == Id).SingleOrDefault();

        // }

        // public void CreateItem(Item item)
        // {
        //     items.Add(item);
        // }

        // public void UpdateItem(Item item)
        // {
        //     var index = items.FindIndex(existingItem => item.Id == existingItem.Id);
        //     items[index] = item;
        // }

        // public void DeleteItem(Guid Id)
        // {
        //     var index = items.FindIndex(existingItem => Id == existingItem.Id);
        //     items.RemoveAt(index);
        // }

// Note that you do not have to use async in this case
        // GetItems
        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(items);
            // Here we just telling the code to create a Task that is completed and introduce the items to it

        }

        // GetItem
        public async Task<Item> GetItemAsync(Guid Id)
        {
            var item = items.Where(x => x.Id == Id).SingleOrDefault();
            return await Task.FromResult(item);

        }

        public async Task CreateItemAsync(Item item)
        {
            items.Add(item);
            await Task.CompletedTask;
            // Just return a completed task since there is nothing to return
        }

        public async Task UpdateItemAsync(Item item)
        {
            var index = items.FindIndex(existingItem => item.Id == existingItem.Id);
            items[index] = item;
            await Task.CompletedTask;
            // Just return a completed task since there is nothing to return
        }

        public async Task DeleteItemAsync(Guid Id)
        {
            var index = items.FindIndex(existingItem => Id == existingItem.Id);
            items.RemoveAt(index);
            await Task.CompletedTask;
            // Just return a completed task since there is nothing to return
        }
    }

}

 