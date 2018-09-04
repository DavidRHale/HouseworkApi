using System;
using FluentAssertions;
using Xunit;
using HouseworkApi.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HouseworkApiTests
{
  public class HouseworkRepositoryTests
  {
    int choreId;
    int roomId;

    [Fact]
    public async void GetAllRooms()
    {
      var repo = await GetInMemoryHouseworkRepository();

      var rooms = repo.GetAllRooms().ToList();
      rooms.Count.Should().Be(3);
    }

    [Fact]
    public async void GetRoomsById()
    {
      var repo = await GetInMemoryHouseworkRepository();

      var id = 88;
      var newRoom = new Room() { Name = "Bathroom", Id = id };

      repo.AddEntity(newRoom);
      var saved = await repo.SaveAllAsync();
      saved.Should().Be(true);

      var room = repo.GetRoomById(id);
      room.Name.Should().Be("Bathroom");
    }

    [Fact]
    public async void GetAllChores()
    {
      var repo = await GetInMemoryHouseworkRepository();

      var chores = repo.GetAllChores().ToList();
      chores.Count.Should().Be(1);
    }

    [Fact]
    public async void GetChoreById()
    {
      var repo = await GetInMemoryHouseworkRepository();

      var chore = repo.GetChoreById(choreId);
      chore.Name.Should().Be("Mop floor");
      chore.LastCompleted.Should().Be(DateTime.MinValue);
      chore.Frequency.Should().Be(TimeSpan.FromDays(7));
      chore.RoomId.Should().Be(roomId);
    }


    private async Task<IHouseworkRepository> GetInMemoryHouseworkRepository()
    {
        DbContextOptions<HouseworkApiContext> options;
        var builder = new DbContextOptionsBuilder<HouseworkApiContext>().UseInMemoryDatabase("TestingDb");
        options = builder.Options;
        HouseworkApiContext HouseworkApiContext = new HouseworkApiContext(options);
        HouseworkApiContext.Database.EnsureDeleted();
        HouseworkApiContext.Database.EnsureCreated();
        var repo = new HouseworkRepository(HouseworkApiContext);

        var kitchen = new Room() { Name = "Kitchen" };
        repo.AddEntity(kitchen);

        var mop = new Chore() {
          Name = "Mop floor",
          LastCompleted = DateTime.MinValue,
          Frequency = TimeSpan.FromDays(7),
          RoomId = kitchen.Id
        };
        repo.AddEntity(mop);

        repo.AddEntity(new Room() { Name = "Living Room" });
        repo.AddEntity(new Room() { Name = "Bathroom" });

        await repo.SaveAllAsync();

        roomId = kitchen.Id;
        choreId = mop.Id;

        return repo;
    }
  }
}