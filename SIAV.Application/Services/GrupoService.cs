using Microsoft.EntityFrameworkCore;
using SIAV.Application.DTOs;
using SIAV.Application.Interfaces;
using SIAV.Domain.Entities;

namespace SIAV.Application.Services;

public class GrupoService(IApplicationDbContext db) : IGrupoService
{
    public async Task<List<GrupoDto>> ObtenerPorInstitucionAsync(int institucionId) =>
        await db.Grupos
            .Where(g => g.Curso.InstitucionId == institucionId)
            .Select(g => new GrupoDto(
                g.Id, g.CursoId, g.Curso.Nombre,
                g.DocenteId, g.Docente.Usuario.Nombre,
                g.Nombre, g.FechaInicio, g.FechaFin, g.LimiteAlumnos,
                g.Inscripciones.Count(i => i.Estado == EstadoInscripcion.Activa),
                g.Estado.ToString()))
            .ToListAsync();

    public async Task<List<GrupoDto>> ObtenerPorCursoAsync(int cursoId, int institucionId) =>
        await db.Grupos
            .Where(g => g.CursoId == cursoId && g.Curso.InstitucionId == institucionId)
            .Select(g => new GrupoDto(
                g.Id, g.CursoId, g.Curso.Nombre,
                g.DocenteId, g.Docente.Usuario.Nombre,
                g.Nombre, g.FechaInicio, g.FechaFin, g.LimiteAlumnos,
                g.Inscripciones.Count(i => i.Estado == EstadoInscripcion.Activa),
                g.Estado.ToString()))
            .ToListAsync();

    public async Task<GrupoDto> CrearAsync(CrearGrupoDto dto, int institucionId)
    {
        var grupo = new Grupo
        {
            CursoId     = dto.CursoId,
            DocenteId   = dto.DocenteId,
            Nombre      = dto.Nombre,
            FechaInicio = dto.FechaInicio,
            FechaFin    = dto.FechaFin,
            LimiteAlumnos = dto.LimiteAlumnos
        };
        db.Grupos.Add(grupo);
        await db.SaveChangesAsync();

        // Recargar con navegaciones para el DTO
        var g = await db.Grupos
            .Include(x => x.Curso)
            .Include(x => x.Docente).ThenInclude(x => x.Usuario)
            .FirstAsync(x => x.Id == grupo.Id);

        return new GrupoDto(g.Id, g.CursoId, g.Curso.Nombre,
            g.DocenteId, g.Docente.Usuario.Nombre,
            g.Nombre, g.FechaInicio, g.FechaFin, g.LimiteAlumnos, 0,
            g.Estado.ToString());
    }

    public async Task<bool> EliminarAsync(int id, int institucionId)
    {
        var grupo = await db.Grupos
            .FirstOrDefaultAsync(g => g.Id == id && g.Curso.InstitucionId == institucionId);
        if (grupo is null) return false;
        db.Grupos.Remove(grupo);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task InscribirAlumnoAsync(int alumnoMembresiaId, int grupoId)
    {
        await db.Database.ExecuteSqlRawAsync(
            "EXEC InscribirAlumno {0}, {1}",
            alumnoMembresiaId, grupoId);
    }

    public async Task<List<CursoPorAlumnoResultado>> ObtenerCursosPorAlumnoAsync(int alumnoMembresiaId)
    {
        return await db.CursosPorAlumno
            .FromSqlRaw("EXEC ObtenerCursosPorAlumno {0}", alumnoMembresiaId)
            .ToListAsync();
    }

    public async Task ReasignarDocenteAsync(int grupoId, int nuevoDocenteId, int institucionId)
    {
        var grupo = await db.Grupos
            .FirstOrDefaultAsync(g =>
                g.Id == grupoId &&
                g.Curso.InstitucionId == institucionId)
            ?? throw new InvalidOperationException("Grupo no encontrado.");

        var docenteExiste = await db.UsuariosInstitucion
            .AnyAsync(ui =>
                ui.Id == nuevoDocenteId &&
                ui.InstitucionId == institucionId &&
                ui.Rol == RolInstitucion.Docente &&
                ui.Activo);

        if (!docenteExiste)
            throw new InvalidOperationException("Docente no encontrado en esta institución.");

        grupo.DocenteId = nuevoDocenteId;
        await db.SaveChangesAsync();
    }
}