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
            return View();
        }
    }
}