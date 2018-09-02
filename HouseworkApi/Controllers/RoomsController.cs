using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HouseworkApi.Data;
using HouseworkApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HouseworkApi.Controllers
{
  [Route("api/rooms")]
  public class RoomsController : Controller
  {
    private IHouseworkRepository _repo;
    private IMapper _mapper;

    public RoomsController(IHouseworkRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    // GET api/rooms
    [HttpGet]
    public IActionResult Get()
    {
      try
      {
        var rooms = _repo.GetAllRooms().ToList();

        if (!rooms.Any())
        {
          return NotFound();
        }

        var viewModels = _mapper.Map<List<RoomViewModel>>(rooms);
        return Ok(viewModels);
      }
      catch (Exception)
      {
        return StatusCode(500);
      }
    }

    // GET api/rooms/5
    [HttpGet("{id}", Name = "GetRoom")]
    public IActionResult Get(int id)
    {
      try
      {
        var room = _repo.GetRoomById(id);

        if (room == null)
        {
          return NotFound();
        }

        var roomViewModel = _mapper.Map<RoomViewModel>(room);
        return Ok(roomViewModel);
      }
      catch (Exception)
      {
        return StatusCode(500);
      }
    }

    // POST api/rooms
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RoomViewModel viewModel)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var room = _mapper.Map<Room>(viewModel);

        _repo.AddEntity(room);

        if (await _repo.SaveAllAsync())
        {
          return CreatedAtRoute(
            "GetRoom",
            new { id = viewModel.RoomId },
            _mapper.Map<RoomViewModel>(room));
        }
        else
        {
          return BadRequest();
        }
      }
      catch (Exception)
      {
        return StatusCode(500);
      }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        var room = _repo.GetRoomById(id);

        if (room == null)
        {
          return NotFound();
        }

        _repo.Delete(room);
        if (await _repo.SaveAllAsync())
        {
          return NoContent();
        }
        else
        {
          return BadRequest();
        }
      }
      catch (Exception)
      {
        return StatusCode(500);
      }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] RoomViewModel newViewModel)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var room = _repo.GetRoomById(id);

        if (room == null)
        {
          return NotFound();
        }

        _mapper.Map(newViewModel, room);

        if (await _repo.SaveAllAsync())
        {
          return Ok(_mapper.Map<RoomViewModel>(room));
        }
        else
        {
          return BadRequest();
        }
      }
      catch (Exception ex)
      {
        return StatusCode(500);
      }
    }
  }
}
