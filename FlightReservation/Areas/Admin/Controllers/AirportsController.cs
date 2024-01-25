using FlightReservation.Areas.Admin.Services;
using FlightReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlightReservation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] 
    public class AirportsController : Controller
    {
        private readonly IAirportService _airportService;

        public AirportsController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        // GET: Admin/Airports
        public async Task<IActionResult> Index()
        {
            return View(await _airportService.GetAllAirportsAsync());
        }

        // GET: Admin/Airports/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var airport = await _airportService.GetAirportByIdAsync(id);
            if (airport == null)
            {
                return NotFound();
            }
            return View(airport);
        }

        // GET: Admin/Airports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Airports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AirportID,AirportName,City,Country,IsStatus")] Airport airport)
        {
            if (ModelState.IsValid)
            {
                await _airportService.CreateAirportAsync(airport);
                return RedirectToAction(nameof(Index));
            }
            return View(airport);
        }

        // GET: Admin/Airports/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var airport = await _airportService.GetAirportByIdAsync(id);
            if (airport == null)
            {
                return NotFound();
            }
            return View(airport);
        }

        // POST: Admin/Airports/Edit/5
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
                var result = await _airportService.UpdateAirportAsync(airport);
                if (result == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(airport);
        }

        // GET: Admin/Airports/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var airport = await _airportService.GetAirportByIdAsync(id);
            if (airport == null)
            {
                return NotFound();
            }
            return View(airport);
        }

        // POST: Admin/Airports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _airportService.DeleteAirportAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
