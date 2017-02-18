using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<WorldUser> _signInManager;

        public AuthController(SignInManager<WorldUser> signInManager)
        {
            _signInManager = signInManager;
        }

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

        //This is handling the 'Post' for our login that we specified in the Login.cshtml
        [HttpPost]                                              //returnUrl resembles the return url we get in our http url when we are redirected to the login page from the trips page - this is used to redirect us back to the correct page.
        public async Task<ActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);

                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Trips", "App");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or password incorrect");
                }

            }

            //This returns the view depending on what occurs above - if login fails we return a model state error
            return View();//If model state isn't valid, return the current View() to try again
        }

        public async Task<ActionResult> Logout()
        {
            //Only attempt to logout if someone is authenticated and logged in
            if(User.Identity.IsAuthenticated)
            {
                //Signs the current user out of the application
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }
    }
}
