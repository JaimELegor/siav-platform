using SIAV.Application.DTOs;
using SIAV.Domain.Entities;

namespace SIAV.Application.Interfaces;

public interface IGrupoService
{
    Task<List<GrupoDto>> ObtenerPorInstitucionAsync(int institucionId);
    Task<List<GrupoDto>> ObtenerPorCursoAsync(int cursoId, int institucionId);
    Task<GrupoDto> CrearAsync(CrearGrupoDto dto, int institucionId);
    Task<bool> EliminarAsync(int id, int institucionId);
    Task InscribirAlumnoAsync(int alumnoMembresiaId, int grupoId);
    Task<List<CursoPorAlumnoResultado>> ObtenerCursosPorAlumnoAsync(int alumnoMembresiaId); // ← cambiado
}