using SIAV.Application.DTOs.Docentes;
using SIAV.Domain.Entities;

namespace SIAV.Application.Interfaces;

public interface IDocenteService
{
    // Disponibilidad declarada
    Task<DisponibilidadDto> AgregarDisponibilidadAsync(int docenteId, int institucionId, CrearDisponibilidadDto dto);
    Task<List<DisponibilidadDto>> ObtenerDisponibilidadAsync(int docenteId, int institucionId);
    Task<bool> EliminarDisponibilidadAsync(int disponibilidadId, int institucionId);

    // Consulta de disponibilidad real (declarada - asignada)
    Task<List<DocenteDisponibleDto>> ObtenerDisponiblesAsync(
        int institucionId,
        DiaSemana dia,
        TimeOnly horaInicio,
        TimeOnly horaFin);
}