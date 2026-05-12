using System.ComponentModel.DataAnnotations;

namespace SmartTrip.API.Models;

public class PreferenciasDoUsuario {
    [Key]
    public int PreferenciaId { get; set; }
    public int UsuarioId { get; set; }
    public bool PrefereNatureza { get; set; }
    public bool PrefereUrbano { get; set; }
    public bool PrefereAventura { get; set; }
    public bool PrefereCultural { get; set; }
    public bool PrefereGastronomico { get; set; }
    public decimal? OrcamentoMedio { get; set; }

    public Usuario? Usuario { get; set; }
}