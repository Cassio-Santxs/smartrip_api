namespace SmartTrip.API.Models;

public class Usuario {
    public int UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public PreferenciasDoUsuario? Preferencias { get; set; }
    public ICollection<HistoricoDePasseioDoUsuario>? Historicos { get; set; }
    public ICollection<Feedback>? Feedbacks { get; set; }
}