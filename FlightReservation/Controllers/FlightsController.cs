﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightReservation.Data;

namespace FlightReservation.Controllers
{
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Flights.Include(f => f.Airline).Include(f => f.ArrivalAirport).Include(f => f.DepartureAirport);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Airline)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .FirstOrDefaultAsync(m => m.FlightID == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        public IActionResult Create()
        {
            ViewData["AirlineID"] = new SelectList(_context.Airlines, "AirlineID", "AirlineName");
            ViewData["ArrivalAirportID"] = new SelectList(_context.Airports, "AirportID", "AirportName");
            ViewData["DepartureAirportID"] = new SelectList(_context.Airports, "AirportID", "AirportName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightID,AirlineID,DepartureAirportID,ArrivalAirportID,DepartureDateTime,ArrivalDateTime,SeatCapacity,Price,IsStatus")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AirlineID"] = new SelectList(_context.Airlines, "AirlineID", "AirlineName", flight.AirlineID);
            ViewData["ArrivalAirportID"] = new SelectList(_context.Airports, "AirportID", "AirportName", flight.ArrivalAirportID);
            ViewData["DepartureAirportID"] = new SelectList(_context.Airports, "AirportID", "AirportName", flight.DepartureAirportID);
            return View(flight);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            ViewData["AirlineID"] = new SelectList(_context.Airlines, "AirlineID", "AirlineName", flight.AirlineID);
            ViewData["ArrivalAirportID"] = new SelectList(_context.Airports, "AirportID", "AirportName", flight.ArrivalAirportID);
            ViewData["DepartureAirportID"] = new SelectList(_context.Airports, "AirportID", "AirportName", flight.DepartureAirportID);
            return View(flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightID,AirlineID,DepartureAirportID,ArrivalAirportID,DepartureDateTime,ArrivalDateTime,SeatCapacity,Price,IsStatus")] Flight flight)
        {
            if (id != flight.FlightID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.FlightID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AirlineID"] = new SelectList(_context.Airlines, "AirlineID", "AirlineName", flight.AirlineID);
            ViewData["ArrivalAirportID"] = new SelectList(_context.Airports, "AirportID", "AirportName", flight.ArrivalAirportID);
            ViewData["DepartureAirportID"] = new SelectList(_context.Airports, "AirportID", "AirportName", flight.DepartureAirportID);
            return View(flight);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.Airline)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .FirstOrDefaultAsync(m => m.FlightID == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Flights == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Flights'  is null.");
            }
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
          return (_context.Flights?.Any(e => e.FlightID == id)).GetValueOrDefault();
        }
    }
}
