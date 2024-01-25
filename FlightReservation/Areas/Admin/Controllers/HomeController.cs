// Admin area's Home Controller
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

// Passenger area's Home Controller
namespace FlightReservation.Areas.Passenger.Controllers
{
    [Area("Passenger")]
    [Authorize(Roles = "Passenger")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
