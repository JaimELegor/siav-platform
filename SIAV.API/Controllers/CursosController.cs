using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIAV.Application.DTOs;
using SIAV.Application.Interfaces;
using System.Security.Claims;

namespace SIAV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Coordinador")]
public class CursosController(ICursoService cursoService) : ControllerBase
{
    private int InstitucionId =>
        int.Parse(User.FindFirstValue("institucionId")!);

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await cursoService.ObtenerTodosAsync(InstitucionId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var curso = await cursoService.ObtenerPorIdAsync(id, InstitucionId);
        return curso is null ? NotFound() : Ok(curso);
    }

    [HttpPost]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> Create([FromBody] CrearCursoDto dto)
    {
        var curso = await cursoService.CrearAsync(dto, InstitucionId);
        return CreatedAtAction(nameof(GetById), new { id = curso.Id }, curso);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarCursoDto dto)
    {
        var curso = await cursoService.ActualizarAsync(id, dto, InstitucionId);
        return curso is null ? NotFound() : Ok(curso);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await cursoService.EliminarAsync(id, InstitucionId);
        return ok ? NoContent() : NotFound();
    }
}
