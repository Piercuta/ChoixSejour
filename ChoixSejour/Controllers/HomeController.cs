using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoixSejour.Models;
using ChoixSejour.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChoixSejour.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IDal dal;
        public HomeController()
        {
            dal = new Dal();
        }
        public IActionResult Index()
        {
            return View();
        }        
    }
}