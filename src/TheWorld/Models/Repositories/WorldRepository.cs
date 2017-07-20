using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TheWorld.Models.Repositories
{
	public class WorldRepository : IWorldRepository
	{
		private WorldContext _context;

		// Specify the type of the logger by passing WorldRepository. Log system imits it into the log messages.
		private ILogger<WorldRepository> _logger;

		public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
	    {
		    _context = context;
		    _logger = logger;
	    }

	    public IEnumerable<Trip> GetAllTrips()
	    {
			_logger.LogInformation("Getting All Trips from DB");
		    return _context.Trips.ToList();
	    }

		// Pushing a new obj into a context, not saving into DB
		public void AddTrip(Trip trip)
		{
			_context.Add(trip);
		}

		// Returns whether or not the changes were saved
		public async Task <bool> SaveChangesAsync()
		{
			int result = await _context.SaveChangesAsync();

			return result > 0; 
		}

		public Trip GetTripByName(string name)
		{
			//include the collection of stops when the trip returned
			var trip = _context.Trips 
				.Include(t => t.Stops)
				.FirstOrDefault(x => x.Name.Equals(name));

			return trip;
		}

		public void AddStop(string tripName, Stop newStop)
		{
			Trip trip = GetTripByName(tripName);
			if (trip != null)
			{
				//Foreign key is set
				trip.Stops.Add(newStop);

				// The record is added
				_context.Stops.Add(newStop);
			}

			
		}
	}
}
