using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightReservation.Data;

namespace FlightReservation.Controllers
{
    public class AirlinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AirlinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Airlines != null ? 
                          View(await _context.Airlines.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Airlines'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Airlines == null)
            {
                return NotFound();
            }

            var airline = await _context.Airlines
                .FirstOrDefaultAsync(m => m.AirlineID == id);
            if (airline == null)
            {
                return NotFound();
            }

            return View(airline);
        }

        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AirlineID,AirlineName,IsStatus")] Airline airline)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airline);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Airlines == null)
            {
                return NotFound();
            }

            var airline = await _context.Airlines.FindAsync(id);
            if (airline == null)
            {
                return NotFound();
            }
            return View(airline);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AirlineID,AirlineName,IsStatus")] Airline airline)
        {
            if (id != airline.AirlineID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirlineExists(airline.AirlineID))
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
            return View(airline);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Airlines == null)
            {
                return NotFound();
            }

            var airline = await _context.Airlines
                .FirstOrDefaultAsync(m => m.AirlineID == id);
            if (airline == null)
            {
                return NotFound();
            }

            return View(airline);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Airlines == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Airlines'  is null.");
            }
            var airline = await _context.Airlines.FindAsync(id);
            if (airline != null)
            {
                _context.Airlines.Remove(airline);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirlineExists(int id)
        {
          return (_context.Airlines?.Any(e => e.AirlineID == id)).GetValueOrDefault();
        }
    }
}
