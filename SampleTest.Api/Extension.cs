using SampleTest.Api.Dtos;
// using SampleTest.Api.DTOs;
using SampleTest.Api.Entiites;

namespace SampleTest.Api
{
    public static class Extensions
    {
        public static ItemDto asDto(this Item item)
        {
            // return new ItemDto
            // {
            //     Id = item.Id,
            //     Name = item.Name,
            //     Price = item.Price,
            //     CreatedDate = item.CreatedDate
            // };
            // Now that we have changed the Dtos as record types, they are now immutable and they must be created from the constructors and no one would be abel to change their properties
            return new ItemDto(item.Id, item.Name, item.Description ,item.Price, item.CreatedDate);

// So the extension here basically extends the functionality of the entity class, by easily converting it to it's Dto and it can be easily resueed anywhere else
        }
    }
}