using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPFinalGrupo4.Data;
using System.Media;

namespace TPFinalGrupo4.Models
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly UsuarioContext _context;
        private SoundPlayer _soundPlayer;

        public ReservasController(UsuarioContext context)
        {
            _context = context;
        }

        // GET: Reservas
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reserva.Include(r => r.Alojamiento).Include(r => r.Usuario).ToListAsync());
        }

        // GET: Reservas
        public async Task<IActionResult> List()
        {
            var reservas = from reserva in this._context.Reserva.Include(r => r.Alojamiento) select reserva;

            reservas = reservas.Where(reserva => reserva.Usuario.Id == Int32.Parse(@User.Identity.Name));

            return View(reservas.ToList());
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva.Include(r => r.Alojamiento).Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        [HttpGet]
        public IActionResult Create(int? id, String? message)
        {
            if (id == null) return NotFound();
            var alojamiento = this._context.Alojamiento.FirstOrDefaultAsync(alojamiento => alojamiento.Id == id).Result;
            if (alojamiento == null) return NotFound();

            ViewData["message"] = null;
            if (message != null)
                ViewData["message"] = message.Replace("-", " ");

            ViewData["alojamiento_id"] = alojamiento.Id;
            ViewData["alojamiento_ciudad"] = alojamiento.Ciudad;
            ViewData["alojamiento_barrio"] = alojamiento.Barrio;
            ViewData["alojamiento_estrellas"] = alojamiento.Estrellas;
            ViewData["alojamiento_cantidadDePersonas"] = alojamiento.CantidadDePersonas;
            ViewData["alojamiento_tv"] = alojamiento.Tv;
            ViewData["alojamiento_tipo"] = alojamiento.Tipo;
            ViewData["alojamiento_precioPorPersona"] = alojamiento.PrecioPorPersona;
            ViewData["alojamiento_precioPorDia"] = alojamiento.PrecioPorDia;
            ViewData["alojamiento_habitaciones"] = alojamiento.Habitaciones;
            ViewData["alojamiento_banios"] = alojamiento.Banios;

            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime fechaDesde, DateTime fechaHasta, int id_alojamiento)
        {
            if (!this.DisponibilidadPorFechas(id_alojamiento, fechaDesde, fechaHasta))
            {
                _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                _soundPlayer.Play();
                return Redirect("/Reservas/Create?id=" + id_alojamiento);
            }

            var usuario = this._context.Usuario.Find(int.Parse(User.Identity.Name));
            var alojamiento = await this._context.Alojamiento.FindAsync(id_alojamiento);
            int dias_reservados = (fechaHasta - fechaDesde).Days;

            double precio = dias_reservados * alojamiento.PrecioPorDia;
            if (alojamiento.Tipo == "hotel")
                precio = dias_reservados * alojamiento.CantidadDePersonas * alojamiento.PrecioPorPersona;

            var reserva = new Reserva
            {
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                Alojamiento = alojamiento,
                Precio = precio,
                Usuario = usuario
            };

            this._context.Reserva.Add(reserva);
            this._context.SaveChanges();
            _soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
            _soundPlayer.Play();
            return Redirect("/Alojamientoes/all");
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaDesde,FechaHasta,Precio")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                _soundPlayer.Play();
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id, String? message)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);
            _context.Reserva.Remove(reserva);
            await _context.SaveChangesAsync();
            _soundPlayer = new SoundPlayer("Resources/DeleteSound.wav");
            _soundPlayer.Play();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reserva.Any(e => e.Id == id);
        }

        private bool DisponibilidadPorFechas(int id_alojamiento, DateTime fechaDesde, DateTime fechaHasta)
        {
            foreach (var reserva in this._context.Reserva.Where(r => r.Alojamiento.Id == id_alojamiento).ToList())
            {
                bool validarFechaDesde = DateTime.Compare(reserva.FechaDesde, fechaDesde) == 1 && DateTime.Compare(reserva.FechaDesde, fechaHasta) == 1;
                bool validarFechaHasta = DateTime.Compare(reserva.FechaHasta, fechaDesde) == -1 && DateTime.Compare(reserva.FechaHasta, fechaDesde) == -1;
                if (!validarFechaDesde && !validarFechaHasta)
                    return false;
            }
            return true;
        }
    }
}
