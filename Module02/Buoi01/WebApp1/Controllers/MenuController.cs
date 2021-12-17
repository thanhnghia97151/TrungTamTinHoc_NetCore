using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class MenuController : BaseController
    {
        public MenuController(SiteProvider provider) : base(provider) { }
        public IActionResult Index()
        {
            // return View(provider.Category.GetCategories());
            return View(provider.Category.GetPrents());
        }
    }
}
