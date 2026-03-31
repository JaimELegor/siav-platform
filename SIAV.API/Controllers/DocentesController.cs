using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIAV.Application.DTOs.Docentes;
using SIAV.Application.Interfaces;
using SIAV.Domain.Entities;
using System.Security.Claims;

namespace SIAV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocentesController(IDocenteService docenteService) : ControllerBase
{
    private int InstitucionId =>
        int.Parse(User.FindFirstValue("institucionId")!);
    private int MembresiaId =>
        int.Parse(User.FindFirstValue("membresiaId")!);

    // Coordinador declara disponibilidad de un docente
    [HttpPost("{docenteId}/disponibilidad")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> AgregarDisponibilidad(
        int docenteId, [FromBody] CrearDisponibilidadDto dto)
    {
        try
        {
            var result = await docenteService
                .AgregarDisponibilidadAsync(docenteId, InstitucionId, dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // Coordinador ve disponibilidad de un docente específico
    [HttpGet("{docenteId}/disponibilidad")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> ObtenerDisponibilidad(int docenteId) =>
        Ok(await docenteService.ObtenerDisponibilidadAsync(docenteId, InstitucionId));

    // Docente ve su propia disponibilidad
    [HttpGet("mi-disponibilidad")]
    [Authorize(Roles = "Docente")]
    public async Task<IActionResult> MiDisponibilidad() =>
        Ok(await docenteService.ObtenerDisponibilidadAsync(MembresiaId, InstitucionId));

    // Coordinador elimina un bloque de disponibilidad
    [HttpDelete("disponibilidad/{disponibilidadId}")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> EliminarDisponibilidad(int disponibilidadId)
    {
        var ok = await docenteService
            .EliminarDisponibilidadAsync(disponibilidadId, InstitucionId);
        return ok ? NoContent() : NotFound();
    }

    // Coordinador consulta docentes disponibles en un rango — alimenta el dropdown
    [HttpGet("disponibles")]
    [Authorize(Roles = "Coordinador")]
    public async Task<IActionResult> Disponibles(
        [FromQuery] DiaSemanaDto dia,
        [FromQuery] TimeOnly horaInicio,
        [FromQuery] TimeOnly horaFin)
    {
        var docentes = await docenteService
            .ObtenerDisponiblesAsync(InstitucionId, (DiaSemana)dia, horaInicio, horaFin);
        return Ok(docentes);
    }
}
