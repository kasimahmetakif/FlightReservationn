using FlightReservation.Data;
using FlightReservation.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
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

        public async Task<IActionResult> Index()
        {
            var airportsSelectList = new SelectList(await _context.Airports.ToListAsync(), "AirportID", "AirportName");
            ViewBag.DepartureAirports = airportsSelectList;
            ViewBag.ArrivalAirports = airportsSelectList;

            var airportInfos = await _context.Airports
            .Select(a => new
            {
                AirportID = a.AirportID, 
                AirportName = a.AirportName,
                Image = a.Image, 
                LowestPrice = _context.Flights
                    .Where(f => f.DepartureAirportID == a.AirportID)
                    .OrderBy(f => f.Price)
                    .Select(f => f.Price)
                    .FirstOrDefault() 
            })
            .ToListAsync();

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

        [HttpGet]
        public async Task<IActionResult> CountryFlights(int airportID)
        {
            var flights = await _context.Flights
                .Include(f => f.Airline)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Where(f => f.DepartureAirportID == airportID) 
                .ToListAsync();

            return View(flights);
        }

        public async Task<IActionResult> CommonPurchase(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var flight = await _context.Flights
                .Include(f => f.Airline)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.FlightID == id);

            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        public IActionResult Order()
        {
            Options options = new Options(); // Iyzico Import
            options.ApiKey = "sandbox-ZDtW3lzihIjLVRBEvyccHn1n8FxQwvwR";
            options.SecretKey = "sandbox-6HuobXnHRvJA2Zkzzn8WkZ030wREchvI";
            options.BaseUrl = "Https://sandbox-api.iyzipay.com";

            //double savePrice = 0;
            //double delivaryShippingPrice = 38;
            //foreach (var item in GetCart().CartLines)
            //{
            //    savePrice += ((item.Quantity * item.Advert.Price) / 100) * campaignRepository.Detail(item.Advert.CampaignId).Rate;
            //}
            //double Price = GetCart().TotalPrice() - savePrice + delivaryShippingPrice;
            //string TotalPrice = Math.Round(Price, 2).ToString().Replace(',', '.');

            //var user = userRepository.UserAccount(User.Identity.Name);
            //var userShippingAddress = addressRepository.List().FirstOrDefault(x => x.UserId == user.Id && x.IsSelected == true);
            //var userProvince = provinceRepository.Detail(userShippingAddress.ProvinceId);


            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = "1";
            request.PaidPrice = "1";
            request.Currency = Currency.TRY.ToString();
            request.BasketId = "B67832";
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "https://localhost:7178/Customer/Cart/Success";

            //List<int> enabledInstallments = new List<int>();
            //enabledInstallments.Add(2);
            //enabledInstallments.Add(3);
            //enabledInstallments.Add(6);
            //enabledInstallments.Add(9);
            //request.EnabledInstallments = enabledInstallments;

            Buyer buyer = new Buyer();
            buyer.Id = "asdadsada";
            buyer.Name = "Erhan";
            buyer.Surname = "Kaya";
            buyer.GsmNumber = "+905554443322";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2000-12-12 12:00:00";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Erhan Kaya";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Bereket döner karşısı";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Erhan Kaya";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Bereket Döner";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketProduct;
            basketProduct = new BasketItem();
            basketProduct.Id = "1";
            basketProduct.Name = "Asus Bilgisayar";
            basketProduct.Category1 = "Bilgisayar";
            basketProduct.Category2 = "";
            basketProduct.ItemType = BasketItemType.PHYSICAL.ToString();

            double price = 1;
            double endPrice = 1;
            basketProduct.Price = endPrice.ToString().Replace(",", "");
            basketItems.Add(basketProduct);

            request.BasketItems = basketItems;

            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
            ViewBag.pay = checkoutFormInitialize.CheckoutFormContent;
            return View();
        }

    }
}
