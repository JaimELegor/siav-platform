using SIAV.Application.DTOs;

namespace SIAV.Application.Interfaces;

public interface ICursoService
{
    Task<List<CursoDto>> ObtenerTodosAsync(int institucionId);
    Task<CursoDto?> ObtenerPorIdAsync(int id, int institucionId);
    Task<CursoDto> CrearAsync(CrearCursoDto dto, int institucionId);
    Task<CursoDto?> ActualizarAsync(int id, ActualizarCursoDto dto, int institucionId);
    Task<bool> EliminarAsync(int id, int institucionId);
}