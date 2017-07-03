using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
	    private IMailService _mailService;
	    private IConfigurationRoot _config;
		private WorldContext _context;

		// Implementing a dependency injection for the Contact() method to
		// be able to get the email message
		public AppController(IMailService mailService, IConfigurationRoot config, WorldContext context)
	    {
		    _mailService = mailService;
		    _config = config;
		    _context = context;
	    }


        // GET: /<controller>/
        public IActionResult Index()
        {
			// Converts this line into the query, and retrieves list of trips
	        List<Trip> data = _context.Trips.ToList();

            return View(data);
        }


	    public IActionResult Contact()
	    {
			return View();
	    }

		[HttpPost]
		public IActionResult Contact(ContactViewModel model)
		{
			if (model.Email.Contains("oal.com"))
			{
				//Testing against Validation Attributes 
				//Property error
				ModelState.AddModelError("Email", "We don't support AOL addresses");
			}
			if (model.Email.Contains("hell.com"))
			{
				// Displays Model error in Summary => ""
				ModelState.AddModelError("", "We don't support HELL addresses");
			}

			if (ModelState.IsValid)
			{
				_mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "Important Subject", model.Message);

				//Clears the typed content of the page
				ModelState.Clear();

				ViewBag.UserMessage = "Message Sent";
			}

			return View();
		}


		public IActionResult About()
	    {
			return View();
		}
    }
}
