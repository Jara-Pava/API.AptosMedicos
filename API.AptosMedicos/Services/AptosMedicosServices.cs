using API.AptosMedicos.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.AptosMedicos.Services;

public interface ISolicitudAptoMedicoService
{
    Task<IEnumerable<SolicitudAptoMedicoDto>> GetAllAsync();
    Task<SolicitudAptoMedicoDto?> GetByIdAsync(int idSolicitud);
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
            "JPJ_GetAllSolicitudesAptosMedicos",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<SolicitudAptoMedicoDto?> GetByIdAsync(int idSolicitud)
    {
        using IDbConnection db = new SqlConnection(_connectionString);

        return await db.QueryFirstOrDefaultAsync<SolicitudAptoMedicoDto>(
            "JPJ_GetSolicitudAptoById",
            new { id_solicitud = idSolicitud },
            commandType: CommandType.StoredProcedure);
    }
}