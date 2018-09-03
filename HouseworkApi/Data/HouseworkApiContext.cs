using Microsoft.EntityFrameworkCore;

namespace HouseworkApi.Data
{
  public class HouseworkApiContext : DbContext
  {
    public HouseworkApiContext(DbContextOptions<HouseworkApiContext> options) : base(options)
        {
        }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Chore> Chores { get; set; }
  }
}