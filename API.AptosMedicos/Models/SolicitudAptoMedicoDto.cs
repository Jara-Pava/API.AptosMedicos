namespace API.AptosMedicos.Models;

public class SolicitudAptoMedicoDto
{
    public int Id_Solicitud { get; set; }
    public int Id_Tipo_Solicitud { get; set; }
    public int Id_Solicitante { get; set; }
    public int Id_Proyecto { get; set; }
    public int Id_Status { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Nombre_Solicitante { get; set; } = string.Empty;
    public string Proyecto { get; set; } = string.Empty;
    public string? Id_Global { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? No_Identidad { get; set; }
    public DateTime? Fecha_Nacimiento { get; set; }
    public DateTime Fecha_Solicitud { get; set; }
    public int? Id_PersonalContratista { get; set; }
    public int? Id_Subcontratista { get; set; }
    public string nombre_contratista { get; set; } = string.Empty;
    public string? Puesto { get; set; }
    public int? Id_Medico { get; set; }
    public string? Medico_Asigna_Examen { get; set; }
    public int Id_Medico_apto { get; set; }
    public string ? Medico_Apto { get; set; }
    public int Id_Tipo_Examen { get; set; }
    public string Nombre_Tipo_Examen { get; set; } = string.Empty;
    public int Id_Apto { get; set; }
    public string Apto { get; set; } = string.Empty;
    public DateTime? Fecha_Diagnostico { get; set; }
    public DateTime? Fecha_Vigencia { get; set; }
    public int? Edad { get; set; }
    public string? Sexo { get; set; }
}