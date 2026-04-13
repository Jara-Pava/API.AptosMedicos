namespace API.AptosMedicos.Models;

public class SolicitudAptoMedicoDto
{
    public int Id_Solicitud { get; set; }
    public int Id_Tipo_Solicitud { get; set; }
    public int Id_Solicitante { get; set; }
    public int Id_proyecto { get; set; }
    public string Proyecto { get; set; } = string.Empty;
    public string? Id_Global { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? No_Identidad { get; set; }
    public DateTime? Fecha_Nacimiento { get; set; }
    public DateTime Fecha_Solicitud { get; set; }
    public int? Id_PersonalContratista { get; set; }
    public string? Puesto { get; set; }
    public int Id_Apto { get; set; }
    public string? Medico { get; set; }
    public string Apto { get; set; } = string.Empty;
    public DateTime? Fecha_Vigencia { get; set; }
    public int? Edad { get; set; }
    public string? Sexo { get; set; }
}