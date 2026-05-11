namespace SmartTrip.API.Models;

public class TipoDePasseio {
    public int TipoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public ICollection<Passeio>? Passeios { get; set; }
}