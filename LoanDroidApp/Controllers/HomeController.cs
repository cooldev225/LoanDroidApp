using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace App.Controllers
{
    //[Authorize(Roles = "client,investor")]
    //[Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            ViewBag.Layout = "_Layout1";
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Wantloan()
        {
            return View();
        }
        public IActionResult Wantlend()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
