using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPFinalGrupo4.Data;
using TPFinalGrupo4.Helpers;
using System.Media;

namespace TPFinalGrupo4.Models
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioContext _context;
        private SoundPlayer _soundPlayer;

        public UsuariosController(UsuarioContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }

        // GET: Usuarios/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        [HttpGet("Registrarse")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Registrarse")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dni,Nombre,Email,Password,IsAdmin,Bloqueado,Intentos")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (_context.Usuario.Any(x => String.Equals(x.Dni.ToString(), usuario.Dni.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                    _soundPlayer.Play();
                    ModelState.AddModelError("Dni", "Dni ya registrado");
                }
                else
                {
                    if (_context.Usuario.Any(x => String.Equals(x.Email, usuario.Email, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                        _soundPlayer.Play();
                        ModelState.AddModelError("Email", "Email ya registrado");
                    }
                    else
                    {
                        usuario.Password = Utils.Encriptar(usuario.Password);
                        _context.Add(usuario);
                        await _context.SaveChangesAsync();
                        _soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
                        _soundPlayer.Play();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
            _soundPlayer.Play();
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Dni,Nombre,Email,Password,IsAdmin,Bloqueado,Intentos")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                _soundPlayer.Play();
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                        _soundPlayer.Play();
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
                _soundPlayer.Play();
                return RedirectToAction(nameof(Index));
            }
            _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
            _soundPlayer.Play();
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            _soundPlayer = new SoundPlayer("Resources/DeleteSound.wav");
            _soundPlayer.Play();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }

        // GET: Cambiar Contraseña
        /*[Authorize]
        [HttpGet]
        public IActionResult CambiarContrasena()
        {
            if (this.agencia.FindUserForDNI(dni) == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(dni);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
            var usuario = await _context.Usuario.FindAsync(id); 
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
            return View();
        }*/
        [Authorize]
        [HttpGet("MisDatos")]
        public async Task<IActionResult> MisDatos(String? message)
        {
            var usuario = await _context.Usuario.FindAsync(int.Parse(User.Identity.Name));
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Cambiar Contraseña
        [HttpPost("MisDatos")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MisDatos(Usuario usuario, String Nombre, String Email, String contraseñaActual, int Dni)
        {
            var usuarioActual = this._context.Usuario.Where(user => user.Dni == Dni).FirstOrDefault();

            if (contraseñaActual != null && Utils.Encriptar(contraseñaActual).Equals(usuarioActual.Password))
            {

            if (int.Parse(User.Identity.Name) != usuario.Id)
            {
                _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                _soundPlayer.Play();
                return NotFound();
            }

            if(Nombre != null)
            {
              try
              {
                        usuarioActual.Nombre = Nombre;
                  _context.Update(usuarioActual);
                  await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                        _soundPlayer.Play();
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if(Email != null)
            {

                

                try
                {
                        usuarioActual.Email = Email;
                    _context.Update(usuarioActual);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                        _soundPlayer.Play();
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (usuario.Password != null)
            {
                try
                {
                    usuarioActual.Password = Utils.Encriptar(usuario.Password);
                    _context.Update(usuarioActual);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                        _soundPlayer.Play();
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            }
            // return redirect("Usuarios/MisDatos?message=asd-as-asd-asd-asd-";
            return View(usuario);
        }

    }
}
