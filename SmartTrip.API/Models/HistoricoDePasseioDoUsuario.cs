using System.ComponentModel.DataAnnotations;

namespace SmartTrip.API.Models;

public class HistoricoDePasseioDoUsuario {
    [Key]
    public int HistoricoId { get; set; }
    public int UsuarioId { get; set; }
    public int PasseioId { get; set; }
    public DateTime? DataPasseio { get; set; }
    public string? Status { get; set; }
    public int? Avaliacao { get; set; }
    public string? Comentario { get; set; }

    public Usuario? Usuario { get; set; }
    public Passeio? Passeio { get; set; }
}