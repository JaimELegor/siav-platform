namespace SIAV.Application.DTOs.Auth;

public class RegisterDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int InstitucionId { get; set; }
    public RolInstitucionDto Rol { get; set; }
}

public enum RolInstitucionDto
{
    Coordinador = 1,
    Docente = 2,
    Alumno = 3
}