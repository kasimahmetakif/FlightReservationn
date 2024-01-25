using FlightReservation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightReservation.Services
{
    public interface IAirlineService
    {
        Task<IEnumerable<Airline>> GetAllAirlinesAsync();
        Task<Airline> GetAirlineByIdAsync(int id);
        Task CreateAirlineAsync(Airline airline);
        Task UpdateAirlineAsync(Airline airline);
        Task DeleteAirlineAsync(int id);
    }
}
