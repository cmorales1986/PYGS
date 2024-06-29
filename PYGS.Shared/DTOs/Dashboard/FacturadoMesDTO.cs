using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.DTOs.Dashboard
{
    public class FacturadoMesDTO
    {
        public int? mesnum {  get; set; }

        public string? mes {  get; set; }

        public decimal? total { get; set; }
    }
}
