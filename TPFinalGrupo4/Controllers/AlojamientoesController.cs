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
            if (!this._context.Usuario.Find(int.Parse(User.Identity.Name)).IsAdmin)
                return Redirect("/Alojamientoes/all?message=No-tenes-permiso-de-administrador");

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
        [Authorize(Roles = "User")]
        public async Task<IActionResult> all(int? precio, String? estrellas, String? cantidadDePersonas, String? message)
        {
            ViewData["message"] = null;
            if (message != null)
                ViewData["message"] = message.Replace("-", " ");


            var alojamientos = from alojamiento in this._context.Alojamiento
                               select alojamiento;
       
            if (precio != null && estrellas != null && cantidadDePersonas != null)
            {
                alojamientos = alojamientos
                    .Where(al => al.PrecioPorDia >= precio || al.PrecioPorPersona >= precio)
                    .Where(al => al.Estrellas >= int.Parse(estrellas))
                    .Where(al => al.CantidadDePersonas >= int.Parse(cantidadDePersonas));
            }

            // Ciudades
            var ciudades = this._context.Ciudad.ToList();

            // Todos los alojamientos o alojamientos filtrados
            var alojamientosParaVistas = new List<Alojamiento>();

            // Agrego el nombre de la ciudad 
            foreach (var alojamiento in await alojamientos.ToListAsync())
            {
                alojamiento.Ciudad = ciudades.First(c => c.Codigo == alojamiento.Ciudad).Nombre;
                alojamientosParaVistas.Add(alojamiento);
            }

            return View(alojamientosParaVistas);
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

        public IActionResult Create(String? message)
        {        
            ViewData["message"] = null;
            if (message != null)
            {
                ViewData["message"] = message.Replace("-", " ");
            }
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
                if (_context.Alojamiento.Any(x => String.Equals(x.Codigo, alojamiento.Codigo, StringComparison.InvariantCultureIgnoreCase)))
                {
                    _soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
                    _soundPlayer.Play();
                    ModelState.AddModelError("Dni", "Dni ya registrado");
                }
                else
                {
                    if (alojamiento.Tipo == "Hotel")
                    {
                        alojamiento.PrecioPorDia = 0;
                        alojamiento.Habitaciones = 0;
                        alojamiento.Banios = 0;
                    }
                    else
                    {
                        alojamiento.PrecioPorPersona = 0;
                    }

                    this._context.Add(alojamiento);
                    await this._context.SaveChangesAsync();
                    this._soundPlayer = new SoundPlayer("Resources/SuccessSound.wav");
                    this._soundPlayer.Play();
                    return RedirectToAction(nameof(Index));
                }
            }
            this._soundPlayer = new SoundPlayer("Resources/ErrorSound.wav");
            this._soundPlayer.Play();
            return Redirect("/Alojamientoes/Create?message=El-codigo-ingresado-ya-existe");
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Buscador(String? ciudad, String? tipoDeAlojamiento, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            // Muestra todos los alojamientos porque no se uso el buscador
            if (tipoDeAlojamiento == null || fechaDesde == null || fechaHasta == null)
                return View(await this._context.Alojamiento.Where(a => 1==0).ToListAsync());

            // Lista vacia
            var alojamientosEncontrados = new List<Alojamiento>();
            // Todos los alojamientos
            var alojamientos = from alojamiento in this._context.Alojamiento
                               select alojamiento;

            // Buscar por ciudad
            if(ciudad != null)
            {
                // Ciudades que coinciden
                var ciudades = this._context.Ciudad.Where(c => c.Nombre.ToUpper().Contains(ciudad.ToUpper())).Select(c => c.Codigo).ToList();

                // Si no encontro ciudades devuelvo una lista vacia
                if( ciudades.Count == 0)
                {
                    return View(await this._context.Alojamiento.Where(a => 1 == 0).ToListAsync());
                }
                alojamientos = alojamientos.Where(al => ciudades.Contains(al.Ciudad));
            }
            
            // Filtro por tipo de alojamiento
            if (tipoDeAlojamiento != "Todos")
                alojamientos = alojamientos.Where(al => al.Tipo == tipoDeAlojamiento);

            foreach (var alojamiento in alojamientos.ToList())
            {
                if (this.DisponibilidadPorFechas(alojamiento.Id, (DateTime)fechaDesde, (DateTime)fechaHasta))
                    alojamientosEncontrados.Add(alojamiento);
            }

            if (alojamientosEncontrados.Count != 0)
            {
                // Ciudades
                var ciudades = this._context.Ciudad.ToList();

                // Todos los alojamientos o alojamientos filtrados
                var alojamientosParaVistas = new List<Alojamiento>();

                // Agrego el nombre de la ciudad 
                foreach (var alojamiento in await alojamientos.ToListAsync())
                {
                    alojamiento.Ciudad = ciudades.First(c => c.Codigo == alojamiento.Ciudad).Nombre;
                    alojamientosParaVistas.Add(alojamiento);
                }
                alojamientosEncontrados = alojamientosParaVistas;
            }
            ViewData["fechaDesde"] = ((DateTime)fechaDesde).ToString();
            ViewData["fechaHasta"] = ((DateTime)fechaHasta).ToString();

            return View(alojamientosEncontrados);
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
