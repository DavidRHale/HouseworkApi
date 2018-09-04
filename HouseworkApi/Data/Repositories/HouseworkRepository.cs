using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseworkApi.Data
{
  public class HouseworkRepository : IHouseworkRepository
  {
    private readonly HouseworkApiContext _context;

    public HouseworkRepository(HouseworkApiContext context) 
    {
      _context = context;
    }

    // ------------------------------------------------ ROOMS ------------------------------------------------
    public IEnumerable<Room> GetAllRooms()
    {
      return _context.Rooms.ToList();
    }
    
    public Room GetRoomById(int id)
    {
      return _context
        .Rooms
        .Where(r => r.Id == id)
        .FirstOrDefault();
    }

    // ------------------------------------------------ CHORES ------------------------------------------------
    public IEnumerable<Chore> GetAllChores()
    {
      return _context.Chores.ToList();      
    }

    public Chore GetChoreById(int id)
    {
      return _context
        .Chores
        .Where(c => c.Id == id)
        .FirstOrDefault();
    }

    // ------------------------------------------------ GENERAL ------------------------------------------------
    public void AddEntity(object entity)
    {
      _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);
    }

    public async Task<bool> SaveAllAsync()
    {
      return (await _context.SaveChangesAsync()) > 0;
    }
  }
}