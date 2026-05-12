using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTrip.API.Data;
using SmartTrip.API.Models;
using SmartTrip.API.DTOs;

namespace SmartTrip.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase {
    private readonly SmartTripContext _context;

    public UsuarioController(SmartTripContext context)
    {
        _context = context;
    }

    // GET: api/usuario
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

    // GET: api/usuario/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Preferencias)
            .FirstOrDefaultAsync(u => u.UsuarioId == id);

        if (usuario == null) return NotFound();
        return usuario;
    }

    // POST: api/usuario/cadastrar
    [HttpPost("cadastrar")]
    public async Task<ActionResult<Usuario>> Cadastrar(Usuario usuario)
    {
        var existe = await _context.Usuarios
            .AnyAsync(u => u.Email == usuario.Email);

        if (existe) return BadRequest("E-mail já cadastrado.");

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
        usuario.DataCadastro = DateTime.Now;

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioId }, usuario);
    }

    // POST: api/usuario/login
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (usuario == null) return Unauthorized("E-mail ou senha inválidos.");

        bool senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.Senha);
        if (!senhaValida) return Unauthorized("E-mail ou senha inválidos.");

        return Ok(new { mensagem = "Login realizado com sucesso.", usuarioId = usuario.UsuarioId, nome = usuario.Nome });
    }
}