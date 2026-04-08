namespace SIAV.Application.DTOs.Docentes;

public record DocenteEstadoDto(
    int Id,
    string Nombre,
    string Email,
    EstadoDocenteDto Estado
);

public enum EstadoDocenteDto
{
    Baja = 0,
    ActivoSinClases = 1,
    ActivoConClases = 2
}