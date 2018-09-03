using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace HouseworkApi.ViewModels
{
  public class RoomViewModel
  {
    public int RoomId { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<ChoreViewModel> Chores { get; set; }
    = new List<ChoreViewModel>();
  }
}