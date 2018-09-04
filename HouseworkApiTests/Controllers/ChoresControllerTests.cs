using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using HouseworkApi.Controllers;
using HouseworkApi.Data;
using HouseworkApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HouseworkApiTests.Controllers
{
  public class ChoresControllerTests
  {
    ChoresController controller;
    Mock<IHouseworkRepository> mockRepo;
    Mock<IMapper> mockMapper;

    readonly int mopId = 1;
    Chore mop;
    ChoreViewModel mopViewModel;
    readonly Chore nullChore = null;

    readonly int kitchenId = 88;
    Room kitchen;
    RoomViewModel kitchenViewModel;

    public ChoresControllerTests()
    {
      mockRepo = new Mock<IHouseworkRepository>();
      mockMapper = new Mock<IMapper>();

      kitchen = new Room()
      {
        Id = kitchenId,
        Name = "Kitchen"
      };
      kitchenViewModel = new RoomViewModel()
      {
        RoomId = kitchenId,
        Name = "Kitchen"
      };

      mop = new Chore()
      {
        Id = mopId,
        Name = "Mop floors",
        LastCompleted = DateTime.Now,
        Frequency = TimeSpan.FromDays(7),
        RoomId = kitchenId
      };
      mopViewModel = new ChoreViewModel()
      {
        ChoreId = mopId,
        Name = "Mop floors",
        Frequency = TimeSpan.FromDays(7),
        RoomId = kitchenId
      };

      controller = new ChoresController(mockRepo.Object, mockMapper.Object);
    }   

    [Fact]
    public void GetAllChores_ShouldReturnOk_WhenSuccessful()
    {
      mockRepo.Setup(r => r.GetAllChores())
              .Returns(new List<Chore>() { mop });

      var result = controller.Get();

      result.Should().BeAssignableTo<OkObjectResult>();
    }

    [Fact]
    public void GetAllChores_ShouldReturnChoreViewModels_FromRepository_WhenFound()
    {
      mockRepo.Setup(r => r.GetAllChores())
              .Returns(new List<Chore>() { mop });
      mockMapper.Setup(m => m.Map<List<ChoreViewModel>>(It.IsAny<List<Chore>>()))
                .Returns(new List<ChoreViewModel>() { mopViewModel });

      var result = controller.Get();

      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      var value = okResult.Value.Should().BeAssignableTo<IEnumerable<ChoreViewModel>>().Subject;
    }

    [Fact]
    public void GetAllChores_ShouldReturnNotFound_WhenChoresNotFound()
    {
      mockRepo.Setup(r => r.GetAllChores())
              .Returns(new List<Chore>());

      var result = controller.Get();

      result.Should().BeAssignableTo<NotFoundResult>();
    }

    [Fact]
    public void GetAllChores_ShouldReturnStatus500_WhenErrorThrown()
    {
      mockRepo.Setup(r => r.GetAllChores()).Throws(new Exception());

      var result = controller.Get();

      var statusResult = result.Should().BeOfType<StatusCodeResult>().Subject;
      statusResult.StatusCode.Should().Be(500);      
    }

    [Fact]
    public void GetChoreById_ShouldReturnOkResult_WithChore_WhenFound()
    {
      mockRepo.Setup(r => r.GetChoreById(mopId))
              .Returns(mop);
      mockMapper.Setup(m => m.Map<ChoreViewModel>(mop))
                .Returns(mopViewModel);
      
      var result = controller.Get(mopId);

      var okResult = result.Should().BeAssignableTo<OkObjectResult>().Subject;
      okResult.Value.Should().Be(mopViewModel);
    }

    [Fact]
    public void GetChoreById_ShouldReturnNotFound_WhenChoreNotFound()
    {
      mockRepo.Setup(r => r.GetChoreById(mopId))
              .Returns(nullChore);

      var result = controller.Get(mopId);

      result.Should().BeOfType<NotFoundResult>();     
    }

    [Fact]
    public void GetChoreById_ShouldReturnStatus500_WhenExceptionThrown()
    {
      mockRepo.Setup(r => r.GetChoreById(mopId)).Throws(new Exception());

      var result = controller.Get(mopId);

      var statusResult = result.Should().BeAssignableTo<StatusCodeResult>().Subject;
      statusResult.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void PostChore_ShouldReturnCreated_WhenSuccessful()
    {
      mockMapper.Setup(m => m.Map<Chore>(mopViewModel))
                .Returns(mop);
      mockMapper.Setup(m => m.Map<ChoreViewModel>(mop))
                .Returns(mopViewModel);
      mockRepo.Setup(r => r.SaveAllAsync())
              .Returns(Task.FromResult(true));

      var result = await controller.Post(mopViewModel);

      var createdResult = result.Should().BeAssignableTo<CreatedAtRouteResult>().Subject;
      createdResult.Value.Should().Be(mopViewModel);
    }

    [Fact]
    public async void PostChore_ShouldAddEntityAndSavesRepo_WhenSuccessful()
    {
      var result = await controller.Post(mopViewModel);

      mockRepo.Verify(r => r.AddEntity(It.IsAny<Room>()), Times.Once);
      mockRepo.Verify(r => r.SaveAllAsync(), Times.Once);
    }

    [Fact]
    public async void PostChore_ShouldReturnBadRequest_WhenTryingToPostInvalidChore()
    {
      controller.ModelState.AddModelError("Name", "Name is required");
      mopViewModel.Name = "";

      var result = await controller.Post(mopViewModel);

      var badResult = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
      var modelState = badResult.Value.Should().BeAssignableTo<SerializableError>().Subject;
      modelState["Name"].Should().Equals("Name is required");
    }

    [Fact]
    public async void PostChore_ShouldNotAddChore_WhenTryingToPostInvalidChore()
    {
      controller.ModelState.AddModelError("Name", "Name is required");
      mopViewModel.Name = "";

      var result = await controller.Post(mopViewModel);

      mockRepo.Verify(r => r.AddEntity(It.IsAny<Chore>()), Times.Never);
      mockRepo.Verify(r => r.SaveAllAsync(), Times.Never);
    }

    [Fact]
    public async void PostChore_ShouldReturnBadRequest_WhenRepoFailsToAddChore()
    {
      mockRepo.Setup(r => r.SaveAllAsync())
              .Returns(Task.FromResult(false));

      var result = await controller.Post(mopViewModel);

      result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async void PostChore_ShouldReturnStatus500_WhenExceptionThrown()
    {
      mockRepo.Setup(r => r.AddEntity(It.IsAny<Chore>()))
              .Throws(new Exception());

      var result = await controller.Post(mopViewModel);

      var statusResult = result.Should().BeAssignableTo<StatusCodeResult>().Subject;
      statusResult.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void DeleteChore_ShouldReturnNoContent_WhenSuccessful()
    {
      mockRepo.Setup(r => r.GetChoreById(mopId))
              .Returns(mop);
      mockRepo.Setup(r => r.SaveAllAsync())
              .Returns(Task.FromResult(true));

      var result = await controller.Delete(mopId);

      result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async void DeleteChore_ShouldDeleteChore_WhenChoreFound()
    {
      mockRepo.Setup(r => r.GetChoreById(mopId))
              .Returns(mop);

      var result = await controller.Delete(mopId);

      mockRepo.Verify(r => r.Delete(mop), Times.Once);
      mockRepo.Verify(r => r.SaveAllAsync(), Times.Once);
    }

    [Fact]
    public async void DeleteChore_ShouldReturnNotFound_WhenChoreNotFound()
    {
      mockRepo.Setup(r => r.GetChoreById(mopId))
              .Returns(nullChore);

      var result = await controller.Delete(mopId);

      result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void DeleteChore_ShouldReturnBadResult_IfChoreFailsToSave()
    {
      mockRepo.Setup(r => r.GetChoreById(mopId))
              .Returns(mop);
      mockRepo.Setup(r => r.SaveAllAsync())
              .Returns(Task.FromResult(false));

      var result = await controller.Delete(mopId);

      result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async void DeleteChore_ShouldReturnStatusCode500_IfExceptionThrown()
    {
      mockRepo.Setup(r => r.GetChoreById(mopId)).Throws(new Exception());

      var result = await controller.Delete(mopId);

      var statusResult = result.Should().BeAssignableTo<StatusCodeResult>().Subject;
      statusResult.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void PutChore_ReturnsOk_WhenSuccessful()
    {
      mockMapper.Setup(m => m.Map<Chore>(It.IsAny<ChoreViewModel>()))
                .Returns(mop);
      mockMapper.Setup(m => m.Map<ChoreViewModel>(It.IsAny<Chore>()))
                .Returns(mopViewModel);
      mockRepo.Setup(r => r.GetChoreById(mopId))
              .Returns(mop);
      mockRepo.Setup(r => r.SaveAllAsync())
              .Returns(Task.FromResult(true));

      var result = await controller.Put(mopId, mopViewModel);

      var okResult = result.Should().BeAssignableTo<OkObjectResult>().Subject;
      var viewModel = okResult.Value.Should().BeAssignableTo<ChoreViewModel>().Subject;
      viewModel.Should().Be(mopViewModel);
    }

    [Fact]
    public async void PutChore_ShouldSaveRepo_WhenSuccessful()
    {
      mockRepo.Setup(r => r.GetChoreById(mopId))
              .Returns(mop);

      var result = await controller.Put(mopId, mopViewModel);

      mockRepo.Verify(r => r.SaveAllAsync(), Times.Once);
    }

    [Fact]
    public async void PutChore_ShouldReturnBadRequest_WhenRepoFailsToAddChore()
    {
      mockRepo.Setup(r => r.SaveAllAsync())
              .Returns(Task.FromResult(false));
      mockRepo.Setup(r => r.GetChoreById(mopId))
              .Returns(mop);

      var result = await controller.Put(mopId, new ChoreViewModel() { Name = "" });

      result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async void PutChore_ShoulReturnBadRequest_WhenTryingToPutInvalidChore()
    {
      controller.ModelState.AddModelError("Name", "Name is required");
      mopViewModel.Name = "";

      var result = await controller.Put(mopId, new ChoreViewModel() { Name = "" });

      var badResult = result.Should().BeAssignableTo<BadRequestObjectResult>().Subject;
      var modelState = badResult.Value.Should().BeAssignableTo<SerializableError>().Subject;
      modelState["Name"].Should().Equals("Name is required");
    }

    [Fact]
    public async void PutChore_ShouldReturnNotFound_WhenChoreNotFound()
    {
      mockRepo.Setup(r => r.GetChoreById(543))
              .Returns(nullChore);

      var result = await controller.Put(543, new ChoreViewModel() { Name = "" });

      result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void PutChore_ShouldReturnStatus500_WhenExceptionThrown()
    {
      mockRepo.Setup(r => r.GetChoreById(It.IsAny<int>()))
              .Throws(new Exception());

      var result = await controller.Put(mopId, mopViewModel);

      var statusResult = result.Should().BeAssignableTo<StatusCodeResult>().Subject;
      statusResult.StatusCode.Should().Be(500);
    }
  }
}