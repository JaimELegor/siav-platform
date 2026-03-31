using SIAV.Application.DTOs.Docentes;

namespace SIAV.Application.DTOs.Horarios;

public record HorarioDto(
    int Id,
    int GrupoId,
    string GrupoNombre,
    int CursoId,
    string CursoNombre,
    int DocenteId,
    string DocenteNombre,
    string DiaSemana,
    TimeOnly HoraInicio,
    TimeOnly HoraFin
);

public record CrearHorarioDto(
    int GrupoId,
    int DiaSemana,
    TimeOnly HoraInicio,
    TimeOnly HoraFin
);