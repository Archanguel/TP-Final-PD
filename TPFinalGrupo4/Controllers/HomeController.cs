using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TPFinalGrupo4.Models;
using System.Media;

namespace TPFinalGrupo4.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ILogger<HomeController> _logger;
        private SoundPlayer _soundPlayer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet("Home")]
        public IActionResult Index()
        {
            //ViewData["Message"] = _localizer["Entrar"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /*[HttpGet("Home")]
        public IActionResult VistaCliente()
        {
            return View();
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
            _soundPlayer.Play();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            //return Redirect("/Home");
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
