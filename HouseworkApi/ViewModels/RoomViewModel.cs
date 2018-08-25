using System;
using System.ComponentModel.DataAnnotations;

namespace HouseworkApi.ViewModels
{
  public class RoomViewModel
  {
    public int RoomId { get; set; }
    [Required]
    public string Name { get; set; }
  }
}