using Microsoft.EntityFrameworkCore;
using SmartTrip.API.Models;

namespace SmartTrip.API.Data;

public class SmartTripContext : DbContext {
    public SmartTripContext(DbContextOptions<SmartTripContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<TipoDePasseio> TiposDePasseio { get; set; }
    public DbSet<Passeio> Passeios { get; set; }
    public DbSet<PreferenciasDoUsuario> PreferenciasDoUsuario { get; set; }
    public DbSet<HistoricoDePasseioDoUsuario> HistoricosDePasseio { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Usuario
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // TipoDePasseio
        modelBuilder.Entity<TipoDePasseio>()
            .HasIndex(t => t.Nome)
            .IsUnique();

        // Passeio -> TipoDePasseio
        modelBuilder.Entity<Passeio>()
            .HasOne(p => p.Tipo)
            .WithMany(t => t.Passeios)
            .HasForeignKey(p => p.TipoId);

        // PreferenciasDoUsuario -> Usuario
        modelBuilder.Entity<PreferenciasDoUsuario>()
            .HasOne(p => p.Usuario)
            .WithOne(u => u.Preferencias)
            .HasForeignKey<PreferenciasDoUsuario>(p => p.UsuarioId);

        // HistoricoDePasseio -> Usuario
        modelBuilder.Entity<HistoricoDePasseioDoUsuario>()
            .HasOne(h => h.Usuario)
            .WithMany(u => u.Historicos)
            .HasForeignKey(h => h.UsuarioId);

        // HistoricoDePasseio -> Passeio
        modelBuilder.Entity<HistoricoDePasseioDoUsuario>()
            .HasOne(h => h.Passeio)
            .WithMany(p => p.Historicos)
            .HasForeignKey(h => h.PasseioId);

        // Feedback -> Usuario
        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Usuario)
            .WithMany(u => u.Feedbacks)
            .HasForeignKey(f => f.UsuarioId);
    }
}