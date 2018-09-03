using System;
using System.Collections.Generic;
using HouseworkApi.Data;

namespace HouseworkApi.Data
{
  public class Room
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Chore> Chores { get; set; }
  }
}