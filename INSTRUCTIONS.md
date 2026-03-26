# 1. Copiar el archivo al contenedor
docker cp crear_sps.sql sqlserver-siav:/tmp/crear_sps.sql

# 2. Ejecutarlo
docker exec -it sqlserver-siav \
  /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'TuPassword123!' \
  -C -i /tmp/crear_sps.sql

docker exec -it sqlserver-siav \
  /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'TuPassword123!' -C \
  -Q "USE SIAV_DB; SELECT name FROM sys.procedures WHERE name IN ('InscribirAlumno', 'ObtenerCursosPorAlumno');"