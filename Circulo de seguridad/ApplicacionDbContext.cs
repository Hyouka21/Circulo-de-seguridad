using Circulo_de_seguridad.Dtos;
using Circulo_de_seguridad.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UsuarioGrupo>().HasKey(al => new { al.GrupoId, al.UsuarioId });
            modelBuilder.Entity<Usuario>().HasIndex(x => new { x.Email }).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(x => new { x.NickName }).IsUnique();
            modelBuilder.Entity<Grupo>().HasIndex(x =>  x.Identificador ).IsUnique();
            
        }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioGrupo> UsuariosGrupos{ get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Localizacion> Localizaciones { get; set; }
        public DbSet<Evento> Eventos { get; set; }
    }
}
