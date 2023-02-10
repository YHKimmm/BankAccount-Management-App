using ASP.Net_MVC_Assignment.Data;
using ASP.Net_MVC_Assignment.Models;
using ASP.Net_MVC_Assignment.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.Net_MVC_Assignment.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }

        public IActionResult Index(Client client)
        {
            var adminUserName = _configuration["adminLogin:Username"];
            var adminPassword = _configuration["AdminLogin:Password"];
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var sendgrid = _configuration["SendGrid:ApiKey"];


            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

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
    }
}