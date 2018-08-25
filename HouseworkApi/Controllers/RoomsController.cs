using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseworkApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HouseworkApi.Controllers
{
    [Route("api/rooms")]
    public class RoomsController : Controller
    {
        // GET api/rooms
        [HttpGet]
        public IActionResult Get()
        {
            var ret = new RoomViewModel();
            return Ok(new List<RoomViewModel>(){ret});
        }

        // // GET api/values/5
        // [HttpGet("{id}")]
        // public string Get(int id)
        // {
        //     return "value";
        // }

        // // POST api/values
        // [HttpPost]
        // public void Post([FromBody]string value)
        // {
        // }

        // // PUT api/values/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody]string value)
        // {
        // }

        // // DELETE api/values/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
