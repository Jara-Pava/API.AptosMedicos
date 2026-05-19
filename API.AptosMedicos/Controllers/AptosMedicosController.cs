using API.AptosMedicos.Models;
using API.AptosMedicos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.AptosMedicos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AptosMedicosController : ControllerBase
{
    private readonly ISolicitudAptoMedicoService _service;

    // Longitud máxima permitida para IdGlobal
    private const int IdGlobalMaxLength = 50;

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

    [HttpGet("solicitud/{idSolicitud}")]
    public async Task<IActionResult> GetById(string idSolicitud)
    {
        if (string.IsNullOrWhiteSpace(idSolicitud) || idSolicitud.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ErrorResponse
            {
                Errores = ["El parámetro 'idSolicitud' es obligatorio"]
            });

        // Rechazar si incluye caracteres no numéricos como punto o coma
        if (idSolicitud.Contains('.') || idSolicitud.Contains(','))
            return BadRequest(new ErrorResponse
            {
                Errores = [$"El valor proporcionado 'idSolicitud' debe ser Entero. Valor recibido: '{idSolicitud}'"]
            });

        // Rechazar no enteros
        if (!int.TryParse(idSolicitud, out var id))
            return BadRequest(new ErrorResponse
            {
                Errores = [$"El valor proporcionado 'idSolicitud' debe ser un Entero. Valor recibido: '{idSolicitud}'."]
            });

        if (id < 0)
            return BadRequest(new ErrorResponse
            {
                Errores = [$"El valor proporcionado 'idSolicitud' debe ser un valor númerico positivo. Valor recibido: '{idSolicitud}'."]
            }); 

        var solicitud = await _service.GetByIdAsync(id);

        if (solicitud is null)
            return NotFound(new NotFoundResponse
            {
                Mensaje = $"No se encontró ninguna solicitud con el id: '{idSolicitud}', intente más tarde"
            });

        return Ok(new AptosMedicosResponse
        {
            TotalSolicitudes = 1,
            Solicitudes = [solicitud]
        });
    }

    [HttpGet("global/{idGlobal}")]
    public async Task<IActionResult> GetByIdGlobal(string idGlobal)
    {
        if (string.IsNullOrWhiteSpace(idGlobal) || idGlobal.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ErrorResponse
            {
                Errores = ["El parámetro 'idGlobal' es obligatorio."]
            });
        var solicitudes = await _service.GetByIdGlobalAsync(idGlobal);
        var lista = solicitudes.ToList();

        if (lista.Count == 0)
            return NotFound(new NotFoundResponse
            {
                Mensaje = $"No se encontraron solicitudes con el id global: '{idGlobal}'"
            });

        return Ok(new AptosMedicosResponse
        {
            TotalSolicitudes = lista.Count,
            Solicitudes = lista
        });
    }

    [HttpGet("fecha/{fechaSolicitud}")]
    public async Task<IActionResult> GetByFechaSolicitud(string fechaSolicitud)
    {
        if (string.IsNullOrWhiteSpace(fechaSolicitud) || fechaSolicitud.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new ErrorResponse
            {
                Errores = ["El parámetro 'fechaSolicitud' es obligatorio."]
            });

        // Rechazar si incluye componente de hora
        if (fechaSolicitud.Contains(' ') || fechaSolicitud.Contains('T'))
            return BadRequest(new ErrorResponse
            {
                Errores = [$"El valor proporcionado 'fechaSolicitud' no debe incluir hora. Use el formato yyyy-MM-dd. Valor recibido: '{fechaSolicitud}'."]
            });

        if (!DateTime.TryParseExact(fechaSolicitud, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var fecha))
            return BadRequest(new ErrorResponse
            {
                Errores = [$"El valor proporcionado 'fechaSolicitud' tiene que tener el formato yyyy-MM-dd. Valor recibido: '{fechaSolicitud}'."]
            });

        // validar fecha maxima 9999-12-31
        if (fecha >= new DateTime(9999, 12, 31))
            return BadRequest(new ErrorResponse
            {
                Errores = [$"El valor proporcionado 'fechaSolicitud' no puede ser mayor o igual a 9999-12-31. Valor recibido: '{fechaSolicitud}'."]
            });

        // validar fecha minima 0001-01-01
        if (fecha <= new DateTime(1, 1, 1))
            return BadRequest(new ErrorResponse
            {
                Errores = [$"El valor proporcionado 'fechaSolicitud' no puede ser menor o igual a 0001-01-01. Valor recibido: '{fechaSolicitud}'."]
            });

        var solicitudes = await _service.GetAllByFechaSolicitudAsync(fecha);
        var lista = solicitudes.ToList();

        if (lista.Count == 0)
            return NotFound(new NotFoundResponse
            {
                Mensaje = $"No se encontró ningún registro con la fecha '{fechaSolicitud}'"
            });

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

        if (string.IsNullOrWhiteSpace(fechaInicio) || fechaInicio.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
            errores.Add("El parámetro 'fechaInicio' es obligatorio.");
        if (string.IsNullOrWhiteSpace(fechaFin) || fechaFin.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
            errores.Add("El parámetro 'fechaFin' es obligatorio.");

        //if (errores.Count > 0)
        //    return BadRequest(new ErrorResponse {Errores = errores });

        bool inicioValido = DateTime.TryParseExact(fechaInicio, "yyyy-MM-dd",
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None, out var inicio);

        bool finValido = DateTime.TryParseExact(fechaFin, "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var fin);

        // aplicado al rango: Rechazar si incluye hora
        if (fechaInicio.Contains(' ') || fechaInicio.Contains('T'))
            errores.Add($"El valor proporcionado 'fechaInicio' no debe incluir hora. Use el formato yyyy-MM-dd. Valor recibido: '{inicio}'.");
        if (fechaFin.Contains(' ') || fechaFin.Contains('T'))
            errores.Add($"El valor proporcionado 'fechaFin' no debe incluir hora. Use el formato yyyy-MM-dd. Valor recibido: '{fin}'.");

        if (errores.Count > 0)
            return BadRequest(new ErrorResponse { Errores = errores });

        if (!inicioValido)
            errores.Add($"La fecha proporcionada 'fechaInicio' debe tener formato yyyy-MM-dd. Valor recibido: '{fechaInicio}'.");
        if (!finValido)
            errores.Add($"La fecha proporcionada 'fechaFin' debe tener formato yyyy-MM-dd. Valor recibido: '{fechaFin}'.");
        if (errores.Count > 0)
            return BadRequest(new ErrorResponse { Errores = errores });

        if (inicio >= new DateTime(9999, 12, 31))
            errores.Add($"La fecha proporcionada 'fechaInicio' no puede ser mayor o igual a 9999-12-31. Valor recibido: '{fechaInicio}'.");
        if (inicio <= new DateTime(1, 1, 1))
            errores.Add($"La fecha proporcionada 'fechaInicio' no puede ser menor o igual a 0001-01-01. Valor recibido: '{fechaInicio}'.");
        if (fin >= new DateTime(9999, 12, 31))
            errores.Add($"El parámetro 'fechaFin' no puede ser mayor o igual a 9999-12-31. Valor recibido: '{fechaFin}'.");
        if (fin <= new DateTime(1, 1, 1))
            errores.Add($"El parámetro 'fechaFin' no puede ser menor o igual a 0001-01-01. Valor recibido: '{fechaFin}'.");

        if (errores.Count > 0)
            return BadRequest(new ErrorResponse { Errores = errores });

        // Inicio mayor que fin
        if (inicio > fin)
            return BadRequest(new ErrorResponse
            {
                Errores = [$"La fecha de inicio '{fechaInicio}' no puede ser mayor a la fecha fin '{fechaFin}'."]
            });

        var solicitudes = await _service.GetAllByRangoFechasAsync(inicio, fin);
        var lista = solicitudes.ToList();

        if (lista.Count == 0)
            return NotFound(new NotFoundResponse
            {
                Mensaje = $"No se encontró ningúna solicitud entre el rango de fechas proporcionados. Valores recibidos: Fecha Inicio: {fechaInicio} y Fecha Fin: {fechaFin}"
            });

        return Ok(new AptosMedicosResponse
        {
            TotalSolicitudes = lista.Count,
            Solicitudes = lista
        });
    }
}