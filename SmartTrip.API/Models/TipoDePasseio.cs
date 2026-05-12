using System.ComponentModel.DataAnnotations;

namespace SmartTrip.API.Models;

public class TipoDePasseio {
    [Key]
    public int TipoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public ICollection<Passeio>? Passeios { get; set; }
}