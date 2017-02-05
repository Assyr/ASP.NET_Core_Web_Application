using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.ViewModels;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;

        public AppController(IMailService mailService, IConfigurationRoot config)
        {
            _mailService = mailService;
            _config = config;
        }

        public IActionResult Index()
        {
            return View(); //Go find a view - render it and return it to the user
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            //WE DONT WANT TO SEND DATA THAT ISN'T VALID ANYWHERE - WE MUST PUT OUR CHECKS IN SO WE DONT SEND BAD DATA TO THE SERVER
            if (model.Email.Contains("aol.com"))
                ModelState.AddModelError("", "We don't support AOL addresses");

            if (ModelState.IsValid)//If all the data is valid... then you can call SendMail using out _mailService
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "Test", model.Message);

                //Clear the ModelState - This will clear all the models linked to this function
                ModelState.Clear();

                ViewBag.UserMessage = "Message Sent";
            } //else if it isn't valid - just continue displaying/returning the view and do nothing
                return View();            
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
