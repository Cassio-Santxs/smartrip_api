using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTrip.API.Data;
using SmartTrip.API.DTOs;
using SmartTrip.API.Models;

namespace SmartTrip.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoricoController : ControllerBase {
    private readonly SmartTripContext _context;

    public HistoricoController(SmartTripContext context)
    {
        _context = context;
    }

    // GET: api/historico/1
    [HttpGet("{usuarioId}")]
    public async Task<ActionResult<IEnumerable<HistoricoDePasseioDoUsuario>>> GetHistorico(int usuarioId)
    {
        var historico = await _context.HistoricosDePasseio
            .Include(h => h.Passeio)
            .Where(h => h.UsuarioId == usuarioId)
            .OrderByDescending(h => h.DataPasseio)
            .ToListAsync();

        return Ok(historico);
    }

    // POST: api/historico
    [HttpPost]
    public async Task<ActionResult<HistoricoDePasseioDoUsuario>> RegistrarHistorico(HistoricoDePasseioDoUsuario historico)
    {
        historico.DataPasseio = DateTime.Now;
        _context.HistoricosDePasseio.Add(historico);
        await _context.SaveChangesAsync();
        return Ok(historico);
    }

    // PUT: api/historico/avaliar/1
    [HttpPut("avaliar/{historicoId}")]
    public async Task<IActionResult> Avaliar(int historicoId, [FromBody] AvaliacaoRequest request)
    {
        var historico = await _context.HistoricosDePasseio.FindAsync(historicoId);
        if (historico == null) return NotFound();

        historico.Avaliacao = request.Avaliacao;
        historico.Comentario = request.Comentario;
        historico.Status = "Concluído";

        await _context.SaveChangesAsync();
        return NoContent();
    }
}