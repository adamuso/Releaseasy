using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Releaseasy.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IHostingEnvironment hostingEnvironment)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}