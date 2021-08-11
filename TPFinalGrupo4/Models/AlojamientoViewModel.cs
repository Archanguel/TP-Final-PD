using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPFinalGrupo4.Models
{
    public class AlojamientoViewModel
    {
        public List<String> Ciudades { get; set; }
        public List<int> Estrellas { get; set; }
        public List<int> CantidadDePersonas { get; set; }
        public String Tipo { get; set; }
        public List<String> Tipos { get; set; }
    }
}
