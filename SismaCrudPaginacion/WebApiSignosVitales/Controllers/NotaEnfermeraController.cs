using DataSignosVitales.DTOs;
using DataSignosVitales.Interfaces;
using LogicaSignosVitales.Interfaces;
using LogicaSignosVitales.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiSignosVitales.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotaEnfermeraController : ControllerBase
    {
        private readonly INotaEnfermeriaService _notaEnfermeriaService;
        private readonly ISisMaeService _sisMaeService;  
        public NotaEnfermeraController(INotaEnfermeriaService notaEnfermeriaService, ISisMaeService sisMaeService)
        {
            _notaEnfermeriaService = notaEnfermeriaService;
            _sisMaeService = sisMaeService;
        }

        [HttpPost("CreateNotaEnfermera")]
        public async Task<IActionResult> CreateNotaEnfermeria(NotaEnfermeriaDTOs notaEnfermeria)
        {
            try
            {
                var codigoIngreso = await _sisMaeService.GetSisMae(notaEnfermeria.Ingreso);

                if (codigoIngreso == null)
                {
                    return BadRequest($"El ingreso: {notaEnfermeria.Ingreso} no existe, debe realizar un estudio para poder registrar los signos vitales");
                }

                var nota = await _notaEnfermeriaService.GetNotaEnfermera(notaEnfermeria.Numero);

                if (nota != null )
                {
                    return BadRequest($"La nota de enfermería {notaEnfermeria.Numero} ya existe");
                }

                await _notaEnfermeriaService.CreateNota(notaEnfermeria);

                var fechaActual = DateTime.Now;
                var mensaje = $"Signos vitales guardados exitosamente. Fecha: {fechaActual}";
                return Ok(new { Message = mensaje, Nota = notaEnfermeria });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Error al agregar la nota de enfermería: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Error al registrar el ingreso: " + ex.Message);
            }
        }


        [HttpPut("UpdateNotaEnfermera/{numero}")]
        public async Task<IActionResult> UpdateNotaEnfermeria(string numero, NotaEnfermeriaDTOs notaEnfermeria)
        {
            if (numero!= notaEnfermeria.Numero)
            {
                return BadRequest(new { message = $"El Numero = ( {numero}) de la URL no coincide con el Id ({notaEnfermeria.Numero}) del cuerpo de la solicitud  " });
            }
            var notaUpdate = await _notaEnfermeriaService.GetNotaEnfermera(numero);

            if (notaUpdate == null)
            {
                return NotFound(new { message = $"No se encontro Nota de Enfermera  con codigo {numero}" });
            }
            else
            {

                await _notaEnfermeriaService.UpdateNota(numero,notaEnfermeria);

                var fechaActual = DateTime.Now;
                var mensaje = $"Signos vitales se ha actualizado exitosamente. Fecha: {fechaActual}";
                return Ok(new { Message = mensaje, Nota = notaEnfermeria });
                
            }

        }

        [HttpDelete("Delete/{numero}")]
        public async Task<IActionResult> DeleteNotaEnfermeria(string numero)
        {
            var exitsCrud = await _notaEnfermeriaService.GetNotaEnfermera(numero);

            if (exitsCrud == null)
            {
                return NotFound(new { message = $"No se encontro Nota de Enfermera con id {numero}" });
            }

            await _notaEnfermeriaService.DeleteNota(numero);


            return NoContent();
        }

        [HttpGet("GetNotaEnfermera")]
        public async Task<IActionResult> GetNotaEnfermeria(int page = 1, int pageSize = 10, string? numero = null,  DateTime? Fecha = null, string? codEnfermera = null)
        {
            if (string.IsNullOrWhiteSpace(numero) && Fecha == null && string.IsNullOrWhiteSpace(codEnfermera))
            {
                return NotFound("Ingrese al menos un campo");
            }
            var notaEnfermeria = new NotaEnfermeriaDTOs();

            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (nota, totalPages) = await _notaEnfermeriaService.GetNotaEnfermeria(page, pageSize, numero, Fecha, codEnfermera);

            if (nota == null)
            {
                return NotFound("Debe Agregar valores a los parametros");
            }

            var result = new
            {
                Data = nota,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }



        //[NonAction]
        //public async Task<string> ValidateSisMae(NotaEnfermeriaDTOs notaEnfermeria)
        //{
        //    string result = "existe";

        //    var codigoIngreso = await _sisMaeService.GetSisMae(notaEnfermeria.Ingreso);

        //    if (codigoIngreso == null)
        //    {
        //        result = $"El ingreso {codigoIngreso.ConEstudio} no existe";
        //    } 
            
        //    return result;

        //}

    }

}
