using System;

namespace Trabajo_Final___Grupo_4.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int Dni { get; set; }
        public String Nombre { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool Bloqueado { get; set; }
        public int Intentos { get; set; }
    }

}