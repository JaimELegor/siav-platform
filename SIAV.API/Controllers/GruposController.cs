using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIAV.Application.DTOs;
using SIAV.Application.DTOs.Docentes;
using SIAV.Application.Interfaces;
using System.Security.Claims;
using SIAV.Domain.Entities;

namespace SIAV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GruposController(IGrupoService grupoService) : ControllerBase
{
    private int InstitucionId =>
        int.Parse(User.FindFirstValue("institucionId")!);

    private int MembresiaId =>
        int.Parse(User.FindFirstValue("membresiaId")!);

    // Coordinador ve todos los grupos de la institución
    [HttpGet]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> GetAll() =>
        Ok(await grupoService.ObtenerPorInstitucionAsync(InstitucionId));

    // Por curso (accesible para todos los roles)
    [HttpGet("por-curso/{cursoId}")]
    public async Task<IActionResult> GetByCurso(int cursoId) =>
        Ok(await grupoService.ObtenerPorCursoAsync(cursoId, InstitucionId));

    [HttpPost]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> Create([FromBody] CrearGrupoDto dto)
    {
        var grupo = await grupoService.CrearAsync(dto, InstitucionId);
        return CreatedAtAction(nameof(GetAll), new { id = grupo.Id }, grupo);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await grupoService.EliminarAsync(id, InstitucionId);
        return ok ? NoContent() : NotFound();
    }

    // Inscripción usando SP
    [HttpPost("inscribir")]
    [Authorize(Roles = "Alumno")]
    public async Task<IActionResult> Inscribir([FromBody] InscribirAlumnoDto dto)
    {
        try
        {
            await grupoService.InscribirAlumnoAsync(MembresiaId, dto.GrupoId);
            return Ok(new { mensaje = "Inscripción exitosa." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // Mis cursos — datos desde SP
    [HttpGet("mis-cursos")]
    [Authorize(Roles = "Alumno")]
    public async Task<IActionResult> MisCursos() =>
        Ok(await grupoService.ObtenerCursosPorAlumnoAsync(MembresiaId));

    [HttpPut("{grupoId}/reasignar-docente")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> ReasignarDocente(
        int grupoId, [FromBody] ReasignarDocenteDto dto)
    {
        try
        {
            await grupoService.ReasignarDocenteAsync(grupoId, dto.NuevoDocenteId, InstitucionId);
            return Ok(new { mensaje = "Docente reasignado exitosamente." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarGrupoDto dto)
    {
        try
        {
            var grupo = await grupoService.ActualizarAsync(id, dto, InstitucionId);
            return grupo is null ? NotFound() : Ok(grupo);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}