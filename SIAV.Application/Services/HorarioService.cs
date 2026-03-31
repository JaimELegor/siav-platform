using Microsoft.EntityFrameworkCore;
using SIAV.Application.DTOs.Docentes;
using SIAV.Application.DTOs.Horarios;
using SIAV.Application.Interfaces;
using SIAV.Domain.Entities;

namespace SIAV.Application.Services;

public class HorarioService(IApplicationDbContext db) : IHorarioService
{
    public async Task<HorarioDto> CrearAsync(CrearHorarioDto dto, int institucionId)
    {
        var grupo = await db.Grupos
            .Include(g => g.Curso)
            .Include(g => g.Docente).ThenInclude(d => d.Usuario)
            .FirstOrDefaultAsync(g =>
                g.Id == dto.GrupoId &&
                g.Curso.InstitucionId == institucionId)
            ?? throw new InvalidOperationException("Grupo no encontrado.");

        var dia = (DiaSemana)dto.DiaSemana;

        // Validar que el horario cabe en la disponibilidad declarada del docente
        var dentroDeDisponibilidad = await db.DisponibilidadesDocente
            .AnyAsync(d =>
                d.DocenteId == grupo.DocenteId &&
                d.DiaSemana == dia &&
                d.HoraInicio <= dto.HoraInicio &&
                d.HoraFin >= dto.HoraFin);

        if (!dentroDeDisponibilidad)
            throw new InvalidOperationException(
                "El horario está fuera de la disponibilidad declarada del docente.");

        // Validar conflicto con horarios ya asignados al docente
        var conflicto = await db.Horarios
            .AnyAsync(h =>
                h.Grupo.DocenteId == grupo.DocenteId &&
                h.DiaSemana == dia &&
                h.HoraInicio < dto.HoraFin &&
                h.HoraFin > dto.HoraInicio);

        if (conflicto)
            throw new InvalidOperationException(
                "El docente ya tiene una clase asignada en ese horario.");

        // Validar que el grupo no tenga ya un horario en ese mismo slot
        var grupoOcupado = await db.Horarios
            .AnyAsync(h =>
                h.GrupoId == dto.GrupoId &&
                h.DiaSemana == dia &&
                h.HoraInicio < dto.HoraFin &&
                h.HoraFin > dto.HoraInicio);

        if (grupoOcupado)
            throw new InvalidOperationException(
                "El grupo ya tiene un horario asignado en ese slot.");

        var horario = new Horario
        {
            GrupoId    = dto.GrupoId,
            DiaSemana  = dia,
            HoraInicio = dto.HoraInicio,
            HoraFin    = dto.HoraFin
        };

        db.Horarios.Add(horario);
        await db.SaveChangesAsync();

        return MapDto(horario, grupo);
    }

    public async Task<List<HorarioDto>> ObtenerPorInstitucionAsync(int institucionId) =>
        await db.Horarios
            .Include(h => h.Grupo).ThenInclude(g => g.Curso)
            .Include(h => h.Grupo).ThenInclude(g => g.Docente).ThenInclude(d => d.Usuario)
            .Where(h => h.Grupo.Curso.InstitucionId == institucionId)
            .Select(h => new HorarioDto(
                h.Id,
                h.GrupoId,
                h.Grupo.Nombre,
                h.Grupo.CursoId,
                h.Grupo.Curso.Nombre,
                h.Grupo.DocenteId,
                h.Grupo.Docente.Usuario.Nombre,
                h.DiaSemana.ToString(),
                h.HoraInicio,
                h.HoraFin))
            .ToListAsync();

    public async Task<List<HorarioDto>> ObtenerPorDocenteAsync(int docenteId, int institucionId) =>
        await db.Horarios
            .Include(h => h.Grupo).ThenInclude(g => g.Curso)
            .Include(h => h.Grupo).ThenInclude(g => g.Docente).ThenInclude(d => d.Usuario)
            .Where(h =>
                h.Grupo.DocenteId == docenteId &&
                h.Grupo.Curso.InstitucionId == institucionId)
            .Select(h => new HorarioDto(
                h.Id,
                h.GrupoId,
                h.Grupo.Nombre,
                h.Grupo.CursoId,
                h.Grupo.Curso.Nombre,
                h.Grupo.DocenteId,
                h.Grupo.Docente.Usuario.Nombre,
                h.DiaSemana.ToString(),
                h.HoraInicio,
                h.HoraFin))
            .ToListAsync();

    public async Task<List<HorarioDto>> ObtenerPorAlumnoAsync(int alumnoId, int institucionId) =>
        await db.Horarios
            .Include(h => h.Grupo).ThenInclude(g => g.Curso)
            .Include(h => h.Grupo).ThenInclude(g => g.Docente).ThenInclude(d => d.Usuario)
            .Where(h =>
                h.Grupo.Inscripciones.Any(i =>
                    i.AlumnoId == alumnoId &&
                    i.Estado == EstadoInscripcion.Activa) &&
                h.Grupo.Curso.InstitucionId == institucionId)
            .Select(h => new HorarioDto(
                h.Id,
                h.GrupoId,
                h.Grupo.Nombre,
                h.Grupo.CursoId,
                h.Grupo.Curso.Nombre,
                h.Grupo.DocenteId,
                h.Grupo.Docente.Usuario.Nombre,
                h.DiaSemana.ToString(),
                h.HoraInicio,
                h.HoraFin))
            .ToListAsync();

    public async Task<bool> EliminarAsync(int id, int institucionId)
    {
        var horario = await db.Horarios
            .FirstOrDefaultAsync(h =>
                h.Id == id &&
                h.Grupo.Curso.InstitucionId == institucionId);

        if (horario is null) return false;

        db.Horarios.Remove(horario);
        await db.SaveChangesAsync();
        return true;
    }

    private static HorarioDto MapDto(Horario h, Grupo g) => new(
        h.Id,
        h.GrupoId,
        g.Nombre,
        g.CursoId,
        g.Curso.Nombre,
        g.DocenteId,
        g.Docente.Usuario.Nombre,
        h.DiaSemana.ToString(),
        h.HoraInicio,
        h.HoraFin);
}