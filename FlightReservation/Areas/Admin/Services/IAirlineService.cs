namespace FlightReservation.Areas.Admin.Services
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
