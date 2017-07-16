using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Api
{
	[Route("api/trips")]
    public class TripsController : Controller
    {
	    private IWorldRepository _repository;

	    public TripsController(IWorldRepository repository)
	    {
		    _repository = repository;
	    }


		[HttpGet("")]
	    public IActionResult Get()
		{
			

			//if (true) return BadRequest("Error occured!");

		    return Ok(_repository.GetAllTrips());
	    }

		//[FromBody] is to model bind a coming in with JSON data (body of it) to Trip obj.
	    [HttpPost("")]
	    public IActionResult Post([FromBody] TripViewModel trip)
	    {
			// In order to check for ModelState, we need to have validation attributes in VM class
		    if (ModelState.IsValid)
		    {
			    return Created($"api/trips/{trip.Name}", trip);
		    }

			return BadRequest(ModelState);
	    }
    }
}
