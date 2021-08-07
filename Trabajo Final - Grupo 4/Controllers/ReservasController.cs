﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trabajo_Final___Grupo_4.Data;

namespace Trabajo_Final___Grupo_4.Models
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly UsuarioContext _context;

        public ReservasController(UsuarioContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reserva.ToListAsync());
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Reservas/Create
        public IActionResult Create(int? id)
        {
            if (id == null) return NotFound();
            var alojamiento = this._context.Alojamiento.FirstOrDefaultAsync(alojamiento => alojamiento.Id == id).Result;
            if (alojamiento == null) return NotFound();

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
            int usuario_id = int.Parse(User.Identity.Name);
            var usuario = this._context.Usuario.Find(usuario_id);
            var alojamiento = await this._context.Alojamiento.FindAsync(id_alojamiento);
            int dias_reservados = (fechaHasta - fechaDesde).Days;
            double precio = dias_reservados * alojamiento.PrecioPorDia;
            if (alojamiento.Tipo == "hotel")
                precio = dias_reservados * alojamiento.CantidadDePersonas * alojamiento.PrecioPorPersona;

            var reserva = new Reserva {
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                Alojamiento = alojamiento,
                Precio = precio,
                Usuario = usuario
            };

            this._context.Reserva.Add(reserva);
            this._context.SaveChanges();
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
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reserva.Any(e => e.Id == id);
        }
    }
}
