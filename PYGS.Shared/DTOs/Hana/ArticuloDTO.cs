using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.DTOs.Hana
{
    public class ArticuloDTO
    {
        public string? ItemCode { get; set; }

        public string? ItemName { get; set; }

        public decimal? OnHand { get; set; }

        public string? DfltWH { get; set; }


    }
}
