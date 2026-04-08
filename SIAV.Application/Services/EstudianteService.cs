using Microsoft.EntityFrameworkCore;
using SIAV.Application.DTOs.Inscripciones;
using SIAV.Application.Interfaces;
using SIAV.Domain.Entities;

namespace SIAV.Application.Services;

public class EstudianteService(IApplicationDbContext db) : IEstudianteService
{
    public async Task<List<InscripcionDto>> ObtenerPorGrupoAsync(
        int grupoId, int institucionId)
    {
        return await db.Inscripciones
            .Where(i =>
                i.GrupoId == grupoId &&
                i.Grupo.Curso.InstitucionId == institucionId)
            .Select(i => new InscripcionDto(
                i.Id,
                i.Alumno.Id,
                i.Alumno.Usuario.Nombre,
                i.Alumno.Usuario.Email,
                i.GrupoId,
                i.Grupo.Nombre,
                i.FechaInscripcion,
                (EstadoInscripcionDto)(int)i.Estado))
            .OrderBy(i => i.NombreAlumno)
            .ToListAsync();
    }

    public async Task<List<InscripcionDto>> ObtenerPorCursoAsync(
        int cursoId, int institucionId)
    {
        return await db.Inscripciones
            .Where(i =>
                i.Grupo.CursoId == cursoId &&
                i.Grupo.Curso.InstitucionId == institucionId)
            .Select(i => new InscripcionDto(
                i.Id,
                i.Alumno.Id,
                i.Alumno.Usuario.Nombre,
                i.Alumno.Usuario.Email,
                i.GrupoId,
                i.Grupo.Nombre,
                i.FechaInscripcion,
                (EstadoInscripcionDto)(int)i.Estado))
            .OrderBy(i => i.NombreAlumno)
            .ToListAsync();
    }

    public async Task<List<InscripcionDto>> ObtenerPorInstitucionAsync(
        int institucionId, EstadoInscripcionDto? filtroEstado = null)
    {
        var alumnos = await db.UsuariosInstitucion
            .Where(ui =>
                ui.InstitucionId == institucionId &&
                ui.Rol == RolInstitucion.Alumno &&
                ui.Activo)
            .Include(ui => ui.Usuario)
            .Include(ui => ui.Inscripciones)
                .ThenInclude(i => i.Grupo)
            .ToListAsync();

        var resultado = alumnos.Select(ui =>
        {
            var inscripcion = ui.Inscripciones
                .OrderBy(i => (int)i.Estado)
                .FirstOrDefault();

            return new InscripcionDto(
                inscripcion?.Id ?? 0,
                ui.Id,
                ui.Usuario.Nombre,
                ui.Usuario.Email,
                inscripcion?.GrupoId,
                inscripcion?.Grupo?.Nombre,
                inscripcion?.FechaInscripcion,
                inscripcion is null
                ? EstadoInscripcionDto.SinInscripcion
                : (EstadoInscripcionDto)(int)inscripcion.Estado
            );
        });

        if (filtroEstado.HasValue)
            resultado = resultado
                .Where(a => a.Estado == filtroEstado.Value);

        return resultado
            .OrderBy(a => a.NombreAlumno)
            .ToList();
    }
}