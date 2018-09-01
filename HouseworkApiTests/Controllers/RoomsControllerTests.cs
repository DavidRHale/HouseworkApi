using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// using AutoMapper;
using HouseworkApi.Controllers;
using HouseworkApi.Data;
using HouseworkApi.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AutoMapper;

namespace HouseworkApiTests
{
  public class RoomsControllerTests
  {
    RoomsController controller;
    Mock<IHouseworkRepository> mockRepo;
    Mock<IMapper> mockMapper;

    readonly int kitchenId = 1;
    Room kitchen;
    RoomViewModel kitchenViewModel;

    readonly Room nullRoom = null;

    public RoomsControllerTests()
    {
      mockRepo = new Mock<IHouseworkRepository>();
      mockMapper = new Mock<IMapper>();

      kitchen = new Room() {
        Id = kitchenId,
        Name = "Kitchen"
      };
      kitchenViewModel = new RoomViewModel() {
        RoomId = kitchenId,
        Name = "Kitchen"
      };

      controller = new RoomsController(mockRepo.Object, mockMapper.Object);
    }

    [Fact]
    public void GetAllRooms_ReturnsOkObjectResult_WhenFound()
    {
      mockRepo.Setup(r => r.GetAllRooms())
              .Returns(new List<Room>() {kitchen});

      var result = controller.Get();

      result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void GetAllRooms_ReturnsRoomsViewModels_FromRepository_WhenFound()
    {
      mockRepo.Setup(r => r.GetAllRooms())
              .Returns(new List<Room>() {kitchen});
      mockMapper.Setup(m => m.Map<List<RoomViewModel>>(It.IsAny<List<Room>>()))
                .Returns(new List<RoomViewModel>() {kitchenViewModel});

      var result = controller.Get();

      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      var value = okResult.Value.Should().BeAssignableTo<IEnumerable<RoomViewModel>>().Subject;
    }

    [Fact]
    public void GetAllRooms_ReturnsNotFoundResult_WithoutRooms_WhenNotFound()
    {  
      mockRepo.Setup(r => r.GetAllRooms())
                           .Returns(new List<Room>());

      var result = controller.Get();

      result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void GetAllRooms_ReturnsStatus500_WhenErrorThrown()
    {
      mockRepo.Setup(r => r.GetAllRooms()).Throws(new Exception());

      var result = controller.Get();
      
      mockRepo.Verify(r => r.GetAllRooms(), Times.Once);  
      var statusResult = result.Should().BeOfType<StatusCodeResult>().Subject;
      statusResult.StatusCode.Should().Be(500);
    }

    [Fact]
    public void GetRoomById_ReturnsOkResult_WithRoom_WhenFound()
    {
      mockMapper.Setup(m => m.Map<RoomViewModel>(kitchen))
                .Returns(kitchenViewModel);
      mockRepo.Setup(r => r.GetRoomById(kitchenId))
              .Returns(kitchen);

      var result = controller.Get(kitchenId);

      mockRepo.Verify(r => r.GetRoomById(kitchenId), Times.Once);
      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      var RoomViewModel = okResult.Value.Should().BeAssignableTo<RoomViewModel>().Subject;
      RoomViewModel.Name.Should().Be(kitchen.Name);
      RoomViewModel.RoomId.Should().Be(kitchenId);
    }

    [Fact]
    public void GetRoomById_ReturnsNotFound_WhenRoomNotFound()
    {
      mockRepo.Setup(r => r.GetRoomById(500))
              .Returns(nullRoom);

      var result = controller.Get(500);

      result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void GetRoomById_ReturnsStatus500_WhenExceptionThrown()
    {
      var id = 99;
      mockRepo.Setup(r => r.GetRoomById(id))
              .Throws(new Exception());

      var result = controller.Get(id);

      var status = result.Should().BeOfType<StatusCodeResult>().Subject;
      status.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void PostRoom_ReturnsCreated_WhenSuccessful()
    {
      mockMapper.Setup(m => m.Map<Room>(It.IsAny<RoomViewModel>()))
                .Returns(kitchen);
      mockMapper.Setup(m => m.Map<RoomViewModel>(It.IsAny<Room>()))
                .Returns(kitchenViewModel);
      mockRepo.Setup(r => r.SaveAllAsync())
              .Returns(Task.FromResult(true));

      var result = await controller.Post(kitchenViewModel);

      var createdResult = result.Should().BeAssignableTo<CreatedAtRouteResult>().Subject;
      var viewModel = createdResult.Value.Should().BeAssignableTo<RoomViewModel>().Subject;
      viewModel.Name.Should().Be("Kitchen");
    }
  }
}
