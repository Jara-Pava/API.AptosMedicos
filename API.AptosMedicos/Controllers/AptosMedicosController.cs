using API.AptosMedicos.Models;
using API.AptosMedicos.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.AptosMedicos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AptosMedicosController : ControllerBase
{
    private readonly ISolicitudAptoMedicoService _service;

    public AptosMedicosController(ISolicitudAptoMedicoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<AptosMedicosResponse>> GetAll()
    {
        var solicitudes = await _service.GetAllAsync();
        var lista = solicitudes.ToList();

        return Ok(new AptosMedicosResponse
        {
            TotalSolicitudes = lista.Count,
            Solicitudes = lista
        });
    }

    [HttpGet("{idSolicitud:int}")]
    public async Task<ActionResult<AptosMedicosResponse>> GetById(int idSolicitud)
    {
        var solicitud = await _service.GetByIdAsync(idSolicitud);

        if (solicitud is null)
            return NotFound(new AptosMedicosResponse { TotalSolicitudes = 0, Solicitudes = [] });

        return Ok(new AptosMedicosResponse
        {
            TotalSolicitudes = 1,
            Solicitudes = [solicitud]
        });
    }
}