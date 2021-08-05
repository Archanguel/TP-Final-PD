using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Trabajo_Final___Grupo_4.Models;
using Trabajo_Final___Grupo_4.Data;
using Trabajo_Final___Grupo_4.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Trabajo_Final___Grupo_4.Controllers
{
    public class LoginController : Controller
    {
        private int contadorDeIntentos;
        private int dniIngresado;
        //ILogger<LoginController> logger;

        private readonly AgenciaManager agencia;

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
            //int dni = int.Parse("1234");
            //String password = "1234";
            int intentos = 0;

            if (this.agencia.FindUserForDNI(dni) == null) //!_context.Usuario.Any(x => x.Dni == dni)
            {
                //MessageBox.Show("No existe ese usuario");
                //return;
                ViewBag.Error = "Usuario o contraseña invalida";
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
                    new Claim(ClaimTypes.Name, this.agencia.GetUsuarioLogeado().Nombre),
                    //new Claim("FullName", user.FullName),
                    new Claim(ClaimTypes.Role, this.agencia.GetUsuarioLogeado().IsAdmin.ToString()),
                    new Claim("id", this.agencia.GetUsuarioLogeado().Id.ToString()),
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
                    //return View("Home");
                    return Redirect("/Abm");
                }
                else
                {
                    // USUARIO CLIENTE
                    /*VistaDashboardCliente cliente = new VistaDashboardCliente(this.agencia);
                    cliente.Show();
                    this.Hide();*/
                    //return View("Home");
                    //return Redirect("/VistaCliente");
                    return Redirect("/Home");
                }
            }
            else
            {
                this.agencia.IntentosLogueo(dni);
                intentos += 1;
                //MessageBox.Show("Contraseña incorrecta");
                if (intentos >= 3)
                {
                    this.agencia.BloquearUsuario(dni);
                    this.agencia.ReiniciarIntentos(dni);
                    ViewBag.Error = "El usuario ha sido bloqueado, contacte con un administrador";
                }
                else
                {
                    ViewBag.Error = "Usuario o contraseña invalida";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpGet("Logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
