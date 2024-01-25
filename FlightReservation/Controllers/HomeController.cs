using FlightReservation.Data;
using FlightReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FlightReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Bu action, Index view'ında iki dropdown listesi için havalimanları verilerini çeker.
        public async Task<IActionResult> Index()
        {
            var airportsSelectList = new SelectList(await _context.Airports.ToListAsync(), "AirportID", "AirportName");
            ViewBag.DepartureAirports = airportsSelectList;
            ViewBag.ArrivalAirports = airportsSelectList;

            // Havalimanları ve en düşük fiyatları çekme
            var airportInfos = await _context.Airports
                .Select(a => new
                {
                    a.AirportName,
                    a.Image, // Varsayalım ki havalimanı için resim url'si bu alanda tutuluyor
                    LowestPrice = _context.Flights
                        .Where(f => f.DepartureAirport.AirportName == a.AirportName)
                        .OrderBy(f => f.Price)
                        .Select(f => f.Price)
                        .FirstOrDefault() // En düşük fiyat
                })
                .ToListAsync();

            // View'da kullanmak üzere ViewBag'a ekleme
            ViewBag.AirportInfos = airportInfos;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Bu action, Flights view'ı için gerekli uçuş verilerini çeker.
        public async Task<IActionResult> Flights()
        {
            var flights = await _context.Flights
                .Include(f => f.Airline)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .ToListAsync();

            return View(flights);
        }

        [HttpGet]
        public async Task<IActionResult> SearchFlights(int? departureAirportID, int? arrivalAirportID)
        {
            if (!departureAirportID.HasValue || !arrivalAirportID.HasValue)
            {
                return RedirectToAction("Index");
            }

            var flights = await _context.Flights
                .Include(f => f.Airline)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Where(f => f.DepartureAirportID == departureAirportID.Value && f.ArrivalAirportID == arrivalAirportID.Value)
                .ToListAsync();

            return View("SearchResults", flights);
        }

    }
}
