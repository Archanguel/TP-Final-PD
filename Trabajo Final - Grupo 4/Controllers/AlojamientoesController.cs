using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trabajo_Final___Grupo_4.Data;
using System.Media;

namespace Trabajo_Final___Grupo_4.Models
{
    [Authorize]
    public class AlojamientoesController : Controller
    {
        private readonly UsuarioContext _context;
        private SoundPlayer _soundPlayer;

        public AlojamientoesController(UsuarioContext context)
        {
            _context = context;
        }

        // GET: Alojamientoes
        public async Task<IActionResult> Index()
        {
            var usuarioLogeado = User.Identity;
            return View(await _context.Alojamiento.ToListAsync());
        }

        // GET: Alojamientoes
        public async Task<IActionResult> all(int? precio, String? estrellas, String? cantidadDePersonas)
        {
            var alojamientos = from alojamiento in this._context.Alojamiento
                               select alojamiento;
       
            if (precio != null && estrellas != null && cantidadDePersonas != null)
            {
                alojamientos = alojamientos
                    .Where(al => al.PrecioPorDia >= precio || al.PrecioPorPersona >= precio)
                    .Where(al => al.Estrellas >= int.Parse(estrellas))
                    .Where(al => al.CantidadDePersonas >= int.Parse(cantidadDePersonas));
            }
       
            return View(alojamientos.ToList());
        }

        // GET: Alojamientoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alojamiento = await _context.Alojamiento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alojamiento == null)
            {
                return NotFound();
            }

            return View(alojamiento);
        }

        // GET: Alojamientoes/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: Alojamientoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Ciudad,Barrio,Estrellas,CantidadDePersonas,Tv,Tipo,PrecioPorPersona,PrecioPorDia,Habitaciones,Banios")] Alojamiento alojamiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alojamiento);
                await _context.SaveChangesAsync();
                _soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
                _soundPlayer.Play();
                return RedirectToAction(nameof(Index));
            }
            _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
            _soundPlayer.Play();
            return View(alojamiento);
        }

        // GET: Alojamientoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alojamiento = await _context.Alojamiento.FindAsync(id);
            if (alojamiento == null)
            {
                return NotFound();
            }
            return View(alojamiento);
        }

        // POST: Alojamientoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Ciudad,Barrio,Estrellas,CantidadDePersonas,Tv,Tipo,PrecioPorPersona,PrecioPorDia,Habitaciones,Banios")] Alojamiento alojamiento)
        {
            if (id != alojamiento.Id)
            {
                _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                _soundPlayer.Play();
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alojamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlojamientoExists(alojamiento.Id))
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
            return View(alojamiento);
        }

        // GET: Alojamientoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alojamiento = await _context.Alojamiento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alojamiento == null)
            {
                return NotFound();
            }

            return View(alojamiento);
        }

        // POST: Alojamientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alojamiento = await _context.Alojamiento.FindAsync(id);
            _context.Alojamiento.Remove(alojamiento);
            await _context.SaveChangesAsync();
            _soundPlayer = new SoundPlayer("Resources/DeleteSound.wav");
            _soundPlayer.Play();
            return RedirectToAction(nameof(Index));
        }

        private bool AlojamientoExists(int id)
        {
            return _context.Alojamiento.Any(e => e.Id == id);
        }

        // GET: Alojamientoes/Edit/Buscar
        [Authorize]
        public async Task<IActionResult> Buscador(String searchCiudad, String searchTipo)
        {
            var alojamiento = from a in _context.Alojamiento select a;

            if (!String.IsNullOrEmpty(searchCiudad))
            {
                alojamiento = alojamiento.Where(a => a.Ciudad.Contains(searchCiudad));
            }

            if (!String.IsNullOrEmpty(searchTipo))
            {
                alojamiento = alojamiento.Where(a => a.Tipo.Contains(searchTipo));
            }

            return View(await alojamiento.ToListAsync());
        }

    }
}
