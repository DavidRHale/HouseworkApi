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
  [Route("api/chores")]
  public class ChoresController : Controller
  {
    private IHouseworkRepository _repo;
    private IMapper _mapper;

    public ChoresController(IHouseworkRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get()
    {
      try
      {
        var chores = _repo.GetAllChores().ToList();

        if (!chores.Any())
        {
          return NotFound();
        }
        return Ok(_mapper.Map<List<ChoreViewModel>>(chores));
      }
      catch (Exception) 
      {
        return StatusCode(500);
      }
    }

    [HttpGet("{id}", Name = "GetChore")]
    public IActionResult Get(int id) 
    {
      try
      {
        var chore = _repo.GetChoreById(id);

        if (chore == null)
        {
          return NotFound();
        }
        return Ok(_mapper.Map<ChoreViewModel>(chore));  
      }
      catch (System.Exception)
      {
        return StatusCode(500);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChoreViewModel viewModel)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        var chore = _mapper.Map<Chore>(viewModel);
        _repo.AddEntity(chore);

        if (await _repo.SaveAllAsync())
        {
          return CreatedAtRoute(
            "GetChore",
            new { id = viewModel.ChoreId },
            viewModel
          );
        }
        else
        {
          return BadRequest();
        }
      }
      catch (System.Exception)
      {
        return StatusCode(500);
      }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        var chore = _repo.GetChoreById(id);

        if (chore == null)
        {
          return NotFound();
        }

        _repo.Delete(chore);
        
        if (await _repo.SaveAllAsync()) 
        {
          return NoContent();
        }
        else
        {
          return BadRequest();
        }
      }
      catch (System.Exception)
      {
        return StatusCode(500);
      }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ChoreViewModel viewModel)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var chore = _repo.GetChoreById(id);

        if (chore == null)
        {
          return NotFound();
        }

        _mapper.Map(viewModel, chore);

        if (await _repo.SaveAllAsync())
        {
          return Ok(_mapper.Map<ChoreViewModel>(chore));
        }
        else
        {
          return BadRequest();
        }
      }
      catch (System.Exception)
      {
        return StatusCode(500);
      }
    }
  }
}