USE SIAV_DB;
GO

-- SP #1: Inscribir alumno (valida duplicados y límite)
CREATE OR ALTER PROCEDURE InscribirAlumno
    @AlumnoId INT,
    @GrupoId  INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 FROM Inscripciones
        WHERE AlumnoId = @AlumnoId AND GrupoId = @GrupoId
          AND Estado != 'Baja'
    )
    BEGIN
        RAISERROR('El alumno ya está inscrito en este grupo.', 16, 1);
        RETURN;
    END

    DECLARE @Inscritos INT = (
        SELECT COUNT(*) FROM Inscripciones
        WHERE GrupoId = @GrupoId AND Estado = 'Activa'
    );
    DECLARE @Limite INT = (
        SELECT LimiteAlumnos FROM Grupos WHERE Id = @GrupoId
    );

    IF @Inscritos >= @Limite
    BEGIN
        RAISERROR('El grupo ha alcanzado su límite de alumnos.', 16, 1);
        RETURN;
    END

    INSERT INTO Inscripciones (AlumnoId, GrupoId, FechaInscripcion, Estado)
    VALUES (@AlumnoId, @GrupoId, GETUTCDATE(), 'Activa');
END;
GO

-- SP #2: Obtener cursos activos de un alumno
CREATE OR ALTER PROCEDURE ObtenerCursosPorAlumno
    @AlumnoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        g.Id          AS GrupoId,
        g.Nombre      AS GrupoNombre,
        c.Id          AS CursoId,
        c.Nombre      AS CursoNombre,
        c.Categoria,
        g.FechaInicio,
        g.FechaFin,
        g.Estado
    FROM Inscripciones i
    INNER JOIN Grupos g ON g.Id = i.GrupoId
    INNER JOIN Cursos c ON c.Id = g.CursoId
    WHERE i.AlumnoId = @AlumnoId
      AND i.Estado = 'Activa';
END;
GO