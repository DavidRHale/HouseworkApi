using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseworkApi.Data
{
  public interface IHouseworkRepository
  {
  // General
    Task<bool> SaveAllAsync();
    void AddEntity(object entity);
    void Delete<T>(T entity) where T : class;

    // Rooms
    IEnumerable<Room> GetAllRooms();
  }
}