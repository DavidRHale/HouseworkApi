using System;
using System.ComponentModel.DataAnnotations;

namespace HouseworkApi.ViewModels
{
  public class ChoreViewModel
  {
    public int ChoreId { get; set; }   
    [Required]
    public string Name { get; set; }
    public DateTime LastCompleted { get; set; }
    = DateTime.MinValue;
    [Required]
    public TimeSpan Frequency { get; set; }

    [Required]
    public int RoomId { get; set; }
    public RoomViewModel Room { get; set; }
  }
}