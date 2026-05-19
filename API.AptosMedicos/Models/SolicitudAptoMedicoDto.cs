using System.Text.Json.Serialization;
using API.AptosMedicos.Converters;

namespace API.AptosMedicos.Models;

public class SolicitudAptoMedicoDto
{
    public int Id_Solicitud { get; set; }
    public string Nombre_Solicitante { get; set; } = string.Empty;
    public string Proyecto { get; set; } = string.Empty;
    public string? Id_Global { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? No_Identidad { get; set; }
    public DateTime? Fecha_Nacimiento { get; set; }
    public DateTime Fecha_Solicitud { get; set; }
    public string nombre_contratista { get; set; } = string.Empty;
    public string? Puesto { get; set; }
    public string? Medico_Asigna_Examen { get; set; }
    public string ? Medico_Apto { get; set; }
    public string Nombre_Tipo_Examen { get; set; } = string.Empty;
    public string Apto { get; set; } = string.Empty;
    public DateTime? Fecha_Diagnostico { get; set; }
    [JsonConverter(typeof(DateOnlyConverter))]
    public DateTime? Fecha_Vigencia { get; set; }
    public int? Edad { get; set; }
    public string? Sexo { get; set; }
}