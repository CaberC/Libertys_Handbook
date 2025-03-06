using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Handbook.Models;

namespace Handbook.Controllers{
    class PersonController : Controller{
        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }
        public IActionResult Member(){
            ViewData["Title"] = "Member Page";
            Models.Date day = new Date();
            day.Today();
            ViewData["Day"] = day.ToString();
            return View("Memeber");
        }
    }
}