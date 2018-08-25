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