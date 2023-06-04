using SampleTest.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SampleTest.Api.Controllers;
using SampleTest.Api.Dtos;
// using SampleTest.Api.DTOs;
using SampleTest.Api.Entiites;
using SampleTest.Api.Repositories;

namespace SampleTest.UnitTests;

public class ItemsControllerTest
{
    private readonly Mock<IInMemItemsRepository> repositoryStub = new Mock<IInMemItemsRepository>();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new Mock<ILogger<ItemsController>>();
    private readonly Random random = new();

    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
        // Arrange
        // var repositoryStub = new Mock<IInMemItemsRepository>();
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null);
        // var loggerStub = new Mock<ILogger<ItemsController>>();

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        result.Result.Should().BeOfType<NotFoundResult>();
    }


// Another test for whenthe item actually exists
// Since the stubs would be reused, we can declare them as class level fileds so that they can reused multiple times
    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
        // Arrange
        var expectedItem = CreateRandomItem();
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedItem);

        // FOr this test case, we need an item that we would be using across in the test, becaus ein this usecase, where it returns an item that is converted as Dto, so we would need to set it up before test
        // Now instead of creating the item on the fly for the test case, we can have like a helper function that we can use in other tests also to create random items quickly.

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        // Assert
        // Assert.IsType<ItemDto>(result.Value);
        // var dto = (result as ActionResult<ItemDto>).Value;
        // Assert.Equal(expectedItem.Id, dto.Id);
        // Assert.Equal(expectedItem.Name, dto.Name);

        // TO avoid listing all of these in the case of a lot of object properties, we'll use fluent assertions
        // result.Value.Should().BeEquivalentTo(expectedItem, options => options.ComparingByMembers<Item>());

//      Now that we have the object type as a class and not a record we can just pass the BeEquivalentTo without theoptions as explkained above
        result.Value.Should().BeEquivalentTo(expectedItem);
        // result.Value.Should().BeEquivalentTo(expectedItem.asDto());
        


        // So this checks that both objects, by comapring the properties of the resulting Dtos
        // Note that recordtypes already overrite the equals method of the object and this does not make it behave properly
        // We add an additional method ComparingByMembers, which tells it to not compare the Dto directly to the item. but focuson the properties name and values
    }

    // So now to test the GetItemsAsync method that returns all the items,

    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItem()
    {
        // Arrange
        var expectedItems = new[] {CreateRandomItem(), CreateRandomItem(), CreateRandomItem()};
        repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(expectedItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var actualItems = await controller.GetItemsAsync();

        // Assert
        actualItems.Should().BeEquivalentTo(expectedItems, options => options.ComparingByMembers<Item>());

        // Here also, we should not need to pass the options since it is now a classs
    }

    // Testing the CreateItemAsync Method
    [Fact]
    public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
    {
        // Arrange
        // Old implmentation, new implemetation takes in the parameters as conbstructor
        // var itemToCreate  = new CreateItemDto() {
        //     Name = Guid.NewGuid().ToString(),
        //     Price = random.Next(1000)
        // };
        

        var itemToCreate  = new CreateItemDto(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            random.Next(1000)
        );
        

        // In this case we do not need to setup the repository becasue we are not exactly interested in what happens when the repository is invoked to create the item
        // Unit tests should also receive some input to the method and validate the output to the method and not what happens inside.
        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.CreateItemAsync(itemToCreate);

        // Assert
        var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
        itemToCreate.Should().BeEquivalentTo(
            createdItem, 
            options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
        );
        // But remember that both of the DTOs do not have the same members...where the itemsDto has 4 properties and the createitemdto has onyl 2 and so it will faill,
        // so we can specify to only look at properites that have similar properties

        // But to now explicitly check for the additional items
        createdItem.Id.Should().NotBeEmpty();
        createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
        // Depending on how fast the test executes, we would expect for the created time to be close enough to it's current time
    }


    // Now for the updateItems, we test for 2 things, for the notfound and the found and update and return no content usecase
    [Fact]
    public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
    {
        // Arrange

        Item existingItem = CreateRandomItem();
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(existingItem);

        var itemId = existingItem.Id;
        // var itemToUpdate = new UpdateItemDto()
        // {
        //     Name =  Guid.NewGuid().ToString(),
        //     Price = existingItem.Price + 3
        // };

        var itemToUpdate = new UpdateItemDto(
           Guid.NewGuid().ToString(),
           Guid.NewGuid().ToString(),
            existingItem.Price + 3
        );
        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
    {
        // Arrange

        Item existingItem = CreateRandomItem();
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(existingItem);

        var itemId = existingItem.Id;
        
        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.DeleteItemAsync(itemId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        
    }

    // TDD in practice implemetation, writing the test case first
    [Fact]
    public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
    {
        // Arrange
        var allItems = new[] 
        {
            new Item() {Name = "Potion"}, 
            new Item() {Name = "Antidote"}, 
            new Item() {Name = "Hi-Potion"}, 
        };
        var nameToMatch = "Potion";

        repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(allItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);
        // Here we explicitly tell C# the type of object we are expecting and we pass inthe proposed signatire to the controlle rmetjos

        // Assert
        foundItems.Should().OnlyContain(
            item => item.Name == allItems[0].Name || item.Name ==  allItems[2].Name
        );
        // Here we are explictly asserting that the result should only match the first and last potion as detailed above

        // So the test case is expected to fail here because we do not have a method that implments it
        // At first run, it fails because we do not have the GetItemsAsync method that takes int he name as expected, thosis the red phase
        // Then we implenent the method, you can click ctrl . to generate a method in the controller

    }


    private Item CreateRandomItem()
    // So the method returns a random item, that we do not really care about the Guid or Name    
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Price = random.Next(1000),
            CreatedDate = DateTimeOffset.UtcNow
        };
        
    }
    
}