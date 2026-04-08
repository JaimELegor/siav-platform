using SIAV.Application.DTOs.Inscripciones;

namespace SIAV.Application.Interfaces;

public interface IEstudianteService
{
    Task<List<InscripcionDto>> ObtenerPorGrupoAsync(int grupoId, int institucionId);
    Task<List<InscripcionDto>> ObtenerPorCursoAsync(int cursoId, int institucionId);
    Task<List<InscripcionDto>> ObtenerPorInstitucionAsync(
        int institucionId, EstadoInscripcionDto? filtroEstado = null);
}