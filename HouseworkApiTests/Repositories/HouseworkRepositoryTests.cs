using System;
using FluentAssertions;
using Xunit;
using HouseworkApi.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HouseworkApiTests
{
  public class HouseworkRepositoryTests
  {
    [Fact]
    public void GetAllRooms()
    {
      var repo = GetInMemoryHouseworkRepository();

      var rooms = repo.GetAllRooms().ToList();
      rooms.Count.Should().Be(3);
    }

    [Fact]
    public async void GetRoomsById()
    {
      var repo = GetInMemoryHouseworkRepository();

      var id = 88;
      var newRoom = new Room() { Name = "Bathroom", Id = id };

      repo.AddEntity(newRoom);
      var saved = await repo.SaveAllAsync();
      saved.Should().Be(true);

      var room = repo.GetRoomById(id);
      room.Name.Should().Be("Bathroom");
    }

    private IHouseworkRepository GetInMemoryHouseworkRepository()
    {
        DbContextOptions<HouseworkApiContext> options;
        var builder = new DbContextOptionsBuilder<HouseworkApiContext>().UseInMemoryDatabase("TestingDb");
        options = builder.Options;
        HouseworkApiContext HouseworkApiContext = new HouseworkApiContext(options);
        HouseworkApiContext.Database.EnsureDeleted();
        HouseworkApiContext.Database.EnsureCreated();
        var repo = new HouseworkRepository(HouseworkApiContext);

        repo.AddEntity(new Room() { Name = "Kitchen" });
        repo.AddEntity(new Room() { Name = "Living Room" });
        repo.AddEntity(new Room() { Name = "Bathroom" });

        repo.SaveAllAsync();

        return repo;
    }
  }
}