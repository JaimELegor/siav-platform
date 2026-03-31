namespace SIAV.Application.DTOs.Docentes;

public record DocenteDisponibleDto(int Id, string Nombre);

public record CrearDisponibilidadDto(
    DiaSemanaDto DiaSemana,
    TimeOnly HoraInicio,
    TimeOnly HoraFin
);

public record DisponibilidadDto(
    int Id,
    DiaSemanaDto DiaSemana,
    TimeOnly HoraInicio,
    TimeOnly HoraFin
);

public enum DiaSemanaDto
{
    Lunes = 1,
    Martes = 2,
    Miercoles = 3,
    Jueves = 4,
    Viernes = 5,
    Sabado = 6
}