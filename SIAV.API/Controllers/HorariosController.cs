using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIAV.Application.DTOs.Horarios;
using SIAV.Application.Interfaces;
using System.Security.Claims;

namespace SIAV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HorariosController(IHorarioService horarioService) : ControllerBase
{
    private int InstitucionId =>
        int.Parse(User.FindFirstValue("institucionId")!);
    private int MembresiaId =>
        int.Parse(User.FindFirstValue("membresiaId")!);

    [HttpPost]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> Crear([FromBody] CrearHorarioDto dto)
    {
        try
        {
            var horario = await horarioService.CrearAsync(dto, InstitucionId);
            return CreatedAtAction(nameof(GetAll), new { id = horario.Id }, horario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // Admin ve todos
    [HttpGet]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> GetAll() =>
        Ok(await horarioService.ObtenerPorInstitucionAsync(InstitucionId));

    // Docente ve los suyos
    [HttpGet("mis-horarios")]
    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> MisHorarios() =>
        Ok(await horarioService.ObtenerPorDocenteAsync(MembresiaId, InstitucionId));

    // Coordinador consulta horarios de un docente específico
    [HttpGet("por-docente/{docenteId}")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> PorDocente(int docenteId) =>
        Ok(await horarioService.ObtenerPorDocenteAsync(docenteId, InstitucionId));

    // Alumno ve horarios de sus cursos inscritos
    [HttpGet("mis-clases")]
    [Authorize(Roles = "Alumno")]
    public async Task<IActionResult> MisClases() =>
        Ok(await horarioService.ObtenerPorAlumnoAsync(MembresiaId, InstitucionId));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var ok = await horarioService.EliminarAsync(id, InstitucionId);
        return ok ? NoContent() : NotFound();
    }
}