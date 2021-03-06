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
      
        public async Task<IActionResult> Index()
        {
            if (!this._context.Usuario.Find(int.Parse(User.Identity.Name)).IsAdmin)
                return Redirect("/Alojamientoes/all?message=No-tenes-permiso-de-administrador");
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

            double precio = 0;
            if (alojamiento.Tipo == "Hotel")
            {
                precio = alojamiento.CantidadDePersonas * alojamiento.PrecioPorPersona;
            }
            else
            {
                precio = alojamiento.PrecioPorDia;
            }

            ViewData["precioTotal"] = precio;

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
                return Redirect("/Reservas/Create?id=" + id_alojamiento +"&message=El-alojamiento-no-esta-disponible-en-esas-fechas");
            }

            var usuario = this._context.Usuario.Find(int.Parse(User.Identity.Name));
            var alojamiento = await this._context.Alojamiento.FindAsync(id_alojamiento);
            int dias_reservados = (fechaHasta - fechaDesde).Days;

            double precio = dias_reservados * alojamiento.PrecioPorDia;
            if (alojamiento.Tipo == "Hotel")
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
            return Redirect("/Reservas/List");
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var reserva = await _context.Reserva.FindAsync(id);
            var todasLasreserva = await _context.Reserva.Include(r => r.Alojamiento).Include(r => r.Usuario).ToListAsync();
            var reserva = todasLasreserva.Find(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            ViewData["reserva_id"] = id;
            ViewData["alojamiento_id"] = reserva.Alojamiento.Id;
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int reserva_id, int alojamiento_id, DateTime FechaDesde, DateTime FechaHasta)
        {
            var reserva = this._context.Reserva.Find(reserva_id);
            var alojamiento = this._context.Alojamiento.Find(alojamiento_id);
            var usuario = this._context.Usuario.Find(int.Parse(User.Identity.Name));

            if(reserva == null || alojamiento == null || usuario == null)
            {
                this._soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                this._soundPlayer.Play();
                return NotFound();
            }
            int diasDeReservas = (FechaHasta - FechaDesde).Days;
            double precio = alojamiento.Tipo == "Hotel" ? 
                alojamiento.PrecioPorPersona * diasDeReservas : 
                alojamiento.PrecioPorDia * alojamiento.CantidadDePersonas * diasDeReservas;

            try
            {
                reserva.FechaDesde = FechaDesde;
                reserva.FechaHasta = FechaHasta;
                reserva.Precio = precio;
                this._context.Reserva.Update(reserva);
                this._context.SaveChanges();
            }
            catch (Exception)
            {
                this._soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                this._soundPlayer.Play();
                return RedirectToAction(nameof(Index));
            }

            this._soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
            this._soundPlayer.Play();
            return RedirectToAction(nameof(Index));
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
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(List));
            }
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
