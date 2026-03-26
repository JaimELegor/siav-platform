using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SIAV.Application.DTOs.Auth;
using SIAV.Application.Interfaces;
using SIAV.Domain.Entities;

namespace SIAV.Application.Services;

public class AuthService(IApplicationDbContext context, IConfiguration config) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        // Verificar que la institución existe
        var institucion = await context.Instituciones.FindAsync(dto.InstitucionId)
            ?? throw new InvalidOperationException("Institución no encontrada.");

        // Verificar email único global
        var emailExiste = await context.Usuarios.AnyAsync(u => u.Email == dto.Email);
        if (emailExiste)
            throw new InvalidOperationException("El email ya está registrado.");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Telefono = dto.Telefono
        };

        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();

        var membresía = new UsuarioInstitucion
        {
            UsuarioId = usuario.Id,
            InstitucionId = dto.InstitucionId,
            Rol = (RolInstitucion)dto.Rol
        };

        context.UsuariosInstitucion.Add(membresía);
        await context.SaveChangesAsync();

        return GenerarToken(usuario, membresía);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var usuario = await context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email)
            ?? throw new InvalidOperationException("Credenciales inválidas.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            throw new InvalidOperationException("Credenciales inválidas.");

        var membresia = await context.UsuariosInstitucion
            .FirstOrDefaultAsync(ui =>
                ui.UsuarioId == usuario.Id &&
                ui.InstitucionId == dto.InstitucionId &&
                ui.Activo)
            ?? throw new InvalidOperationException("El usuario no pertenece a esta institución.");

        return GenerarToken(usuario, membresia);
    }

    private AuthResponseDto GenerarToken(Usuario usuario, UsuarioInstitucion membresia)
    {
        var jwtSettings = config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

        var expiracion = DateTime.UtcNow.AddMinutes(
            double.Parse(jwtSettings["ExpirationMinutes"]!));

        var claims = new[]
        {
            new Claim("id", usuario.Id.ToString()),
            new Claim("email", usuario.Email),
            new Claim("nombre", usuario.Nombre),
            new Claim("rol", membresia.Rol.ToString()),
            new Claim("institucionId", membresia.InstitucionId.ToString()),
            new Claim("membresiaId", membresia.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expiracion,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = membresia.Rol.ToString(),
            InstitucionId = membresia.InstitucionId,
            Expiracion = expiracion
        };
    }
}
