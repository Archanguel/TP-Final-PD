using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TPFinalGrupo4.Models;

namespace TPFinalGrupo4.Data
{
    public class Agencia
    {
        public const int MAXIMA_CANTIDAD_DE_PERSONAS_POR_ALOJAMIENTO = 10;
        public const int MINIMA_CANTIDAD_DE_ESTRELLAS = 1;
        public const int MAXIMA_CANTIDAD_DE_ESTRELLAS = 5;

        private UsuarioContext contexto;

        public Agencia(UsuarioContext contexto)
        {
            this.contexto = contexto;
        }

        #region ABM DE ALOJAMIENTO
        //public bool AgregarAlojamiento(Alojamiento alojamiento)
        //{
        //    try
        //    {
        //        contexto.Alojamiento.Add(alojamiento);
        //        contexto.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        //public bool ModificarAlojamiento(Alojamiento alojamiento)
        //{
        //    try
        //    {
        //        var alojamientoEncontrado = this.contexto.Alojamiento.FirstOrDefault(a => a.Codigo == alojamiento.Codigo);
        //        alojamientoEncontrado.Ciudad = alojamiento.Ciudad;
        //        alojamientoEncontrado.Barrio = alojamiento.Barrio;
        //        alojamientoEncontrado.Estrellas = alojamiento.Estrellas;
        //        alojamientoEncontrado.CantidadDePersonas = alojamiento.CantidadDePersonas;
        //        alojamientoEncontrado.Tv = alojamiento.Tv;
        //        alojamientoEncontrado.PrecioPorPersona = alojamiento.PrecioPorPersona;
        //        alojamientoEncontrado.PrecioPorDia = alojamiento.PrecioPorDia;
        //        alojamientoEncontrado.Habitaciones = alojamiento.Habitaciones;
        //        alojamientoEncontrado.Banios = alojamiento.Banios;
        //        this.contexto.Alojamiento.Update(alojamientoEncontrado);
        //        contexto.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        //public bool EliminarAlojamiento(int codigoDelAlojamiento)
        //{
        //    try
        //    {
        //        var alojamiento = this.contexto.Alojamiento.FirstOrDefault(a => a.Codigo == codigoDelAlojamiento.ToString());
        //        this.contexto.Alojamiento.Remove(alojamiento);
        //        contexto.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        #endregion
        
        #region METODOS COMPLEMENTARIOS
        //public List<List<String>> DatosDeAlojamientosParaLasVistasAdmin(String tipoAloj)
        //{
        //    List<List<String>> alojamientos = new List<List<string>>();
        //    foreach (Alojamiento alojamiento in this.Alojamientos)
        //    {
        //        if (tipoAloj == "todo")
        //        {
        //            double precio = alojamiento.Tipo == "hotel" ? alojamiento.PrecioPorPersona : alojamiento.PrecioPorDia;
        //            contexto.Alojamiento.Add(new List<String>(){
        //            alojamiento.Codigo.ToString(),
        //            alojamiento.Ciudad,
        //            alojamiento.Barrio,
        //            alojamiento.Estrellas.ToString(),
        //            alojamiento.CantidadDePersonas.ToString(),
        //            alojamiento.Tv ? "si" : "no",
        //            precio.ToString(),
        //            });
        //        }
        //        else if (alojamiento.Tipo == tipoAloj)
        //        {
        //            double precio = alojamiento.Tipo == "hotel" ? alojamiento.PrecioPorPersona : alojamiento.PrecioPorDia;
        //            contexto.Alojamiento.Add(new List<String>(){
        //            alojamiento.Codigo.ToString(),
        //            alojamiento.Ciudad,
        //            alojamiento.Barrio,
        //            alojamiento.Estrellas.ToString(),
        //            alojamiento.CantidadDePersonas.ToString(),
        //            alojamiento.Tv ? "si" : "no",
        //            precio.ToString(),
        //        });
        //        }
        //    }
        //    return alojamientos;
        //}

        public List<List<String>> GetAlojamientoPorCodigo()
        {
            List<Alojamiento> alojamientos = this.contexto.Alojamiento.OrderBy(alojamiento => alojamiento.Codigo).ToList();
            return this.AlojamientosToLista(alojamientos);
        }
        public List<List<String>> GetAlojamientoPorEstrellas()
        {
            List<Alojamiento> alojamientos = this.contexto.Alojamiento.OrderBy(alojamiento => alojamiento.Estrellas).ToList();
            return this.AlojamientosToLista(alojamientos);
        }
        public List<List<String>> GetAlojamientoPorPersonas()
        {
            List<Alojamiento> alojamientos = this.contexto.Alojamiento.OrderBy(alojamiento => alojamiento.CantidadDePersonas).ToList();
            return this.AlojamientosToLista(alojamientos);
        }
        public List<List<String>> AlojamientosToLista(List<Alojamiento> alojamientos = null)
        {
            List<List<String>> listaDeAlojamientos = new List<List<string>>();

            var alojamientosAIterar = alojamientos == null ? this.contexto.Alojamiento.ToList() : alojamientos;

            foreach (Alojamiento al in alojamientosAIterar)
            {
                listaDeAlojamientos.Add(
                    new List<string>()
                    {
                        al.Codigo,
                        al.Tipo,
                        al.Ciudad.ToString(),
                        al.Barrio,
                        al.Estrellas.ToString(),
                        al.CantidadDePersonas.ToString(),
                        al.Tv ? "si" : "no",
                        al.Tipo == "hotel" ? al.PrecioPorPersona.ToString() : al.PrecioPorDia.ToString()
                    }
                    );
            }
            return listaDeAlojamientos;
        }
        public List<List<String>> GetCabania()
        {
            List<List<String>> cabanias = new List<List<String>>();
            this.contexto.Alojamiento.ToList().ForEach(alojamiento =>
            {
                if (alojamiento.Tipo != "hotel")
                {
                    cabanias.Add(new List<string>() {
                        alojamiento.Codigo,
                        alojamiento.Ciudad.ToString(),
                        alojamiento.Barrio,
                        alojamiento.Estrellas.ToString(),
                        alojamiento.CantidadDePersonas.ToString(),
                        alojamiento.Tv.ToString(),
                        alojamiento.PrecioPorDia.ToString(),
                        alojamiento.Habitaciones.ToString(),
                        alojamiento.Banios.ToString(),
                        (alojamiento.PrecioPorDia * alojamiento.CantidadDePersonas).ToString()
                    });
                }
            });
            return cabanias;
        }
        public List<List<String>> GetHoteles()
        {
            List<List<String>> hoteles = new List<List<String>>();
            this.contexto.Alojamiento.ToList().ForEach( alojamiento =>
            {
                if (alojamiento.Tipo == "hotel")
                {
                    hoteles.Add(new List<string>() {
                        alojamiento.Codigo,
                        alojamiento.Ciudad.ToString(),
                        alojamiento.Barrio,
                        alojamiento.Estrellas.ToString(),
                        alojamiento.CantidadDePersonas.ToString(),
                        alojamiento.Tv.ToString(),
                        alojamiento.PrecioPorPersona.ToString(),
                        (alojamiento.PrecioPorPersona* alojamiento.CantidadDePersonas).ToString()
                    });
                }
            });
            return hoteles;
        }
        public Alojamiento FindAlojamientoForCodigo(int codigoAlojamiento)
        {
            return this.contexto.Alojamiento.ToList().Find(al => al.Codigo == codigoAlojamiento.ToString());
        }
        #endregion

    }
}
