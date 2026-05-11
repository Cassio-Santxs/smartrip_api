namespace SmartTrip.API.Models;

public class Passeio {
    public int PasseioId { get; set; }
    public int TipoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Localizacao { get; set; }
    public decimal? Preco { get; set; }
    public int? DuracaoHoras { get; set; }
    public bool Ativo { get; set; } = true;

    public TipoDePasseio? Tipo { get; set; }
    public ICollection<HistoricoDePasseioDoUsuario>? Historicos { get; set; }
}