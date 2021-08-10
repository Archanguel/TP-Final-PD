using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TPFinalGrupo4.Models;
using TPFinalGrupo4.Data;
using TPFinalGrupo4.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Media;

namespace TPFinalGrupo4.Controllers
{
    public class LoginController : Controller
    {
        private readonly AgenciaManager agencia;
        private SoundPlayer _soundPlayer;

        public LoginController(AgenciaManager agencia)
        {
            this.agencia = agencia;
        }

        [HttpGet("Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync(int dni, String password)
        {
            int intentos = 0;

            if (this.agencia.FindUserForDNI(dni) == null) //!_context.Usuario.Any(x => x.Dni == dni)
            {
                //MessageBox.Show("No existe ese usuario");
                //return;
                ViewBag.Error = "Usuario o contraseña invalida";
                _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                _soundPlayer.Play();
                return RedirectToAction("Index");
            }

            //this.agencia.BloquearUsuario(dni);
            // Al bloquear al usuario salgo del metodo con el return
            //if (this.agencia.bloquearUsuarioPorIntentos(dni)) return;


            if (this.agencia.autenticarUsuario(dni, password))
            {
                /*
                //Create Cookies
                HttpCookie UserCookie = new HttpCookie("user", this.agencia.GetUsuarioLogeado().Nombre);
                Response.Cookies["user"].Value = dni;

                //Expire Date
                //userCookie.Expires.AddDays(10);
                Response.Cookies["user"].Expires = DateTime.Now.AddHours(2);

                //Save data at Cookies
                HttpContext.Response.SetCookie(UserCookie);

                //Get user data from Cookie
                HttpCookie NewCookie = Request.Cookies["user"];

                //return NewCookie.Value;
                */
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, this.agencia.GetUsuarioLogeado().Id.ToString()),
                    //new Claim("FullName", user.FullName),
                    new Claim(ClaimTypes.Role, this.agencia.GetUsuarioLogeado().IsAdmin ? "Admin" : "User"),
                    new Claim("Usuario", this.agencia.GetUsuarioLogeado().Nombre),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, "Login"); //CookieAuthenticationDefaults.AuthenticationScheme

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.Now.AddMinutes(60)
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //RedirectUri = <string>
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);



                this.agencia.ReiniciarIntentos(dni);
                if (this.agencia.GetUsuarioLogeado().IsAdmin)
                {
                    // ADMIN
                    /*VistaDashboardAdmin admin = new VistaDashboardAdmin(this.agencia);
                    admin.Show();
                    this.Hide();*/
                    //return View("");
                    _soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
                    _soundPlayer.Play();
                    return Redirect("/Home");
                    //return Redirect("/Alojamientoes");
                }
                else
                {
                    // USUARIO CLIENTE
                    /*VistaDashboardCliente cliente = new VistaDashboardCliente(this.agencia);
                    cliente.Show();
                    this.Hide();*/
                    //return View("Home");
                    //return Redirect("/VistaCliente");
                    _soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
                    _soundPlayer.Play();
                    return Redirect("/Home");
                    //return Redirect("/Alojamientoes/all");
                }
            }
            else
            {
                //MessageBox.Show("Contraseña incorrecta");
                if (intentos >= 3)
                {
                    this.agencia.BloquearUsuario(dni);
                    this.agencia.ReiniciarIntentos(dni);
                    ViewBag.Error = "El usuario ha sido bloqueado, contacte con un administrador";
                }
                else
                {
                    intentos += 1;
                    this.agencia.IntentosLogueo(dni);
                    ViewBag.Error = "Usuario o contraseña invalida";
                }
                _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                _soundPlayer.Play();
                return RedirectToAction("Index");
            }
        }

        [HttpGet("Logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            _soundPlayer = new SoundPlayer("Resources/DeleteSound.wav");
            _soundPlayer.Play();
            return RedirectToAction("Login");
        }
    }
}
