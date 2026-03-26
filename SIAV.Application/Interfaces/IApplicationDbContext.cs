using Microsoft.EntityFrameworkCore;
using SIAV.Domain.Entities;

namespace SIAV.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Usuario> Usuarios { get; }
    DbSet<Institucion> Instituciones { get; }
    DbSet<UsuarioInstitucion> UsuariosInstitucion { get; }
    DbSet<Curso> Cursos { get; }
    DbSet<Grupo> Grupos { get; }
    DbSet<Inscripcion> Inscripciones { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}