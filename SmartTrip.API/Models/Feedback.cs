using System.ComponentModel.DataAnnotations;

namespace SmartTrip.API.Models;

public class Feedback {
    [Key]
    public int FeedbackId { get; set; }
    public int UsuarioId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataFeedback { get; set; } = DateTime.Now;
    public int? Avaliacao { get; set; }

    public Usuario? Usuario { get; set; }
}