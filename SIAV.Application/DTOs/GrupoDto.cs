namespace SIAV.Application.DTOs;

public record GrupoDto(
    int Id,
    int CursoId,
    string CursoNombre,
    int DocenteId,
    string DocenteNombre,
    string Nombre,
    DateTime FechaInicio,
    DateTime FechaFin,
    int LimiteAlumnos,
    int AlumnosInscritos,
    string Estado
);

public record CrearGrupoDto(
    int CursoId,
    int DocenteId,
    string Nombre,
    DateTime FechaInicio,
    DateTime FechaFin,
    int LimiteAlumnos
);

public record InscribirAlumnoDto(
    int GrupoId
    // AlumnoId viene del JWT
);

public record CursoPorAlumnoDto(
    int GrupoId,
    string GrupoNombre,
    int CursoId,
    string CursoNombre,
    string Categoria,
    DateTime FechaInicio,
    DateTime FechaFin,
    string Estado
);

public record ActualizarGrupoDto(
    string Nombre,
    int DocenteId,
    DateTime FechaInicio,
    DateTime FechaFin,
    int LimiteAlumnos,
    string Estado   // "Abierto", "EnCurso", "Cerrado"
);