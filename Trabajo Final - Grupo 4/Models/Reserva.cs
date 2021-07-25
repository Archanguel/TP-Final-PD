using System;

namespace Trabajo_Final___Grupo_4.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public Alojamiento Alojamiento { get; set; }  
        public Usuario Usuario { get; set; }
        public double Precio { get; set; }
    }
}