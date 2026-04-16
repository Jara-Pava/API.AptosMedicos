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

    [HttpGet("{idSolicitud}")]
    public async Task<IActionResult> GetById(string idSolicitud)
    {
        if (string.IsNullOrWhiteSpace(idSolicitud))
        {
            return BadRequest(new ErrorResponse
            {
                Mensaje = "Faltan parámetros requeridos.",
                Errores = ["El parámetro 'idSolicitud' es obligatorio."]
            });
        }

        if (!int.TryParse(idSolicitud, out var id) || id < 0)
        {
            return BadRequest(new ErrorResponse
            {
                Mensaje = "El tipo de parámetro no corresponde a un Entero.",
                Errores = [$"El parámetro 'idSolicitud' debe ser un número entero positivo. Valor recibido: '{idSolicitud}'."]
            });
        }

        var solicitud = await _service.GetByIdAsync(id);

        if (solicitud is null)
            return NotFound(new AptosMedicosResponse { TotalSolicitudes = 0, Solicitudes = [] });

        return Ok(new AptosMedicosResponse
        {
            TotalSolicitudes = 1,
            Solicitudes = [solicitud]
        });
    }

    [HttpGet("global/{idGlobal}")]
    public async Task<IActionResult> GetByIdGlobal(string idGlobal)
    {
        if (string.IsNullOrWhiteSpace(idGlobal))
        {
            return BadRequest(new ErrorResponse
            {
                Mensaje = "Faltan parámetros requeridos.",
                Errores = ["El parámetro 'idGlobal' es obligatorio."]
            });
        }

        var solicitudes = await _service.GetByIdGlobalAsync(idGlobal);
        var lista = solicitudes.ToList();

        if (lista.Count == 0)
            return NotFound(new AptosMedicosResponse { TotalSolicitudes = 0, Solicitudes = [] });

        return Ok(new AptosMedicosResponse
        {
            TotalSolicitudes = lista.Count,
            Solicitudes = lista
        });
    }

    [HttpGet("fecha/{fechaSolicitud}")]
    public async Task<IActionResult> GetByFechaSolicitud(string fechaSolicitud)
    {
        if (string.IsNullOrWhiteSpace(fechaSolicitud))
        {
            return BadRequest(new ErrorResponse
            {

                Mensaje = "Faltan parámetros requeridos.",
                Errores = ["El parámetro 'fechaSolicitud' es obligatorio."]
            });
        }
        if (!DateTime.TryParseExact(fechaSolicitud, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var fecha))
        {
            return BadRequest(new ErrorResponse
            {
                Mensaje = "El tipo de parámetro no corresponde a una Fecha.",
                Errores = [$"El parámetro 'fechaSolicitud' debe ser una fecha válida con formato yyyy-MM-dd. Valor recibido: '{fechaSolicitud}'."]
            });
        }
        var solicitudes = await _service.GetAllByFechaSolicitudAsync(fecha);
        var lista = solicitudes.ToList();
        if (lista.Count == 0)
            return NotFound(new AptosMedicosResponse { TotalSolicitudes = 0, Solicitudes = [] });
        return Ok(new AptosMedicosResponse
        {
            TotalSolicitudes = lista.Count,
            Solicitudes = lista
        });
    }

    [HttpGet("fecha/{fechaInicio}/{fechaFin}")]
    public async Task<IActionResult> GetByRangoFechas(string fechaInicio, string fechaFin)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(fechaInicio))
            errores.Add("El parámetro 'fechaInicio' es obligatorio.");
        if (string.IsNullOrWhiteSpace(fechaFin))
            errores.Add("El parámetro 'fechaFin' es obligatorio.");

        if (errores.Count > 0)
            return BadRequest(new ErrorResponse
            {
                Mensaje = "Faltan parámetros requeridos.",
                Errores = errores
            });

        bool inicioValido = DateTime.TryParseExact(fechaInicio, "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var inicio);

        bool finValido = DateTime.TryParseExact(fechaFin, "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var fin);

        if (!inicioValido)
            errores.Add($"El parámetro 'fechaInicio' debe tener formato yyyy-MM-dd. Valor recibido: '{fechaInicio}'.");
        if (!finValido)
            errores.Add($"El parámetro 'fechaFin' debe tener formato yyyy-MM-dd. Valor recibido: '{fechaFin}'.");

        if (errores.Count > 0)
            return BadRequest(new ErrorResponse
            {
                Mensaje = "El tipo de parámetro no corresponde a una Fecha.",
                Errores = errores
            });

        if (inicio > fin)
            return BadRequest(new ErrorResponse
            {
                Mensaje = "Rango de fechas inválido.",
                Errores = [$"La fecha de inicio '{fechaInicio}' no puede ser mayor a la fecha fin '{fechaFin}'."]
            });

        var solicitudes = await _service.GetAllByRangoFechasAsync(inicio, fin);
        var lista = solicitudes.ToList();

        if (lista.Count == 0)
            return NotFound(new AptosMedicosResponse { TotalSolicitudes = 0, Solicitudes = [] });

        return Ok(new AptosMedicosResponse
        {
            TotalSolicitudes = lista.Count,
            Solicitudes = lista
        });
    }
}