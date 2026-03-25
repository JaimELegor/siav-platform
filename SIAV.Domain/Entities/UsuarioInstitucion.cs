namespace SIAV.Domain.Entities;

public class UsuarioInstitucion
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int InstitucionId { get; set; }
    public RolInstitucion Rol { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

    public Usuario Usuario { get; set; } = null!;
    public Institucion Institucion { get; set; } = null!;
    public ICollection<Inscripcion> Inscripciones { get; set; } = [];
}

public enum RolInstitucion
{
    Coordinador = 1,
    Docente = 2,
    Alumno = 3
}