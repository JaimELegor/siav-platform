namespace SIAV.Application.DTOs.Inscripciones;

public record InscripcionDto(
    int Id,
    int AlumnoId,
    string NombreAlumno,
    string EmailAlumno,
    int? GrupoId,
    string? NombreGrupo,
    DateTime? FechaInscripcion,
    EstadoInscripcionDto Estado
);

public record CrearInscripcionDto(
    int AlumnoId,
    int GrupoId
);

public record ActualizarInscripcionDto(
    EstadoInscripcionDto Estado
);

public enum EstadoInscripcionDto
{
    SinInscripcion = 0,
    Activa = 1,
    Baja = 2,
    Completada = 3
}