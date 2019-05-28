using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EICProjectBackend.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return new OkObjectResult("API Is Up And Running!!");

        }
 
    }
}
