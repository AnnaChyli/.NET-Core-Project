using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.Models.Repositories;
using TheWorld.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Api
{
	[Route("api/trips/{tripname}/stops")]
	public class StopsController : Controller
	{
		private IWorldRepository _repository;
		private ILogger<StopsController> _logger;

		public StopsController(IWorldRepository repository, ILogger<StopsController> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		[HttpGet("")] // set a route to handle association Stops to Trips
		public IActionResult Get(string tripName)
		{
			try
			{
				Trip trip = _repository.GetTripByName(tripName);

				// Map Stop to StopViewModel
				return Ok(Mapper.Map<IEnumerable <StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get Stops {ex}");
				return BadRequest("Failed to get Stops");
			}
		}

		// Add a stop
		[HttpPost("")]
		public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel vm)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Stop newStop = Mapper.Map<Stop>(vm);

					_repository.AddStop(tripName, newStop);

					if (await _repository.SaveChangesAsync())
					{
						return Created($"/api/trips/{tripName}/stops/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
					}
				}
				
			}
			catch (Exception ex)
			{
				_logger.LogError($"Fail to add a new Stop {ex}");
			}

			return BadRequest("Fail to add a new Stop");
		}
	}

}
		//// GET: api/values
		//[HttpGet]
		//public IEnumerable<string> Get()
		//{
		//    return new string[] { "value1", "value2" };
		//}

//// GET api/values/5
//[HttpGet("{id}")]
//public string Get(int id)
//{
//    return "value";
//}

//// POST api/values
//[HttpPost]
//public void Post([FromBody]string value)
//{
//}

//// PUT api/values/5
//[HttpPut("{id}")]
//public void Put(int id, [FromBody]string value)
//{
//}

//// DELETE api/values/5
//[HttpDelete("{id}")]
//public void Delete(int id)
//{
//}

