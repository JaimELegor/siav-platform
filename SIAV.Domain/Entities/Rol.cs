namespace SIAV.Domain.Entities;

public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty; // Admin, Maestro, Alumno

    public ICollection<Usuario> Usuarios { get; set; } = [];
}
