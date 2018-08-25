using System;
using HouseworkApi.Data;
using FluentAssertions;
using Xunit;

namespace HouseworkApiTests
{
  public class RoomsTests
  {
    [Fact]
    public void GetAllRooms()
    {
      var room = new Room();
      room.Id = 1;
      room.Name = "Kitchen";

      room.Id.Should().Equals(1);
      room.Name.Should().Equals("Kitchen");      
    }
  }
}
