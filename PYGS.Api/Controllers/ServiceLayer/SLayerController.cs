using Microsoft.AspNetCore.Mvc;
using PYGS.Shared.Entities;
using PYGS.Api.Services;
using PYGS.Api.Models;

namespace PYGS.Api.Controllers.ServiceLayer
{
    [Route("api/servicelayer/[controller]")]
    [ApiController]
    public class SLayerController : ControllerBase
    {
        private readonly IServiceLayerComponent _serviceLayer;

        public SLayerController(IServiceLayerComponent serviceLayer)
        {
            _serviceLayer = serviceLayer;
        }

        [HttpGet("login")]
        public async Task<ActionResult> GetLoginAsync()
        {
            var result = await _serviceLayer.Login();
            if (result)
            {
                return Ok(result);
            }
            if(_serviceLayer.IsError)
            {
                return BadRequest(_serviceLayer.ErrorMessage);
            }
            return BadRequest();
        }

        [HttpGet("logout")]
        public async Task<ActionResult> GetLogoutAsync()
        {
            try
            {
                await _serviceLayer.Logout();
                if (_serviceLayer.IsError)
                {
                    return BadRequest(_serviceLayer.ErrorMessage);
                }
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            
        }

        [HttpGet("GetCardDetails")]
        public async Task<IActionResult> GetCardDetails([FromQuery] string RUC, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(RUC))
            {
                return BadRequest("RUC es requerido.");
            }

            var cardData = await _serviceLayer.GetCardDetails(RUC, cancellationToken);

            if (cardData == null)
            {
                return NotFound("No se encontró el proveedor para el RUC proporcionado.");
            }

            return Ok(cardData);

        }

        [HttpPost("crearsocio")]
        public async Task<IActionResult> CreateBusinessPartner([FromBody] BusinessPartner businessPartner, CancellationToken cancellationToken)
        {
            if (businessPartner == null)
            {
                return BadRequest("El objeto BusinessPartner es requerido.");
            }

            bool isSuccess = await _serviceLayer.CreateBusinessPartner(businessPartner, cancellationToken);

            if (isSuccess)
            {
                return Created("", "BusinessPartner creado exitosamente.");
            }
            else
            {
                return StatusCode(500, _serviceLayer.ErrorMessage);
            }
        }


    }
}
