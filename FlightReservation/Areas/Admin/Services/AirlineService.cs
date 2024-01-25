using FlightReservation.Data;
using FlightReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightReservation.Areas.Admin.Services
{
    public class AirlineService : IAirlineService
    {
        private readonly ApplicationDbContext _context;

        public AirlineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Airline>> GetAllAirlinesAsync()
        {
            return await _context.Airlines.ToListAsync();
        }

        public async Task<Airline> GetAirlineByIdAsync(int id)
        {
            return await _context.Airlines.FirstOrDefaultAsync(a => a.AirlineID == id);
        }

        public async Task CreateAirlineAsync(Airline airline)
        {
            _context.Airlines.Add(airline);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAirlineAsync(Airline airline)
        {
            _context.Airlines.Update(airline);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAirlineAsync(int id)
        {
            var airline = await _context.Airlines.FindAsync(id);
            if (airline != null)
            {
                _context.Airlines.Remove(airline);
                await _context.SaveChangesAsync();
            }
        }
    }
}