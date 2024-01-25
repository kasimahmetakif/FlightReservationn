using FlightReservation.Data;
using FlightReservation.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightReservation.Services
{
    public class AirportService : IAirportService
    {
        private readonly ApplicationDbContext _context;

        public AirportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Airport>> GetAllAirportsAsync()
        {
            return await _context.Airports.ToListAsync();
        }

        public async Task<Airport> GetAirportByIdAsync(int id)
        {
            return await _context.Airports.FirstOrDefaultAsync(a => a.AirportID == id);
        }

        public async Task<Airport> CreateAirportAsync(Airport airport)
        {
            _context.Airports.Add(airport);
            await _context.SaveChangesAsync();
            return airport;
        }

        public async Task<Airport> UpdateAirportAsync(Airport airport)
        {
            _context.Airports.Update(airport);
            await _context.SaveChangesAsync();
            return airport;
        }

        public async Task DeleteAirportAsync(int id)
        {
            var airport = await _context.Airports.FindAsync(id);
            if (airport != null)
            {
                _context.Airports.Remove(airport);
                await _context.SaveChangesAsync();
            }
        }
    }
}
