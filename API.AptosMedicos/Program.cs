var builder = WebApplication.CreateBuilder(args); // Inicializa la aplicación y carga los modulos

// Soporte para ejecutarse como Windows Service
builder.Host.UseWindowsService();

builder.Services.AddControllers(); // Habilita uso del controller
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Se crea una petición por request
builder.Services.AddScoped<API.AptosMedicos.Services.ISolicitudAptoMedicoService,
                           API.AptosMedicos.Services.SolicitudAptoMedicoService>(); // Inyección de depedencias

// Permite que otros sistemas puedan consumir mi API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Cualquier origen
              .AllowAnyMethod() // Cualquier metodo (GET, POST, DELETE, UPDATE)
              .AllowAnyHeader();// Cualquier header
    });
    options.AddPolicy("localDev", policy =>
    {
        policy.AllowAnyOrigin() // Cualquier origen
                  .AllowAnyMethod() // Cualquier metodo (GET, POST, DELETE, UPDATE)
                  .AllowAnyHeader();// Cualquier header
    }
    );
});

// Contruye la aplicación ensamblando todas las configuraciones de arriba 
var app = builder.Build();

// Habilita el uso de Swagger como una interfaz visual
app.UseSwagger();
app.UseSwaggerUI();

// Habilitamos las politicas del CORS para toda las solicitudes
app.UseCors();
// Conecta rutas HTTP con los contrroladores 
app.MapControllers();
// Inicia el servidro
app.Run();