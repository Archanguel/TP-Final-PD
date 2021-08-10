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
    public class AlojamientoesController : Controller
    {
        private readonly UsuarioContext _context;
        private SoundPlayer _soundPlayer;

        public AlojamientoesController(UsuarioContext context)
        {
            _context = context;
        }

        // GET: Alojamientoes
        public async Task<IActionResult> Index(String searchCiudad, String searchTipo)
        {
            var alojamiento = from a in _context.Alojamiento select a;

            if (!String.IsNullOrEmpty(searchCiudad))
            {
                var CiudadElejida = this._context.Ciudad.FirstOrDefault(nombreCiudad => nombreCiudad.Nombre == searchCiudad);
                Console.WriteLine(CiudadElejida);
                //int.Parse(searchCiudad);
                alojamiento = alojamiento.Where(a => a.Ciudad.Contains(CiudadElejida.Codigo));
            }

            if (!String.IsNullOrEmpty(searchTipo))
            {
                alojamiento = alojamiento.Where(a => a.Tipo.Contains(searchTipo));
            }
            var usuarioLogeado = User.Identity;
            return View(await alojamiento.ToListAsync());
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
                /*if (_context.Alojamiento.Any(x => String.Equals(x.Codigo.ToString(), alojamiento.Codigo.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                    _soundPlayer.Play();
                    ModelState.AddModelError("Codigo", "Codigo ya registrado");
                }
                else
                {*/
                    _context.Add(alojamiento);
                    await _context.SaveChangesAsync();
                    _soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
                    _soundPlayer.Play();
                    return RedirectToAction(nameof(Index));
                //}
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
                var CiudadElejida = this._context.Ciudad.FirstOrDefault(nombreCiudad => nombreCiudad.Nombre == searchCiudad);
                Console.WriteLine(CiudadElejida);
                //int.Parse(searchCiudad);
                alojamiento = alojamiento.Where(a => a.Ciudad.Contains(CiudadElejida.Codigo));
            }

            if (!String.IsNullOrEmpty(searchTipo))
            {
                alojamiento = alojamiento.Where(a => a.Tipo.Contains(searchTipo));
            }

            

            return View(await alojamiento.ToListAsync());
        }  
        public async Task<IActionResult> BuscadorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            var alojamiento = from a in _context.Alojamiento select a;

            var reservas = from r in _context.Reserva select r;
            foreach (var item in alojamiento)
            {
                //foreach(var res in reservas)
                //{
                //    bool validarFechaDesde = DateTime.Compare(res.FechaDesde, fechaDesde) == 1 && DateTime.Compare(res.FechaDesde, fechaHasta) == 1;
                //    bool validarFechaHasta = DateTime.Compare(res.FechaHasta, fechaDesde) == -1 && DateTime.Compare(res.FechaHasta, fechaDesde) == -1;
                //    if (!validarFechaDesde && !validarFechaHasta)
                //    {
                //        alojamiento.Except((IEnumerable<Alojamiento>)item);
                //    }
                //}             
            }
            return View(await alojamiento.ToListAsync());
        }

        public async Task<IActionResult> Reservar(DateTime fechaDesde, DateTime fechaHasta, int id_alojamiento)
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
            return Redirect("/Alojamientoes/Buscador");
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
