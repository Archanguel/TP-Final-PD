using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using TPFinalGrupo4.Models;
using TPFinalGrupo4.Helpers;
using TPFinalGrupo4.Data;
using System.Linq;

namespace TPFinalGrupo4
{
    public class AgenciaManager
    {
        private UsuarioContext contexto;
        private Agencia agencia;

        private Usuario usuarioLogeado;
        public AgenciaManager(UsuarioContext context, Agencia agencia)
        {
            contexto = context;
            this.agencia = agencia;
        }

        #region USUARIO
        /*public bool AgregarUsuario(int dni, String nombre, String email, String password, bool isAdmin, bool bloqueado)
        {
            try
            {
                Usuario nuevo = new Usuario
                {
                    Dni = dni,
                    Nombre = nombre,
                    Email = email,
                    Password = Utils.Encriptar(password),
                    IsAdmin = isAdmin,
                    Bloqueado = bloqueado
                };
                contexto.Usuario.Add(nuevo);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ModificarUsuario(int dni, String nombre, String email, bool bloqueado)
        {
            try
            {
                var usuario = this.contexto.Usuario.FirstOrDefault(u => u.Dni == dni);
		        usuario.Nombre = nombre;
                usuario.Email = email;
                usuario.Bloqueado = bloqueado;
                this.contexto.Usuario.Update(usuario);
                this.contexto.SaveChanges();
		        return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EliminarUsuario(int dni)
        {
            try
            {
                var reservas = this.contexto.Reserva.Where(reserva => reserva.Usuario.Dni == dni).ToList();
                this.contexto.Reserva.RemoveRange(reservas.ToArray());
                var usuario = this.contexto.Usuario.ToList().Find(u => u.Dni == dni);
                this.contexto.Usuario.Remove(usuario);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }*/
        public bool IsUsuarioBloqueado(int dni)
        {
            Usuario user = this.contexto.Usuario.ToList().Find(user => user.Dni == dni && user.Bloqueado == true);
            return user == null ? false : true;
        }
        public Usuario FindUserForDNI(int dni)
        {
            return this.contexto.Usuario.ToList().Find(user => user.Dni == dni);
        }
        public bool autenticarUsuario(int dni, String password)
        {
            Usuario usuarioEncontrado = this.FindUserForDNI(dni);
            if (usuarioEncontrado == null) return false; // DNI no encontrado
            if (usuarioEncontrado.Password != Utils.Encriptar(password)) return false; // Contraseña incorrecta          
            this.usuarioLogeado = usuarioEncontrado;
            return true;
        }
        public bool RecuperarPassword(int dni, String password)
        {
            var usuario = this.contexto.Usuario.FirstOrDefault(u => u.Dni == dni);
            usuario.Password = Utils.Encriptar(password);
            this.contexto.Usuario.Update(usuario);
            this.contexto.SaveChanges();
            return true;
        }
        public bool ExisteEmail(string email)
        {
            try
            {
                return this.contexto.Usuario.Where(user => user.Email == email).First() != null;
            }
            catch
            {
                return false;
            }
        }
        public bool BloquearUsuario(int dni)
        {
            Usuario usuario = this.contexto.Usuario.ToList().Find(u => u.Dni == dni);
            if (usuario == null) return false;
            usuario.Bloqueado = true;
            this.contexto.Usuario.Update(usuario);
            this.contexto.SaveChanges();
            return true;
        }
        public void CerrarSession()
        {
            this.usuarioLogeado = null;
        }
        public bool ReiniciarIntentos(int dni)
        {
            try
            {
                var usuario = this.contexto.Usuario.FirstOrDefault(u => u.Dni == dni);
                usuario.Intentos = 0;
                this.contexto.Usuario.Update(usuario);
                this.contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IntentosLogueo(int dni)
        {
            try
            {
                var usuario = this.contexto.Usuario.FirstOrDefault(u => u.Dni == dni);
                usuario.Intentos = usuario.Intentos + 1;
                this.contexto.Usuario.Update(usuario);
                this.contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region ALOJAMIENTOS
        /*public bool AgregarHotel(String codigo, String ciudad, String barrio, int estrellas, int cantidadDePersonas, bool tv, double precioPorPersona)
        {
            var hotel = new Alojamiento
            {
                Codigo = codigo,
                Ciudad = ciudad,
                Barrio = barrio,
                Estrellas = estrellas,
                CantidadDePersonas = cantidadDePersonas,
                Tv = tv,
                Tipo = "hotel",
                PrecioPorPersona = precioPorPersona,
            };
            return this.agencia.AgregarAlojamiento(hotel);
        }
        public bool ModificarHotel(String codigo, String ciudad, String barrio, int estrellas, int cantidadDePersonas, bool tv, double precioPorPersona)
        {
            var hotel = new Alojamiento
            {
                Codigo = codigo,
                Ciudad = ciudad,
                Barrio = barrio,
                Estrellas = estrellas,
                CantidadDePersonas = cantidadDePersonas,
                Tipo = "hotel",
                Tv = tv,
                PrecioPorPersona = precioPorPersona
            };
            return this.agencia.ModificarAlojamiento(hotel);
        }
        public bool AgregarCabania(String codigo, String ciudad, String barrio, int estrellas, int cantidadDePersonas, bool tv, double precioPorDia, int habitaciones, int banios)
        {
            var cabania = new Alojamiento {
                Codigo = codigo,
                Ciudad = ciudad,
                Barrio = barrio,
                Estrellas = estrellas,
                CantidadDePersonas = cantidadDePersonas,
                Tipo = "cabaña",
                Tv = tv,
                PrecioPorDia = precioPorDia,
                Habitaciones = habitaciones,
                Banios = banios
            };
            return this.agencia.AgregarAlojamiento(cabania);
        }
        public bool ModificarCabania(String codigo, String ciudad, String barrio, int estrellas, int cantidadDePersonas, bool tv, double precioPorDia, int habitaciones, int banios){
            var cabania = new Alojamiento
            {
                Codigo = codigo,
                Ciudad = ciudad,
                Barrio = barrio,
                Estrellas = estrellas,
                CantidadDePersonas = cantidadDePersonas,
                Tipo = "cabaña",
                Tv = tv,
                PrecioPorDia = precioPorDia,
                Habitaciones = habitaciones,
                Banios = banios
            };
            return this.agencia.ModificarAlojamiento(cabania);
        }
        public bool EliminarAlojamiento(int codigo)
        {
            try
            {
                var reservasDelAlojamiento = this.contexto.Reserva.Where(r => r.Alojamiento.Codigo == codigo.ToString()).ToList();
                this.contexto.Reserva.RemoveRange(reservasDelAlojamiento.ToArray());
                this.contexto.SaveChanges();
                return this.agencia.EliminarAlojamiento(codigo);
            }
            catch
            {
                return false;
            }
        }*/
        private List<Reserva> getAllReservasForAlojamiento(String codigo)
        {
            return this.contexto.Reserva.ToList().FindAll(reserva => reserva.Alojamiento.Codigo == codigo);
        }
        public bool ExisteAlojamiento(int codigo)
        {
            return this.agencia.FindAlojamientoForCodigo(codigo) != null ? true : false;
        }
        #endregion

        #region RESERVAS
        /*public bool AgregarReserva(DateTime fechaDesde, DateTime fechaHasta, String codigoAlojamiento, int dniUsuario, double precio)
        {
            var alojamiento = this.contexto.Alojamiento.Where(a => a.Codigo.Equals(codigoAlojamiento)).FirstOrDefault();
            var usuario = this.contexto.Usuario.Where(u => u.Dni == dniUsuario).FirstOrDefault();
            try
            {
                var reservas = new Reserva
                {
                    FechaDesde = fechaDesde,
                    FechaHasta = fechaHasta,
                    Alojamiento = alojamiento,
                    Usuario = usuario,
                    Precio = precio
                };

                this.contexto.Reserva.Add(reservas);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ModificarReserva(String id, DateTime fechaDesde, DateTime fechaHasta, int precio, int alojamiento_id, int usuario_dni)
        {
            try
            {
                var alojamiento = this.Alojamientos.FirstOrDefault( a => a.Codigo == alojamiento_id.ToString());
                var usuario = this.contexto.Usuario.FirstOrDefault( a => a.Dni == usuario_dni );
                var reserva = this.contexto.Reserva.FirstOrDefault( r => r.Id == int.Parse(id) );
                reserva.FechaDesde = fechaDesde;
                reserva.FechaHasta = fechaHasta;
                reserva.Precio = precio;
                reserva.Alojamiento = alojamiento;
                reserva.Usuario = usuario;
                this.contexto.Reserva.Update(reserva);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EliminarReserva(int id)
        {
            try
            {
                var reserva = this.contexto.Reserva.FirstOrDefault(r => r.Id == id);
                contexto.Reserva.Remove(reserva);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }*/
        public List<Reserva> GetAllReservasForUsuario(int dni)
        {
            return this.contexto.Reserva.ToList().FindAll(reserva => reserva.Usuario.Dni == dni);
        }
        private List<Reserva> getAllReservasForAlojamiento(int codigo)
        {
            return this.contexto.Reserva.Where(reserva => reserva.Alojamiento.Codigo == codigo.ToString()).ToList();
        }
        public bool ElAlojamientoEstaDisponible(String codigoDeAlojamiento, DateTime fechaDesde, DateTime fechaHasta)
        {
            bool alojamientoDisponible = true;
            foreach (Reserva reserva in this.getAllReservasForAlojamiento(codigoDeAlojamiento))
            {
                bool validarFechaDesde = DateTime.Compare(reserva.FechaDesde, fechaDesde) == 1 && DateTime.Compare(reserva.FechaDesde, fechaHasta) == 1;
                bool validarFechaHasta = DateTime.Compare(reserva.FechaHasta, fechaDesde) == -1 && DateTime.Compare(reserva.FechaHasta, fechaDesde) == -1;
                if (!validarFechaDesde && !validarFechaHasta)
                    alojamientoDisponible = false;
            }
            return alojamientoDisponible;
        }
        #endregion

        #region INFO PARA LAS VISTAS
        public List<String> OpcionesDelSelectDeTiposDeAlojamientos()
        {
            return new List<String>() { "todos", "hotel", "cabaña" };
        }
        public List<String> OpcionesDelSelectDePersonas()
        {
            List<String> opciones = new List<String>() { "todas" };
            for (int i = 1; i <= Agencia.MAXIMA_CANTIDAD_DE_PERSONAS_POR_ALOJAMIENTO; i++)
                opciones.Add(i.ToString());
            return opciones;
        }
        public List<String> OpcionesDelSelectDeEstrellas()
        {
            List<String> opciones = new List<String>() { "todas" };
            for (int i = Agencia.MINIMA_CANTIDAD_DE_ESTRELLAS; i <= Agencia.MAXIMA_CANTIDAD_DE_ESTRELLAS; i++)
                opciones.Add(i.ToString());
            return opciones;
        }
        public List<String> OpcionesDelSelectDeBarrios()
        {
            List<String> tipos = new List<string>() { "todos" };
            foreach (Alojamiento al in this.contexto.Alojamiento)
                tipos.Add(al.Barrio);
            return tipos.Distinct().ToList();
        }
        public List<String> OpcionesDelSelectDeCiudades()
        {
            List<String> tipos = new List<string>() { "todas" };
            foreach (Alojamiento al in this.contexto.Alojamiento)
                tipos.Add(al.Ciudad.ToString());
            return tipos.Distinct().ToList();
        }
        public List<String> OpcionesDelSelectParaElOrdenamiento()
        {
            return new List<String>() { "fecha de creacion", "personas", "estrellas" };
        }

        public List<List<String>> BuscarDeAlojamientosPorCiudadYFechas(String ciudad, DateTime fechaDesde, DateTime fechaHasta)
        {
            List<List<String>> alojamientos = new List<List<string>>();
            List<Alojamiento> alojamientosFiltrados = new List<Alojamiento>();

            foreach (var alojamiento in this.contexto.Alojamiento.ToList().FindAll(al => al.Ciudad.Equals(ciudad)))
            {
                if (this.ElAlojamientoEstaDisponible(alojamiento.Codigo, fechaDesde, fechaHasta))
                    alojamientosFiltrados.Add(alojamiento);
            }

            foreach (Alojamiento alojamiento in alojamientosFiltrados)
            {
                alojamientos.Add(new List<string>()
                {
                    alojamiento.Codigo.ToString(),
                    alojamiento.Tipo is "Hotel" ? "hotel" : "cabaña",
                    alojamiento.Ciudad.ToString(),
                    alojamiento.Barrio,
                    alojamiento.Estrellas.ToString(),
                    alojamiento.CantidadDePersonas.ToString(),
                    alojamiento.Tv.ToString(),
                    alojamiento.Tipo is "Hotel" ? (alojamiento).PrecioPorPersona.ToString() : (alojamiento).PrecioPorDia.ToString()
                });
            }

            return alojamientos;
        }
        public List<List<String>> GetUsuarios()
        {
            var usuarios = new List<List<String>>();
            foreach (var usuario in this.contexto.Usuario)
                usuarios.Add(new List<string>()
                {
                    usuario.Dni.ToString(),
                    usuario.Nombre,
                    usuario.Email,
                    usuario.IsAdmin.ToString(),
                    usuario.Bloqueado.ToString()
                });
            return usuarios;
        }
        public List<List<String>> DatosDeReservasParaLasVistas(String tipoDeUsuario)
        {
            List<List<String>> reservas = new List<List<String>>();

            if (tipoDeUsuario == "admin")
            {
                foreach (Reserva reserva in this.contexto.Reserva)
                {
                    reservas.Add(new List<String>(){
                        reserva.Id.ToString(),
                        reserva.FechaDesde.ToString(),
                        reserva.FechaHasta.ToString(),
                        reserva.Alojamiento.Codigo.ToString(),
                        reserva.Usuario.Dni.ToString(),
                        reserva.Precio.ToString(),
                    });
                }
            }
            else if (tipoDeUsuario == "user")
            {
                // Reservas del usuario
                List<Reserva> reservasDelUsuario = this.GetAllReservasForUsuario(this.usuarioLogeado.Dni);

                foreach (Reserva reserva in reservasDelUsuario)
                {
                    reservas.Add(new List<String>(){
                        reserva.Alojamiento.Tipo is "hotel" ? "hotel" : "cabaña",
                        reserva.FechaDesde.ToString(),
                        reserva.FechaHasta.ToString(),
                        reserva.Precio.ToString(),
                    });
                }
            }
            return reservas;
        }
        #endregion

        #region FILTRAR
        public List<List<String>> FiltrarAlojamientos(String tipoAlojamiento, String ciudad, String barrio, double precioMin, double precioMax, String estrellas, String personas)
        {
            List<List<String>> alojamientosFiltrados = new List<List<string>>();
            //var alojamiento = this.contexto.Alojamiento.Where(a => a.Codigo.Equals(codigoAlojamiento)).FirstOrDefault();
            var ciudades = from Ciudad in this.contexto.Ciudad select ciudad;
            var alojamientos = from alojamiento in this.contexto.Alojamiento
                               select alojamiento;

            if (tipoAlojamiento != "todos")
                alojamientos = alojamientos.Where(a => a.Tipo == tipoAlojamiento);

            if (ciudad != "todas")
           //     alojamientos = alojamientos.Where(a => a.Ciudad == int.Parse(ciudad));

            if (barrio != "todos")
                alojamientos = alojamientos.Where(a => a.Barrio == barrio);

            if (estrellas != "todas")
                alojamientos = alojamientos.Where(a => a.Estrellas == int.Parse(estrellas));

            if (personas != "todas")
                alojamientos = alojamientos.Where(a => a.CantidadDePersonas == int.Parse(personas));

            if (precioMin - precioMax != 0)
                alojamientos = alojamientos.Where(a => (a.Banios == 0 && precioMin < a.PrecioPorPersona && precioMax > a.PrecioPorPersona) ||
                (a.Banios != 0 && precioMin < a.PrecioPorDia && precioMax > a.PrecioPorDia));

            foreach (var al in alojamientos)
            {
                alojamientosFiltrados.Add(new List<string>()
                {
                    al.Codigo,
                    al.Tipo,
                    al.Ciudad.ToString(),
                    al.Barrio,
                    al.Estrellas.ToString(),
                    al.CantidadDePersonas.ToString(),
                    al.Tv ? "si" : "no",
                    al.Tipo == "hotel" ? al.PrecioPorPersona.ToString() : al.PrecioPorDia.ToString()
                });
            }

            return alojamientosFiltrados;
        }
        #endregion

        
        /* GETTERS */
        //public Agencia GetAgencia() { return this.agencia; }
        public Usuario GetUsuarioLogeado() { return this.usuarioLogeado; }
    }
}
