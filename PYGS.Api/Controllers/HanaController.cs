using Microsoft.AspNetCore.Mvc;
using PYGS.Api.Services;
using PYGS.Shared.DTOs.Hana;

namespace PYGS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HanaController : ControllerBase
    {
        private readonly DatabaseServiceHA _databaseServiceHA;

        public HanaController(DatabaseServiceHA databaseService)
        {
            _databaseServiceHA = databaseService;
        }

        [HttpGet("articulos")]
        public async Task<IActionResult> GetArticulosAsync(CancellationToken cancellationToken)
        {
            try
            {
                var query = "SELECT \"ItemCode\", \"ItemName\", \"OnHand\", \"DfltWH\" FROM PYPORK.OITM o WHERE \"SellItem\" = 'Y' AND \"InvntItem\" = 'Y' AND \"frozenFor\" = 'N' AND \"ItemName\" IS NOT null";
                var data = await _databaseServiceHA.GetDataAsync(query, cancellationToken);
                var articulos = data.Select(d => new ArticuloDTO
                {
                    ItemCode = d["ItemCode"] as string,
                    ItemName = d["ItemName"] as string,
                    OnHand = d["OnHand"] as decimal? ?? 0,
                    DfltWH = d["DfltWH"] as string
                }).ToList();

                return Ok(articulos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("clientes")]
        public async Task<IActionResult> GetDataClientes(CancellationToken cancellationToken)
        {
            try
            {
                var query = "SELECT \"CardCode\" , \"CardName\" , \"LicTradNum\"  FROM PYGS_PRUEBAS.OCRD o WHERE \"CardType\" = 'C' AND \"CardName\" IS NOT NULL ORDER BY \"CardName\" ";
                var data = await _databaseServiceHA.GetDataAsync(query, cancellationToken);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NotFound($"Error: {ex.Message}");
            }
            
        }

        [HttpGet("remisionesgranja")]
        public async Task<IActionResult> GetRemisionesGranja(CancellationToken cancellationToken)
        {
            try
            {
                var query = "SELECT o.\"DocEntry\", o.\"DocNum\", o.\"DocDate\", o.\"CardName\", o.\"Series\" , o.\"FolioNum\" , o.\"U_EST\"||'-'||o.\"U_PDE\"||'-'||RIGHT('0000000' || (o.\"FolioNum\"),7) as nrorem, sum(w.\"Quantity\") animales, sum(w.U_EPPY_PEPR) peso FROM PYGS_PRUEBAS.OWTR o INNER JOIN PYGS_PRUEBAS.WTR1 w ON o.\"DocEntry\" = w.\"DocEntry\" WHERE o.\"DocDate\" >= '2024-01-01' AND o.CANCELED = 'N' AND o.\"Series\" = 106 AND o.\"FolioNum\" IS NOT NULL GROUP BY o.\"DocEntry\", o.\"DocNum\", o.\"DocDate\", o.\"CardName\", o.\"Series\" , o.\"FolioNum\", o.\"U_EST\", o.\"U_PDE\" ORDER BY o.\"DocDate\" DESC, o.\"FolioNum\" asc;";
                var data = await _databaseServiceHA.GetDataAsync(query, cancellationToken);
                // Convertir datos a RemisionesDTo
                var remisiones = data.Select(d => new RemisionesDTo
                {
                    DocEntry = d["DocEntry"] as int?,
                    DocNum = d["DocNum"] as int?,
                    DocDate = d["DocDate"] as DateTime?,
                    CardName = d["CardName"] as string,
                    Series = d["Series"] as int?,
                    FolioNum = d["FolioNum"] as int?,
                    NROREM = d["NROREM"] as string,
                    ANIMALES = d["ANIMALES"] as decimal? ?? 0,
                    PESO = d["PESO"] as decimal? ?? 0
                }).ToList();

                return Ok(remisiones);
            }
            catch (Exception ex)
            {
                return NotFound($"Error: {ex.Message}");
            }

        }
    }
}
