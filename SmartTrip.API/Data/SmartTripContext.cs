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
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("USUARIO");
            e.HasKey(u => u.UsuarioId);
            e.Property(u => u.UsuarioId).HasColumnName("usuario_id");
            e.Property(u => u.Nome).HasColumnName("nome");
            e.Property(u => u.Email).HasColumnName("email");
            e.Property(u => u.Senha).HasColumnName("senha");
            e.Property(u => u.DataCadastro).HasColumnName("data_cadastro");
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<TipoDePasseio>(e =>
        {
            e.ToTable("TIPO_DE_PASSEIO");
            e.HasKey(t => t.TipoId);
            e.Property(t => t.TipoId).HasColumnName("tipo_id");
            e.Property(t => t.Nome).HasColumnName("nome");
            e.Property(t => t.Descricao).HasColumnName("descricao");
            e.HasIndex(t => t.Nome).IsUnique();
        });

        modelBuilder.Entity<Passeio>(e =>
        {
            e.ToTable("PASSEIO");
            e.HasKey(p => p.PasseioId);
            e.Property(p => p.PasseioId).HasColumnName("passeio_id");
            e.Property(p => p.TipoId).HasColumnName("tipo_id");
            e.Property(p => p.Nome).HasColumnName("nome");
            e.Property(p => p.Descricao).HasColumnName("descricao");
            e.Property(p => p.Localizacao).HasColumnName("localizacao");
            e.Property(p => p.Preco).HasColumnName("preco");
            e.Property(p => p.DuracaoHoras).HasColumnName("duracao_horas");
            e.Property(p => p.Ativo).HasColumnName("ativo");
            e.Property(p => p.Latitude).HasColumnName("latitude");
            e.Property(p => p.Longitude).HasColumnName("longitude");
            e.HasOne(p => p.Tipo)
                .WithMany(t => t.Passeios)
                .HasForeignKey(p => p.TipoId);
        });

        modelBuilder.Entity<PreferenciasDoUsuario>(e =>
        {
            e.ToTable("PREFERENCIAS_DO_USUARIO");
            e.HasKey(p => p.PreferenciaId);
            e.Property(p => p.PreferenciaId).HasColumnName("preferencia_id");
            e.Property(p => p.UsuarioId).HasColumnName("usuario_id");
            e.Property(p => p.PrefereNatureza).HasColumnName("prefere_natureza");
            e.Property(p => p.PrefereUrbano).HasColumnName("prefere_urbano");
            e.Property(p => p.PrefereAventura).HasColumnName("prefere_aventura");
            e.Property(p => p.PrefereCultural).HasColumnName("prefere_cultural");
            e.Property(p => p.PrefereGastronomico).HasColumnName("prefere_gastronomico");
            e.Property(p => p.OrcamentoMedio).HasColumnName("orcamento_medio");
            e.HasOne(p => p.Usuario)
                .WithOne(u => u.Preferencias)
                .HasForeignKey<PreferenciasDoUsuario>(p => p.UsuarioId);
        });

        modelBuilder.Entity<HistoricoDePasseioDoUsuario>(e =>
        {
            e.ToTable("HISTORICO_DE_PASSEIO_DO_USUARIO");
            e.HasKey(h => h.HistoricoId);
            e.Property(h => h.HistoricoId).HasColumnName("historico_id");
            e.Property(h => h.UsuarioId).HasColumnName("usuario_id");
            e.Property(h => h.PasseioId).HasColumnName("passeio_id");
            e.Property(h => h.DataPasseio).HasColumnName("data_passeio");
            e.Property(h => h.Status).HasColumnName("status");
            e.Property(h => h.Avaliacao).HasColumnName("avaliacao");
            e.Property(h => h.Comentario).HasColumnName("comentario");
            e.HasOne(h => h.Usuario)
                .WithMany(u => u.Historicos)
                .HasForeignKey(h => h.UsuarioId);
            e.HasOne(h => h.Passeio)
                .WithMany(p => p.Historicos)
                .HasForeignKey(h => h.PasseioId);
        });

        modelBuilder.Entity<Feedback>(e =>
        {
            e.ToTable("FEEDBACK");
            e.HasKey(f => f.FeedbackId);
            e.Property(f => f.FeedbackId).HasColumnName("feedback_id");
            e.Property(f => f.UsuarioId).HasColumnName("usuario_id");
            e.Property(f => f.Descricao).HasColumnName("descricao");
            e.Property(f => f.DataFeedback).HasColumnName("data_feedback");
            e.Property(f => f.Avaliacao).HasColumnName("avaliacao");
            e.HasOne(f => f.Usuario)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UsuarioId);
        });
    }
}