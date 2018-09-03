using System;

namespace HouseworkApi.Data.Entities
{
  public class Task
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime LastCompleted { get; set; }
    public TimeSpan Frequency { get; set; }
  }
}