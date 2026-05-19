namespace API.AptosMedicos.Models;

public class ErrorResponse
{
    //public string Mensaje { get; set; } = string.Empty;
    public List<string> Errores { get; set; } = [];
}

public class NotFoundResponse
{
    public string Mensaje { get; set; } = string.Empty;
}