namespace API.AptosMedicos.Models;

public class ErrorResponse
{
    //public int StatusCode { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public List<string> Errores { get; set; } = [];
}