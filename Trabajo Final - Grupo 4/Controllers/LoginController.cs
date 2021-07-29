using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Trabajo_Final___Grupo_4.Models;
using Trabajo_Final___Grupo_4.Data;

namespace Trabajo_Final___Grupo_4.Controllers
{
    public class LoginController : Controller
    {
        private int contadorDeIntentos;
        private int dniIngresado;
        ILogger<LoginController> logger;

        private AgenciaManager agencia;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(int dni, String password)
        {
            //int dni = int.Parse("1234");
            //String password = "1234";

            if (this.agencia.FindUserForDNI(dni) == null)
            {
                //MessageBox.Show("No existe ese usuario");
                //return;
                ViewBag.Error = "Usuario o contraseña invalida";
                return View();
            }


            this.agencia.BloquearUsuario(dni);
            // Al bloquear al usuario salgo del metodo con el return
            //if (this.bloquearUsuarioPorIntentos(dni)) return;

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
                //MessageBox.Show("Contraseña incorrecta");
                ViewBag.Error = "Usuario o contraseña invalida";
                return View();
            }
        }
    }
}
