namespace SIAV.Domain.Entities;

public class Institucion
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty; // ej. "escuela-ingles-leon"
    public string? LogoUrl { get; set; }
    public string? ConfigJson { get; set; } // preferencias específicas de la escuela

    public ICollection<UsuarioInstitucion> Miembros { get; set; } = [];
    public ICollection<Curso> Cursos { get; set; } = [];
}