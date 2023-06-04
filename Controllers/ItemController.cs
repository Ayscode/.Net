using Microsoft.AspNetCore.Mvc;
using SampleTest.Entiites;
using SampleTest.Repositories;

namespace SampleTest.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ItemController : ControllerBase
    {
        // TO be able to use it here, we bring it in as readonly since we are not going to modify it. Explicit dependency is not ideal
        private readonly inMemItemsRepository repository;

        public ItemController()
        {
            repository = new inMemItemsRepository();
            
        }

        // Get /items
        [HttpGet]
        public IEnumerable<Item> GetItems()
        {
            var items =  repository.GetItems();
            return items;
        }

        // GET .items/{id}
        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {
            var item = repository.GetItem(id);

            if (item is null)
            {
                return NotFound();
                // returns the proper status code, but becuase they are of different types, items and statsu code,
                // we use Action Result to return more than one type  
            }
            return Ok(item);
        }

    }


}