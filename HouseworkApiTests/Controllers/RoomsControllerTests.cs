using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// using AutoMapper;
using HouseworkApi.Controllers;
using HouseworkApi.Data;
// using HouseworkApi.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
// using Moq;
using Xunit;

namespace HouseworkApiTests
{
  public class RoomsControllerTests
  {
    [Fact]
    public void GetAllRooms_ReturnsOkResult_WithRoomsAsViewModels_WhenFound()
    {
      var controller = new RoomsController();

      var result = controller.Get();

      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;




      // var value = okResult.Value.Should().BeAssignableTo<IEnumerable<RoomViewModel>>().Subject;




      // var mockMapper = new Mock<IMapper>();
      // var mockLogger = new Mock<ILogger<CategoriesController>>();
      // var mockRepo = new Mock<IHouseworkRepository>();
      // mockRepo.Setup(r => r.GetAllCategories())
      //                      .Returns(new List<Room>() { 
      //                        new Room() { Id = Guid.NewGuid(), Name = "Anatomy" }, 
      //                        new Room() { Id = Guid.NewGuid(), Name = "Microbiology" }
      //                      });

      // var controller = new CategoriesController(mockLogger.Object, mockRepo.Object, mockMapper.Object);

      // var result = controller.Get();
      
      // var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      // var value = okResult.Value.Should().BeAssignableTo<IEnumerable<RoomViewModel>>().Subject;

      // mockRepo.Verify(r => r.GetAllCategories(), Times.Once);
    }
  }
}
