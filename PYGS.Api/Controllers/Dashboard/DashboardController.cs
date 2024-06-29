using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PYGS.Api.Data;
using PYGS.Shared.DTOs.Dashboard;

namespace PYGS.Api.Controllers.Dashboard
{
    [Route("api/adicional/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly HanaDbContext _context;

        public DashboardController(HanaDbContext context)
        {
            _context = context;
        }

        [HttpGet("cantfacturas")]
        public async Task<List<CantFacturasDTO>> GetCantidadFacturas()
        {
            try
            {
                List<CantFacturasDTO> cantidadfacturas = new List<CantFacturasDTO>();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "Select totalfac from HANASQL.dbo.DB_V_TOTAL_FACTURAS_2024";

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                cantidadfacturas.Add(new CantFacturasDTO
                                {
                                    canfact = (int)reader.GetInt64(reader.GetOrdinal("totalfac"))
                                });
                            }
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    command.Dispose();
                }
                return cantidadfacturas;
            }catch (Exception ex)
            {
                throw new Exception("Error al Obtener la cantidad de facturas", ex);
            }
        }

        [HttpGet("cantnotas")]
        public async Task<List<CantNotasDTOcs>> GetCantidadNotas()
        {
            try
            {
                List<CantNotasDTOcs> cantidadnotas = new List<CantNotasDTOcs>();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "Select totalnotas from HANASQL.dbo.DB_V_TOTAL_NOTAS_2024";

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                cantidadnotas.Add(new CantNotasDTOcs
                                {
                                    totalnotas = (int)reader.GetInt64(reader.GetOrdinal("totalnotas"))
                                });
                            }
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    command.Dispose();
                }
                return cantidadnotas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener la cantidad de Notas", ex);
            }
        }

        [HttpGet("totalmonto")]
        public async Task<List<ToalFacturadoDTO>> GetTotalFacturado()
        {
            try
            {
                List<ToalFacturadoDTO> totalfacturado = new List<ToalFacturadoDTO>();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "Select totalmonto from HANASQL.dbo.DB_V_TOTAL_FACTURADO_MONTO_2024";

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                totalfacturado.Add(new ToalFacturadoDTO
                                {
                                    totalmonto = (decimal)reader.GetDecimal(reader.GetOrdinal("totalmonto"))
                                });
                            }
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    command.Dispose();
                }
                return totalfacturado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener la cantidad de Notas", ex);
            }
        }

        [HttpGet("totaldeudacliente")]
        public async Task<List<TotalDeudaClienteDTO>> GetTotalDeudaCliente()
        {
            try
            {
                List<TotalDeudaClienteDTO> totaldeuda = new List<TotalDeudaClienteDTO>();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "Select sum(DIF_MONTO) monto from HANASQL.dbo.BI_V_EXTRACTO_CLIENTES_DETALLADO";

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                totaldeuda.Add(new TotalDeudaClienteDTO
                                {
                                    monto = (decimal)reader.GetDecimal(reader.GetOrdinal("monto"))
                                });
                            }
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    command.Dispose();
                }
                return totaldeuda;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener el total de la deuda de/los clientes", ex);
            }
        }

        [HttpGet("facturacategoria")]
        public async Task<List<TotalFacturaCategoriaDTO>> GetTotalFacturaCategoria()
        {
            try
            {
                List<TotalFacturaCategoriaDTO> facturacategoria = new List<TotalFacturaCategoriaDTO>();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select categoria, sum(totalventa)  total from HANASQL.dbo.CDM_V_VENTA_X_CANAL_IVA_FECHA where fecha >= '2024-01-01' group by categoria";

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                facturacategoria.Add(new TotalFacturaCategoriaDTO
                                {
                                    categoria = (string)reader.GetString(reader.GetOrdinal("categoria")),
                                    total = (decimal)reader.GetDecimal(reader.GetOrdinal("total"))
                                });
                            }
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    command.Dispose();
                }
                return facturacategoria;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener la facturacion por categoria", ex);
            }
        }

        [HttpGet("facturacionmes")]
        public async Task<List<FacturadoMesDTO>> GetTotalFacturaMes()
        {
            try
            {
                List<FacturadoMesDTO> facturames = new List<FacturadoMesDTO>();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select month(fecha) mesnum, b.NombreMes mes, sum(totalventa) total from CDM_V_VENTA_X_CANAL_IVA_FECHA a inner join Meses b on month(a.fecha) = b.mes where fecha >= '2024-01-01' group by MONTH(a.fecha), b.NombreMes order by MONTH(a.fecha)";

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                facturames.Add(new FacturadoMesDTO
                                {
                                    mesnum = (int)reader.GetInt32(reader.GetOrdinal("mesnum")),
                                    mes = (string)reader.GetString(reader.GetOrdinal("mes")),
                                    total = (decimal)reader.GetDecimal(reader.GetOrdinal("total"))
                                });
                            }
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    command.Dispose();
                }
                return facturames;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener la facturacion por mes", ex);
            }
        }

        [HttpGet("facturacionmesant")]
        public async Task<List<FacturadoMesDTO>> GetTotalFacturaMesAnterior()
        {
            try
            {
                List<FacturadoMesDTO> facturames = new List<FacturadoMesDTO>();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select month(fecha) mesnum, b.NombreMes mes, sum(totalventa) total from CDM_V_VENTA_X_CANAL_IVA_FECHA a inner join Meses b on month(a.fecha) = b.mes where (fecha >= '2023-01-01' and fecha <= '2023-12-31') group by MONTH(a.fecha), b.NombreMes order by MONTH(a.fecha)";

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                facturames.Add(new FacturadoMesDTO
                                {
                                    mesnum = (int)reader.GetInt32(reader.GetOrdinal("mesnum")),
                                    mes = (string)reader.GetString(reader.GetOrdinal("mes")),
                                    total = (decimal)reader.GetDecimal(reader.GetOrdinal("total"))
                                });
                            }
                            reader.Close();
                            reader.Dispose();
                        }
                    }
                    command.Dispose();
                }
                return facturames;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener la facturacion por mes", ex);
            }
        }
    }
}