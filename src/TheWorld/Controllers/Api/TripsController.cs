using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.Models.Repositories;
using TheWorld.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Api
{
	// API Controller
	[Route("api/trips")]
    public class TripsController : Controller
    {
	    private IWorldRepository _repository;
	    private ILogger<TripsController> _logger;

	    public TripsController(IWorldRepository repository, ILogger<TripsController> logger )
	    {
		    _repository = repository;
		    _logger = logger;
	    }
		
		[HttpGet("")]
	    public IActionResult Get()
		{
			//if (true) return BadRequest("Error occured!");
			try
			{
				var tripResults = _repository.GetAllTrips();

				return Ok(Mapper.Map<IEnumerable<TripViewModel>>(tripResults));
			}
			catch(Exception ex)
			{
				_logger.LogError($"Failed to get All Trips: {ex}");


				return BadRequest("Error occured while retrieving trips " + ex.Message);
			}
	    }

		//[FromBody] is to model bind a coming in with JSON data (body of it) to Trip obj.
	    [HttpPost("")]
	    public async Task<IActionResult> Post([FromBody] TripViewModel tripVm)
	    {
			// In order to check for ModelState, we need to have validation attributes in VM class
		    if (ModelState.IsValid)
		    {
				//Map from source object VMTrip to the existing entity Trip.
				//TRICK: first the MAP should be created saying from what to what => see Startup/Configure()
			    Trip newTrip = Mapper.Map<Trip>(tripVm);

			    _repository.AddTrip(newTrip);

			    if (await _repository.SaveChangesAsync())
			    {
				    //Should return a TripVM not to expose entity class Trip
				    return Created($"api/trips/{tripVm.Name}", Mapper.Map<TripViewModel>(newTrip));
			    }
			 }

			return BadRequest("Failed to save Trip to the DB");
	    }
    }
}
