using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
			try
			{
				var tripResults = _repository.GetAllTrips();

				throw new Exception();//return Ok(Mapper.Map<IEnumerable<TripViewModel>>(tripResults));
			}
			catch(Exception e)
			{
				//TODO Logging the exception


				return BadRequest("Error occured while retrieving trips " + e.Message);
			}
	    }

		//[FromBody] is to model bind a coming in with JSON data (body of it) to Trip obj.
	    [HttpPost("")]
	    public IActionResult Post([FromBody] TripViewModel trip)
	    {
			// In order to check for ModelState, we need to have validation attributes in VM class
		    if (ModelState.IsValid)
		    {
				//Map from source object VMTrip to the existing entity Trip.
				//TRICK: first the MAP should be created saying from what to what => see Startup/Configure()
			    Trip newTrip = Mapper.Map<Trip>(trip);




				//Should return a TripVM not to expose entity class Trip
			    return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(newTrip));
		    }

			return BadRequest(ModelState);
	    }
    }
}
