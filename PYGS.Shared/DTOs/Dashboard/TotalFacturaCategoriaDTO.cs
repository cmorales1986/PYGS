using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.DTOs.Dashboard
{
    public class TotalFacturaCategoriaDTO
    {
        public string? categoria {  get; set; }

        public decimal? total { get; set; }

        public string? MontoFormateado => total?.ToString("#,##0", new CultureInfo("es-ES"));
    }
}
