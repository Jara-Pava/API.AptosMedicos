namespace API.AptosMedicos.Models
{
    public class AptosMedicosResponse
    {
        public int TotalSolicitudes { get; set; }
        public IEnumerable<SolicitudAptoMedicoDto> Solicitudes { get; set; } = Enumerable.Empty<SolicitudAptoMedicoDto>();
    }
}
