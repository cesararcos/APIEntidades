using System;
using System.Collections.Generic;
using APIEntidades.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIEntidades.Infrastructure.DataAccess;

public partial class EntidadesDbContext : DbContext
{
    public EntidadesDbContext()
    {
    }

    public EntidadesDbContext(DbContextOptions<EntidadesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calificaciones> Calificaciones { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    public virtual DbSet<Videojuegos> Videojuegos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calificaciones>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nickname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Puntaje).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.VideojuegoNavigation).WithMany(p => p.Calificaciones)
                .HasForeignKey(d => d.Videojuego)
                .HasConstraintName("FK_Calificaciones_Videojuegos");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Videojuegos>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Compania)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Puntaje).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
