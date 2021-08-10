using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPFinalGrupo4.Models;
using TPFinalGrupo4.Helpers;
using Microsoft.Extensions.Logging;

namespace TPFinalGrupo4.Data
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext()
        {
        }

        public UsuarioContext (DbContextOptions<UsuarioContext> options)
            : base(options)
        {
        }

        public DbSet<TPFinalGrupo4.Models.Usuario> Usuario { get; set; }

        public DbSet<TPFinalGrupo4.Models.Reserva> Reserva { get; set; }

        public DbSet<TPFinalGrupo4.Models.Alojamiento> Alojamiento { get; set; }
        public DbSet<TPFinalGrupo4.Models.Ciudad> Ciudad { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(usuario =>
            {
                usuario.Property(u => u.Nombre).HasColumnType("varchar(80)").IsRequired(true);
                usuario.Property(u => u.Dni).HasColumnType("int").IsRequired(true);
                usuario.HasIndex(u => u.Dni).IsUnique();
                usuario.Property(u => u.Email).HasColumnType("varchar(30)").IsRequired(true);
                usuario.HasIndex(u => u.Email).IsUnique();
                usuario.Property(u => u.Password).HasColumnType("varchar(200)").IsRequired(true);
                usuario.Property(u => u.Intentos).HasColumnType("int");
            });
            modelBuilder.Entity<Alojamiento>(alojamiento =>
            {
                alojamiento.Property(a => a.Codigo).HasColumnType("varchar(50)").IsRequired(true);
                alojamiento.HasIndex(a => a.Codigo).IsUnique();
                alojamiento.Property(a => a.Ciudad).HasColumnType("varchar(50)").IsRequired(true);
                alojamiento.Property(a => a.Barrio).HasColumnType("varchar(50)").IsRequired(true);
                alojamiento.Property(a => a.Tipo).HasColumnType("varchar(10)").IsRequired(true);
            });
            modelBuilder.Entity<Usuario>().HasData(new Usuario[]{
                new Usuario{Id=1, Dni = 1234, Nombre = "admin", Email = "admin@admin.com", Password = Utils.Encriptar("1234"), IsAdmin=true, Bloqueado=false, Intentos=0},
                new Usuario{Id=2, Dni = 12312312, Nombre = "prueba1", Email = "prueba1@gmail.com", Password = Utils.Encriptar("1234"), IsAdmin=false, Bloqueado=false, Intentos=0},
                new Usuario{Id=3, Dni = 23423423, Nombre = "prueba2", Email = "prueba2@gmail.com", Password = Utils.Encriptar("1234"), IsAdmin=false, Bloqueado=true, Intentos=0},
                new Usuario{Id=4, Dni = 34534534, Nombre = "prueba3", Email = "prueba3@gmail.com", Password = Utils.Encriptar("1234"), IsAdmin=false, Bloqueado=false, Intentos=0},
            }); ;
            modelBuilder.Entity<Alojamiento>().HasData(new Alojamiento[] {
                new Alojamiento{
                    Id=1,
                    Codigo="352234",
                    Ciudad ="1",
                    Barrio="Recoleta",
                    Estrellas=3,
                    CantidadDePersonas=2,
                    Tv = true,
                    Tipo="Hotel" ,
                    PrecioPorPersona=2400
                },
                new Alojamiento{
                    Id=2,
                    Codigo="934120",
                    Ciudad="2",
                    Barrio="Sur",
                    Estrellas=4,
                    CantidadDePersonas=2,
                    Tv = true,
                    Tipo="Cabaña" ,
                    PrecioPorDia = 1200,
                    Habitaciones = 4,
                    Banios = 2
                },
                new Alojamiento{
                    Id=3,
                    Codigo="846445",
                    Ciudad="1",
                    Barrio="Puerto Madero",
                    Estrellas=2,
                    CantidadDePersonas=2,
                    Tv = true,
                    Tipo="HSotel" ,
                    PrecioPorPersona=6400
                },
                new Alojamiento{
                    Id=4,
                    Codigo="321632",
                    Ciudad="3",
                    Barrio="Centro",
                    Estrellas=1,
                    CantidadDePersonas=5,
                    Tv = true,
                    Tipo="Cabaña" ,
                    PrecioPorDia = 2800,
                    Habitaciones = 1,
                    Banios = 1
                },
            });
            modelBuilder.Entity<Ciudad>(ciudad =>
            {
                ciudad.Property(c => c.Nombre).HasColumnType("varchar(80)").IsRequired(true);
                ciudad.Property(c => c.Provincia).HasColumnType("varchar(80)").IsRequired(true);
                ciudad.Property(c => c.Pais).HasColumnType("varchar(80)").IsRequired(true);
            });
            modelBuilder.Entity<Ciudad>().HasData(new Ciudad[]{
                new Ciudad
                {
                    ID = 1,
                    Codigo = "1",
                    Nombre = "Buenos Aires",
                    Provincia = "CABA",
                    Pais = "Argentina"
                },
                 new Ciudad
                {
                    ID = 2,
                    Codigo = "2",
                    Nombre = "Neuquen",
                    Provincia = "Neuquen",
                    Pais = "Argentina"
                },
                  new Ciudad
                {
                    ID = 3,
                    Codigo = "3",
                    Nombre = "Carlos paz",
                    Provincia = "Córdoba",
                    Pais = "Argentina"
                },
            });
        }
    }
}
