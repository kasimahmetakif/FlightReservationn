using FlightReservation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightReservation.Areas.Admin.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<Flight>> GetAllFlightsAsync();
        Task<Flight> GetFlightByIdAsync(int id);
        Task<Flight> CreateFlightAsync(Flight flight);
        Task<Flight> UpdateFlightAsync(Flight flight);
        Task DeleteFlightAsync(int id);
    }
}
