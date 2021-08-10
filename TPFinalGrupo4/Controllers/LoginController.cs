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
        private readonly UsuarioContext _context;
        private SoundPlayer _soundPlayer;

        public LoginController( UsuarioContext context)
        {
            this._context = context;
        }

        [HttpGet("Login")]
        public IActionResult Index(String? message)
        {
            ViewData["message"] = null;
            if (message != null)
                ViewData["message"] = message.Replace("-", " ");
            return View();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync(int dni, String password)
        {
            // Validar el DNI
            var usuario = this._context.Usuario.Where(user => user.Dni == dni).First();
            var passwordEncriptada = Utils.Encriptar(password);

            // Valido el DNI
            if (usuario == null)
            {
                this._soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                this._soundPlayer.Play();
                return Redirect("/Login?message=El-DNI-ingresado-es-incorrecto");
            }

            // Verifico intentos
            if(usuario.Bloqueado)
            {
                this._soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                this._soundPlayer.Play();
                return Redirect("/Login?message=Su-usuario-esta-bloqueado-por-superar-el-limite-de-intentos-permitidos");
            }

            // Validar la contraseña e Incrementar intentos.
            if (usuario.Password != passwordEncriptada)
            {
                usuario.Intentos++;

                // Bloquear usuario por intentos
                if (usuario.Intentos >= 3)
                    usuario.Bloqueado = true;

                this._context.Update(usuario);
                this._context.SaveChanges();

                this._soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                this._soundPlayer.Play();
                return Redirect("/Login?message=La-clave-ingresada-es-incorrecta");
            }

            // LOGEO AL USUARIO
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                //new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Role, usuario.IsAdmin ? "Admin" : "User"),
                new Claim("Usuario", usuario.Nombre),
            };

            //CookieAuthenticationDefaults.AuthenticationScheme
            var claimsIdentity = new ClaimsIdentity(claims, "Login"); 
            
            // Propiedades de la Autenticacion
            var authProperties = new AuthenticationProperties{ExpiresUtc = DateTimeOffset.Now.AddMinutes(60)};

            // Autenticar usuario
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            if (usuario.IsAdmin)
            {
                // ADMIN
                _soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
                _soundPlayer.Play();
                return Redirect("/Home");
                //return Redirect("/Alojamientoes");
            }
            else
            {
                // USUARIO CLIENTE
                _soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
                _soundPlayer.Play();
                //return Redirect("/Home");
                return Redirect("/Alojamientoes/all");
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
