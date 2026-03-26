namespace SIAV.Application.DTOs;

public record CursoDto(
    int Id,
    string Nombre,
    string Descripcion,
    string Categoria,
    bool TieneNiveles,
    string? LinkGoogleForm
);

public record CrearCursoDto(
    string Nombre,
    string Descripcion,
    string Categoria,
    bool TieneNiveles,
    string? LinkGoogleForm
);

public record ActualizarCursoDto(
    string Nombre,
    string Descripcion,
    string Categoria,
    bool TieneNiveles,
    string? LinkGoogleForm
);