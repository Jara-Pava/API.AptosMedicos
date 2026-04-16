using API.AptosMedicos.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.AptosMedicos.Services;

public interface ISolicitudAptoMedicoService
{
    Task<IEnumerable<SolicitudAptoMedicoDto>> GetAllAsync();
    Task<SolicitudAptoMedicoDto?> GetByIdAsync(int idSolicitud);
    Task<IEnumerable<SolicitudAptoMedicoDto>> GetByIdGlobalAsync(string idGlobal);
    Task<IEnumerable<SolicitudAptoMedicoDto>>GetAllByFechaSolicitudAsync(DateTime fecha);
    Task<IEnumerable<SolicitudAptoMedicoDto>> GetAllByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin);
}

public class SolicitudAptoMedicoService : ISolicitudAptoMedicoService
{
    private readonly string _connectionString;

    public SolicitudAptoMedicoService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AptosMedicosDb")
            ?? throw new InvalidOperationException("Connection string 'AptosMedicosDb' not found.");
    }

    public async Task<IEnumerable<SolicitudAptoMedicoDto>> GetAllAsync()
    {
        using IDbConnection db = new SqlConnection(_connectionString);

        return await db.QueryAsync<SolicitudAptoMedicoDto>(
            "EM_GetAllSolicitudesAptosMedicos",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<SolicitudAptoMedicoDto>> GetAllByFechaSolicitudAsync(DateTime fecha)
    {
        string fechaNueva = fecha.ToString("yyyy-MM-dd");
        using IDbConnection db = new SqlConnection(_connectionString);

        return await db.QueryAsync<SolicitudAptoMedicoDto>(
            "EM_GetSolicitudesAptosByFechaSolicitud",
            new { fecha_solicitud = fechaNueva },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<SolicitudAptoMedicoDto>> GetAllByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        using IDbConnection db = new SqlConnection(_connectionString);

        return await db.QueryAsync<SolicitudAptoMedicoDto>(
            "EM_GetSolicitudesAptosByFechas",
            new
            {
                fecha_inicio = fechaInicio.ToString("yyyy-MM-dd"),
                fecha_fin = fechaFin.ToString("yyyy-MM-dd")
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<SolicitudAptoMedicoDto?> GetByIdAsync(int idSolicitud)
    {
        using IDbConnection db = new SqlConnection(_connectionString);

        return await db.QueryFirstOrDefaultAsync<SolicitudAptoMedicoDto>(
            "EM_GetSolicitudAptoById",
            new { id_solicitud = idSolicitud },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<SolicitudAptoMedicoDto>> GetByIdGlobalAsync(string idGlobal)
    {
        using IDbConnection db = new SqlConnection(_connectionString);

        return await db.QueryAsync<SolicitudAptoMedicoDto>(
            "EM_GetSolicitudesAptoByIdGlobal",
            new { id_global = idGlobal },
            commandType: CommandType.StoredProcedure);
    }
}