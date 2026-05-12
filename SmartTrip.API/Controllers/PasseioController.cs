using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTrip.API.Data;
using SmartTrip.API.Models;

namespace SmartTrip.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PasseioController : ControllerBase {
    private readonly SmartTripContext _context;

    public PasseioController(SmartTripContext context)
    {
        _context = context;
    }

    // GET: api/passeio
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Passeio>>> GetPasseios()
    {
        return await _context.Passeios
            .Include(p => p.Tipo)
            .Where(p => p.Ativo)
            .ToListAsync();
    }

    // GET: api/passeio/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Passeio>> GetPasseio(int id)
    {
        var passeio = await _context.Passeios
            .Include(p => p.Tipo)
            .FirstOrDefaultAsync(p => p.PasseioId == id);

        if (passeio == null) return NotFound();
        return passeio;
    }

    // GET: api/passeio/recomendados/1
    [HttpGet("recomendados/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<Passeio>>> GetRecomendados(int usuarioId)
    {
        var preferencias = await _context.PreferenciasDoUsuario
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

        if (preferencias == null) return NotFound("Preferências não encontradas.");

        var query = _context.Passeios
            .Include(p => p.Tipo)
            .Where(p => p.Ativo);

        if (preferencias.OrcamentoMedio.HasValue)
            query = query.Where(p => p.Preco <= preferencias.OrcamentoMedio);

        var passeios = await query.ToListAsync();

        var recomendados = passeios.Where(p =>
            (preferencias.PrefereNatureza && p.Tipo!.Nome.Contains("Natureza")) ||
            (preferencias.PrefereUrbano && p.Tipo!.Nome.Contains("Urbano")) ||
            (preferencias.PrefereAventura && p.Tipo!.Nome.Contains("Aventura")) ||
            (preferencias.PrefereCultural && p.Tipo!.Nome.Contains("Cultural")) ||
            (preferencias.PrefereGastronomico && p.Tipo!.Nome.Contains("Gastronomia"))
        ).ToList();

        return Ok(recomendados);
    }

    // POST: api/passeio
    [HttpPost]
    public async Task<ActionResult<Passeio>> CriarPasseio(Passeio passeio)
    {
        _context.Passeios.Add(passeio);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPasseio), new { id = passeio.PasseioId }, passeio);
    }

    // PUT: api/passeio/1
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarPasseio(int id, Passeio passeio)
    {
        if (id != passeio.PasseioId) return BadRequest();

        _context.Entry(passeio).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/passeio/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarPasseio(int id)
    {
        var passeio = await _context.Passeios.FindAsync(id);
        if (passeio == null) return NotFound();

        passeio.Ativo = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}