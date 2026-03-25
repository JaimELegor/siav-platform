namespace SIAV.Domain.Entities;

public class Curso
{
    public int Id { get; set; }
    public int InstitucionId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public bool TieneNiveles { get; set; } = false;
    public string? LinkGoogleForm { get; set; }

    public Institucion Institucion { get; set; } = null!;
    public ICollection<Grupo> Grupos { get; set; } = [];
}