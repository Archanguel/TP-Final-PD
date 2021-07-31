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
        public ActionResult Login(int dni, String password)
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
                this.agencia.ReiniciarIntentos(dni);
                if (this.agencia.GetUsuarioLogeado().IsAdmin)
                {
                    // ADMIN
                    /*VistaDashboardAdmin admin = new VistaDashboardAdmin(this.agencia);
                    admin.Show();
                    this.Hide();*/
                    //return View("Home");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // USUARIO CLIENTE
                    /*VistaDashboardCliente cliente = new VistaDashboardCliente(this.agencia);
                    cliente.Show();
                    this.Hide();*/
                    //return View("Home");
                    return RedirectToAction("Index", "Home");
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
        /*[HttpPost("Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SetAuthCookie();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }*/
    }
}
