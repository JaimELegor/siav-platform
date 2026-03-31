using Microsoft.EntityFrameworkCore;
using SIAV.Application.DTOs.Docentes;
using SIAV.Application.Interfaces;
using SIAV.Domain.Entities;

namespace SIAV.Application.Services;

public class DocenteService(IApplicationDbContext db) : IDocenteService
{
    public async Task<DisponibilidadDto> AgregarDisponibilidadAsync(
        int docenteId, int institucionId, CrearDisponibilidadDto dto)
    {
        // Verificar que el docente pertenece a la institución
        var docente = await db.UsuariosInstitucion
            .FirstOrDefaultAsync(ui =>
                ui.Id == docenteId &&
                ui.InstitucionId == institucionId &&
                ui.Rol == RolInstitucion.Docente)
            ?? throw new InvalidOperationException("Docente no encontrado en esta institución.");

        // Verificar que no haya solapamiento con disponibilidad existente
        var solapado = await db.DisponibilidadesDocente
            .AnyAsync(d =>
                d.DocenteId == docenteId &&
                d.DiaSemana == (DiaSemana)dto.DiaSemana &&
                d.HoraInicio < dto.HoraFin &&
                d.HoraFin > dto.HoraInicio);

        if (solapado)
            throw new InvalidOperationException("Ya existe una disponibilidad que se solapa en ese horario.");

        var disponibilidad = new DisponibilidadDocente
        {
            DocenteId  = docenteId,
            DiaSemana  = (DiaSemana)dto.DiaSemana,
            HoraInicio = dto.HoraInicio,
            HoraFin    = dto.HoraFin
        };

        db.DisponibilidadesDocente.Add(disponibilidad);
        await db.SaveChangesAsync();

        return new DisponibilidadDto(
            disponibilidad.Id,
            dto.DiaSemana,
            dto.HoraInicio,
            dto.HoraFin);
    }

    public async Task<List<DisponibilidadDto>> ObtenerDisponibilidadAsync(
        int docenteId, int institucionId)
    {
        return await db.DisponibilidadesDocente
            .Where(d =>
                d.DocenteId == docenteId &&
                d.Docente.InstitucionId == institucionId)
            .Select(d => new DisponibilidadDto(
                d.Id,
                (DiaSemanaDto)d.DiaSemana,
                d.HoraInicio,
                d.HoraFin))
            .OrderBy(d => d.DiaSemana)
            .ThenBy(d => d.HoraInicio)
            .ToListAsync();
    }

    public async Task<bool> EliminarDisponibilidadAsync(int disponibilidadId, int institucionId)
    {
        var disponibilidad = await db.DisponibilidadesDocente
            .FirstOrDefaultAsync(d =>
                d.Id == disponibilidadId &&
                d.Docente.InstitucionId == institucionId);

        if (disponibilidad is null) return false;

        db.DisponibilidadesDocente.Remove(disponibilidad);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<DocenteDisponibleDto>> ObtenerDisponiblesAsync(
        int institucionId, DiaSemana dia, TimeOnly horaInicio, TimeOnly horaFin)
    {
        // Docentes cuya disponibilidad declarada cubre el rango completo
        var conDisponibilidad = await db.DisponibilidadesDocente
            .Where(d =>
                d.Docente.InstitucionId == institucionId &&
                d.DiaSemana == dia &&
                d.HoraInicio <= horaInicio &&
                d.HoraFin >= horaFin)
            .Select(d => d.DocenteId)
            .Distinct()
            .ToListAsync();

        // De esos, excluir los que ya tienen clase asignada en ese rango
        var ocupados = await db.Horarios
            .Where(h =>
                h.DiaSemana == dia &&
                h.HoraInicio < horaFin &&
                h.HoraFin > horaInicio)
            .Select(h => h.Grupo.DocenteId)
            .Distinct()
            .ToListAsync();

        return await db.UsuariosInstitucion
            .Include(ui => ui.Usuario)
            .Where(ui =>
                conDisponibilidad.Contains(ui.Id) &&
                !ocupados.Contains(ui.Id))
            .Select(ui => new DocenteDisponibleDto(ui.Id, ui.Usuario.Nombre))
            .ToListAsync();
    }
}