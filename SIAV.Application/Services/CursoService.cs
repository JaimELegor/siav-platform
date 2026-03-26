using Microsoft.EntityFrameworkCore;
using SIAV.Application.DTOs;
using SIAV.Application.Interfaces;
using SIAV.Domain.Entities;

namespace SIAV.Application.Services;

public class CursoService(IApplicationDbContext db) : ICursoService
{
    public async Task<List<CursoDto>> ObtenerTodosAsync(int institucionId) =>
        await db.Cursos
            .Where(c => c.InstitucionId == institucionId)
            .Select(c => new CursoDto(c.Id, c.Nombre, c.Descripcion, c.Categoria, c.TieneNiveles, c.LinkGoogleForm))
            .ToListAsync();

    public async Task<CursoDto?> ObtenerPorIdAsync(int id, int institucionId)
    {
        var c = await db.Cursos.FirstOrDefaultAsync(c => c.Id == id && c.InstitucionId == institucionId);
        return c is null ? null : new CursoDto(c.Id, c.Nombre, c.Descripcion, c.Categoria, c.TieneNiveles, c.LinkGoogleForm);
    }

    public async Task<CursoDto> CrearAsync(CrearCursoDto dto, int institucionId)
    {
        var curso = new Curso
        {
            InstitucionId = institucionId,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Categoria = dto.Categoria,
            TieneNiveles = dto.TieneNiveles,
            LinkGoogleForm = dto.LinkGoogleForm
        };
        db.Cursos.Add(curso);
        await db.SaveChangesAsync();
        return new CursoDto(curso.Id, curso.Nombre, curso.Descripcion, curso.Categoria, curso.TieneNiveles, curso.LinkGoogleForm);
    }

    public async Task<CursoDto?> ActualizarAsync(int id, ActualizarCursoDto dto, int institucionId)
    {
        var curso = await db.Cursos.FirstOrDefaultAsync(c => c.Id == id && c.InstitucionId == institucionId);
        if (curso is null) return null;

        curso.Nombre = dto.Nombre;
        curso.Descripcion = dto.Descripcion;
        curso.Categoria = dto.Categoria;
        curso.TieneNiveles = dto.TieneNiveles;
        curso.LinkGoogleForm = dto.LinkGoogleForm;

        await db.SaveChangesAsync();
        return new CursoDto(curso.Id, curso.Nombre, curso.Descripcion, curso.Categoria, curso.TieneNiveles, curso.LinkGoogleForm);
    }

    public async Task<bool> EliminarAsync(int id, int institucionId)
    {
        var curso = await db.Cursos.FirstOrDefaultAsync(c => c.Id == id && c.InstitucionId == institucionId);
        if (curso is null) return false;
        db.Cursos.Remove(curso);
        await db.SaveChangesAsync();
        return true;
    }
}