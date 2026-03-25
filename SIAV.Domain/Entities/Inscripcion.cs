namespace SIAV.Domain.Entities;

public class Inscripcion
{
    public int Id { get; set; }
    public int AlumnoId { get; set; } // FK a UsuarioInstitucion
    public int GrupoId { get; set; }
    public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;
    public EstadoInscripcion Estado { get; set; } = EstadoInscripcion.Activa;

    public UsuarioInstitucion Alumno { get; set; } = null!;
    public Grupo Grupo { get; set; } = null!;
}

public enum EstadoInscripcion
{
    Activa = 1,
    Baja = 2,
    Completada = 3
}