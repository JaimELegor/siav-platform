using Microsoft.EntityFrameworkCore;
using SIAV.Domain.Entities;

namespace SIAV.Infraestructure.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Institucion> Instituciones => Set<Institucion>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<UsuarioInstitucion> UsuariosInstitucion => Set<UsuarioInstitucion>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuario — email único global
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // UsuarioInstitucion — un usuario no puede tener el mismo rol dos veces en la misma institución
        modelBuilder.Entity<UsuarioInstitucion>()
            .HasIndex(ui => new { ui.UsuarioId, ui.InstitucionId, ui.Rol })
            .IsUnique();

        // Grupo → Docente (sin cascade para evitar ciclos)
        modelBuilder.Entity<Grupo>()
            .HasOne(g => g.Docente)
            .WithMany()
            .HasForeignKey(g => g.DocenteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Inscripcion → Alumno (sin cascade)
        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Alumno)
            .WithMany(ui => ui.Inscripciones)
            .HasForeignKey(i => i.AlumnoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Enum como string en BD — más legible en queries directas
        modelBuilder.Entity<UsuarioInstitucion>()
            .Property(ui => ui.Rol)
            .HasConversion<string>();

        modelBuilder.Entity<Grupo>()
            .Property(g => g.Estado)
            .HasConversion<string>();

        modelBuilder.Entity<Inscripcion>()
            .Property(i => i.Estado)
            .HasConversion<string>();

        // Seed — institución demo + usuario admin inicial
        modelBuilder.Entity<Institucion>().HasData(
            new Institucion
            {
                Id = 1,
                Nombre = "Institución Demo",
                Slug = "demo",
            }
        );
    }
}