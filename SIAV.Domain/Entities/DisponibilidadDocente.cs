namespace SIAV.Domain.Entities;

public class DisponibilidadDocente
{
    public int Id { get; set; }
    public int DocenteId { get; set; }          // FK → UsuarioInstitucion.Id
    public DiaSemana DiaSemana { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }

    public UsuarioInstitucion Docente { get; set; } = null!;
}

public enum DiaSemana
{
    Lunes = 1,
    Martes = 2,
    Miercoles = 3,
    Jueves = 4,
    Viernes = 5,
    Sabado = 6
}
