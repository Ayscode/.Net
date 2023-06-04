using System.Collections;
using SampleTest.Entiites;

namespace SampleTest.Repositories
{
    
    public class inMemItemsRepository
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

        // GetItems
        public IEnumerable<Item> GetItems()
        {
            return items;
            
        }

        // GetItem
        public Item GetItem(Guid Id)
        {
            return items.Where(x=>x.Id == Id).SingleOrDefault();
            
        }
    }

}

 