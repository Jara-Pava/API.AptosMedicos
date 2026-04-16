# API.AptosMedicos



ESTRUCTURA ESCANFOLDER (CASCADING FOLDER)



API.AptosMedicos/

├── Controllers/

│   └── SolicitudesAptosMedicosController.cs

├── Models/

│   └── SolicitudAptoMedicoDto.cs

├── Services/

│   ├── ISolicitudAptoMedicoService.cs

│   └── SolicitudAptoMedicoService.cs

├── Data/

│   └── AptosMedicosDbContext.cs

├── Program.cs

├── appsettings.json

└── API.AptosMedicos.csproj



1. Se instala el nuget Microsoft.EntityFrameworkCore.SqlServer dando clic en el proyecto API y Nugget, y en browser pegar el nombre, e instalar.



Necesito crear este API para poder consultar y que me devuelva las solicitudes de aptos médicos, este devolverá la respuesta en formato JSON: la estructura

de la respuesta sería {totalSolicitudes, data: \[]}, no se otorgará permisos, es publico el API.

SELECT s.\[id\_solicitud], s.\[id\_tipo\_solicitud], s.\[id\_solicitante], s.\[id\_proyecto], p.\[proyecto], s.\[id\_global], s.\[Nombre] AS nombre, s.\[Apellidos] AS apellidos, s.\[no\_identidad], s.\[fecha\_nacimiento], s.\[fecha\_solicitud], s.\[id\_personalcontratista], s.\[puesto], s.\[apto\_medico] AS id\_apto, mu.\[nombre] + ' ' + mu.\[apellidos] AS medico, a.\[Apto] AS apto, s.\[fecha\_vigencia], s.\[Edad] AS edad, s.\[Sexo] AS sexo FROM dbo.EM\_Solicitudes s INNER JOIN EM\_Proyectos p ON p.id\_proyecto = s.id\_proyecto INNER JOIN DM\_Pais pa ON pa.id\_pais = p.id\_pais INNER JOIN EM\_Apto a ON a.IDApto = s.apto\_medico LEFT JOIN EM\_Medicos m ON m.id\_medico = s.id\_medico LEFT JOIN SEC\_Usuarios mu ON mu.id\_usuario = m.id\_medico WHERE pa.pais\_ISO IN ('CHI', 'ARG') AND a.Apto IN ('A', 'B', 'C', 'D') AND s.id\_solicitud = 115654 ORDER BY fecha\_solicitud Esto es lo que obtengo de un apto medico

