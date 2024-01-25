using FlightReservation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightReservation.Services
{
    public interface IAirportService
    {
        Task<IEnumerable<Airport>> GetAllAirportsAsync();
        Task<Airport> GetAirportByIdAsync(int id);
        Task<Airport> CreateAirportAsync(Airport airport);
        Task<Airport> UpdateAirportAsync(Airport airport);
        Task DeleteAirportAsync(int id);
    }
}
