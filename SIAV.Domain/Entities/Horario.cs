namespace SIAV.Domain.Entities;

public class Horario
{
    public int Id { get; set; }
    public int GrupoId { get; set; }
    public DiaSemana DiaSemana { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }

    public Grupo Grupo { get; set; } = null!;
}