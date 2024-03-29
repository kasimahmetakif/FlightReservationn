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
    public class AirportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AirportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Airports != null ? 
                          View(await _context.Airports.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Airports'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Airports == null)
            {
                return NotFound();
            }

            var airport = await _context.Airports
                .FirstOrDefaultAsync(m => m.AirportID == id);
            if (airport == null)
            {
                return NotFound();
            }

            return View(airport);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AirportID,AirportName,City,Country,IsStatus")] Airport airport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airport);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Airports == null)
            {
                return NotFound();
            }

            var airport = await _context.Airports.FindAsync(id);
            if (airport == null)
            {
                return NotFound();
            }
            return View(airport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AirportID,AirportName,City,Country,IsStatus")] Airport airport)
        {
            if (id != airport.AirportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirportExists(airport.AirportID))
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
            return View(airport);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Airports == null)
            {
                return NotFound();
            }

            var airport = await _context.Airports
                .FirstOrDefaultAsync(m => m.AirportID == id);
            if (airport == null)
            {
                return NotFound();
            }

            return View(airport);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Airports == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Airports'  is null.");
            }
            var airport = await _context.Airports.FindAsync(id);
            if (airport != null)
            {
                _context.Airports.Remove(airport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirportExists(int id)
        {
          return (_context.Airports?.Any(e => e.AirportID == id)).GetValueOrDefault();
        }
    }
}
