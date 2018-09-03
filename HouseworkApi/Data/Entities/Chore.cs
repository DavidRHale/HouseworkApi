using System;

namespace HouseworkApi.Data
{
  public class Chore
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime LastCompleted { get; set; }
    public TimeSpan Frequency { get; set; }
    
    public int RoomId { get; set; }
    public Room Room { get; set; }
  }
}