namespace SIAV.Domain.Entities;

public class CursoPorAlumnoResultado
{
    public int GrupoId { get; set; }
    public string GrupoNombre { get; set; } = string.Empty;
    public int CursoId { get; set; }
    public string CursoNombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; } = string.Empty;
}