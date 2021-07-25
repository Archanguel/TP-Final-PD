using System;
using System.Collections.Generic;
using System.Text;

namespace Trabajo_Final___Grupo_4.Helpers
{
    class Credenciales
    {
        private const String SERVER = "localhost";
        private const String DATABASE = "inicio-proyecto";
        private const String USER = "root";
        private const String PASSWORD = "";
        private const int PORT = 3306;

        public static String GetConnectionString()
        {
            return $"server={Credenciales.SERVER};user={Credenciales.USER};database={Credenciales.DATABASE};port={Credenciales.PORT};password={Credenciales.PASSWORD}";
        }
    }
}
