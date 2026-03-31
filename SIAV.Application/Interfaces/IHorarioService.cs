using SIAV.Application.DTOs.Horarios;
using SIAV.Domain.Entities;

namespace SIAV.Application.Interfaces;

public interface IHorarioService
{
    Task<HorarioDto> CrearAsync(CrearHorarioDto dto, int institucionId);
    Task<List<HorarioDto>> ObtenerPorInstitucionAsync(int institucionId);
    Task<List<HorarioDto>> ObtenerPorDocenteAsync(int docenteId, int institucionId);
    Task<List<HorarioDto>> ObtenerPorAlumnoAsync(int alumnoId, int institucionId);
    Task<bool> EliminarAsync(int id, int institucionId);
}