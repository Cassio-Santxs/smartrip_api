using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTrip.API.Data;
using SmartTrip.API.Models;

namespace SmartTrip.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreferenciasController : ControllerBase {
    private readonly SmartTripContext _context;

    public PreferenciasController(SmartTripContext context)
    {
        _context = context;
    }

    // GET: api/preferencias/1
    [HttpGet("{usuarioId}")]
    public async Task<ActionResult<PreferenciasDoUsuario>> GetPreferencias(int usuarioId)
    {
        var preferencias = await _context.PreferenciasDoUsuario
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

        if (preferencias == null) return NotFound();
        return preferencias;
    }

    // POST: api/preferencias
    [HttpPost]
    public async Task<ActionResult<PreferenciasDoUsuario>> CriarPreferencias(PreferenciasDoUsuario preferencias)
    {
        var existe = await _context.PreferenciasDoUsuario
            .AnyAsync(p => p.UsuarioId == preferencias.UsuarioId);

        if (existe) return BadRequest("Usuário já possui preferências cadastradas.");

        _context.PreferenciasDoUsuario.Add(preferencias);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPreferencias), new { usuarioId = preferencias.UsuarioId }, preferencias);
    }

    // PUT: api/preferencias/1
    [HttpPut("{usuarioId}")]
    public async Task<IActionResult> AtualizarPreferencias(int usuarioId, PreferenciasDoUsuario preferencias)
    {
        if (usuarioId != preferencias.UsuarioId) return BadRequest();

        _context.Entry(preferencias).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}