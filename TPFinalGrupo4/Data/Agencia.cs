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

        
        #region METODOS COMPLEMENTARIOS
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
