using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIAV.Application.DTOs.Inscripciones;
using SIAV.Application.Interfaces;

namespace SIAV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EstudiantesController(IEstudianteService estudianteService) : ControllerBase
{
    // GET api/estudiantes?institucionId=1
    // GET api/estudiantes?institucionId=1&estado=Activa
    [HttpGet]
    [Authorize(Roles = "Coordinador,Docente")]
    public async Task<IActionResult> ObtenerPorInstitucion(
        [FromQuery] int institucionId,
        [FromQuery] EstadoInscripcionDto? estado = null)
    {
        try
        {
            var result = await estudianteService.ObtenerPorInstitucionAsync(institucionId, estado);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // GET api/estudiantes/grupo/3?institucionId=1
    [HttpGet("grupo/{grupoId}")]
    [Authorize(Roles = "Coordinador,Docente")]
    public async Task<IActionResult> ObtenerPorGrupo(
        int grupoId,
        [FromQuery] int institucionId)
    {
        try
        {
            var result = await estudianteService.ObtenerPorGrupoAsync(grupoId, institucionId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // GET api/estudiantes/curso/2?institucionId=1
    [HttpGet("curso/{cursoId}")]
    [Authorize(Roles = "Coordinador,Docente")]
    public async Task<IActionResult> ObtenerPorCurso(
        int cursoId,
        [FromQuery] int institucionId)
    {
        try
        {
            var result = await estudianteService.ObtenerPorCursoAsync(cursoId, institucionId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}