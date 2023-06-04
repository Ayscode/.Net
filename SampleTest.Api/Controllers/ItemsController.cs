using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using SampleTest.Api.Dtos;
// using SampleTest.Api.DTOs;
using SampleTest.Api.Entiites;
using SampleTest.Api.Repositories;

namespace SampleTest.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ItemsController : ControllerBase
    {
        // TO be able to use it here, we bring it in as readonly since we are not going to modify it. Explicit dependency is not ideal
        private readonly IInMemItemsRepository repository;
        private readonly ILogger<ItemsController> logger; //just add a simple depedency injection of the logger

        public ItemsController(IInMemItemsRepository repository, ILogger<ItemsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // // Get /items
        // [HttpGet]
        // public IEnumerable<ItemDto> GetItems()
        // {
        //     // var items =  repository.GetItems().Select(item=> new ItemDto
        //     // {
        //     //     Id = item.Id,
        //     //     Name = item.Name,
        //     //     Price = item.Price,
        //     //     CreatedDate = item.CreatedDate
        //     // });
        //     var items =  repository.GetItemsAsync().Select(item=> item.asDto());
        //     return items;
        // }

        // // GET /items/{id}
        // [HttpGet("{id}")]
        // public ActionResult<ItemDto> GetItem(Guid id)
        // {
        //     var item = repository.GetItemAsync(id);

        //     if (item is null)
        //     {
        //         return NotFound();
        //         // returns the proper status code, but becuase they are of different types, items and statsu code,
        //         // we use Action Result to return more than one type  
        //     }
        //     return Ok(item.asDto());
        // }

        // // POST /items
        // [HttpPost]
        // public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        // {
        //     Item item = new() {
        //         Id = Guid.NewGuid(),
        //         Name = itemDto.Name,
        //         Price = itemDto.Price,
        //         CreatedDate = DateTimeOffset.UtcNow
        //     };
        //     repository.CreateItemAsync(item);
        //     return CreatedAtAction(nameof(GetItem), new{id = item.Id}, item.asDto());
        // }

        // // PUT /items/{id}
        // [HttpPut("{id}")]
        // public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        // {
        //     var existingItem = repository.GetItemAsync(id);
        //     if (existingItem is null)
        //     {
        //         return NotFound();
        //     }

        //     Item updatedItem = existingItem with
        //     {
        //         Name = itemDto.Name,
        //         Price = itemDto.Price
        //     };

        //     repository.UpdateItemAsync(updatedItem);
        //     return NoContent();
        // }

        // // DELETE /items/{id}
        // [HttpDelete("{id}")]
        // public ActionResult DeleteItem(Guid id)
        // {
        //     var existingItem = repository.GetItemAsync(id);
        //     if (existingItem is null)
        //     {
        //         return NotFound();
        //     }

        //     repository.DeleteItemAsync(id);
        //     return NoContent();
        // }

        // Async implementation
         // Get /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            // var items =  repository.GetItems().Select(item=> new ItemDto
            // {
            //     Id = item.Id,
            //     Name = item.Name,
            //     Price = item.Price,
            //     CreatedDate = item.CreatedDate
            // });
            var items =  (await repository.GetItemsAsync()).Select(item=> item.asDto());

            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");
            return items;
        }

        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string name)
        {
            var items =  (await repository.GetItemsAsync()).Select(item=> item.asDto());

            if (!string.IsNullOrWhiteSpace(name))
            {
                items = items.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            // So we just use linq to filter by the name if one is passed


            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");
            return items;
            // This is an overload of the method above se we can also just add the parameter as optional above where the signature just looks like this
            //         public async Task<IEnumerable<ItemDto>> GetItemsAsync(string nameToMatch = null)
        }

        // GET /items/{id}
            [HttpGet("{id}")]
            public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
            {
                var item = await repository.GetItemAsync(id);

                if (item is null)
                {
                    return NotFound();
                    // returns the proper status code, but becuase they are of different types, items and statsu code,
                    // we use Action Result to return more than one type  
                }
                return item.asDto();
            }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new() {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                Description = itemDto.Description,   
                CreatedDate = DateTimeOffset.UtcNow
            };
            await repository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new{id = item.Id}, item.asDto());
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await repository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            // Item updatedItem = existingItem with
            // {
            //     Name = itemDto.Name,
            //     Price = itemDto.Price
            // };

            // This is the new implementstion with class type as the above implementation can only work with records
            existingItem.Name = itemDto.Name;
            existingItem.Price = itemDto.Price;


            // await repository.UpdateItemAsync(updatedItem);
            await repository.UpdateItemAsync(existingItem);
            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = repository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);
            return NoContent();
        }


    }

    // Using TDD to implement a new functionality to return all the items that include the passed Name
    // so we are starting with writing the test case forst, it fails, then it works and then we optimize



}