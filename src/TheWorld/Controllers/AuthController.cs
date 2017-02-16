using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Controllers
{
    public class AuthController : Controller
    {

        public IActionResult Login()
        {
            //If the user is already authenticated, no need to log them in again
            if(User.Identity.IsAuthenticated)
            {
                //Redirect them to the 'Trips' action in our 'App' controller
                return RedirectToAction("Trips", "App");
            }

            return View();
        }

    }
}
