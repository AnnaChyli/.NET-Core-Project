using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models.Repositories
{
	public interface IWorldRepository
	{
		IEnumerable<Trip> GetAllTrips();
		void AddTrip(Trip trip);
		void AddStop(string tripName, Stop newStop);

		//Save changes to the repository all at once
		Task<bool> SaveChangesAsync();

		Trip GetTripByName(string tripName);
		
	}
}