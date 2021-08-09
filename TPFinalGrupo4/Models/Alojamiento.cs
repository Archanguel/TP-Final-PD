using System;

namespace TPFinalGrupo4.Models
{
    public class Alojamiento
    {
        public int Id { get; set; }
        public String Codigo { get; set; }
        public String Ciudad { get; set; }
        public String Barrio { get; set; }
        public int Estrellas { get; set; }
        public int CantidadDePersonas { get; set; }
        public bool Tv { get; set; }
        public String Tipo { get; set; }
        /* HOTEL */
        public double PrecioPorPersona { get; set; }
        /* CABAÑA */
        public double PrecioPorDia { get; set; }
        public int Habitaciones { get; set; }
        public int Banios { get; set; }

    }
}