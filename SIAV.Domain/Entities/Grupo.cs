namespace SIAV.Domain.Entities;

public class Grupo
{
    public int Id { get; set; }
    public int CursoId { get; set; }
    public int DocenteId { get; set; } // FK a UsuarioInstitucion
    public string Nombre { get; set; } = string.Empty; // ej. "Inglés B1 - Lunes 7pm"
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int LimiteAlumnos { get; set; } = 15;
    public EstadoGrupo Estado { get; set; } = EstadoGrupo.Abierto;

    public Curso Curso { get; set; } = null!;
    public UsuarioInstitucion Docente { get; set; } = null!;
    public ICollection<Inscripcion> Inscripciones { get; set; } = [];
}

public enum EstadoGrupo
{
    Abierto = 1,
    EnCurso = 2,
    Cerrado = 3
}