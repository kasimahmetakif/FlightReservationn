using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlightReservation.Models;
using Microsoft.AspNetCore.Identity;

namespace FlightReservation.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Airport>()
                .HasMany(a => a.DepartureFlights)
                .WithOne(f => f.DepartureAirport)
                .HasForeignKey(f => f.DepartureAirportID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Airport>()
                .HasMany(a => a.ArrivalFlights)
                .WithOne(f => f.ArrivalAirport)
                .HasForeignKey(f => f.ArrivalAirportID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Airline>()
                .HasMany(a => a.Flights)
                .WithOne(f => f.Airline)
                .HasForeignKey(f => f.AirlineID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Flight>()
                .HasMany(f => f.Reservations)
                .WithOne(r => r.Flight)
                .HasForeignKey(r => r.FlightID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Passenger>()
                .HasMany(p => p.Reservations)
                .WithOne(r => r.Passenger)
                .HasForeignKey(r => r.PassengerID)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
