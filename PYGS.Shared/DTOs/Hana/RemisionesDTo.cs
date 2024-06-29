using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.DTOs.Hana
{
    public class RemisionesDTo
    {
        public int? DocEntry { get; set; }
        public int? DocNum { get; set; }
        public DateTime? DocDate { get; set; }
        public string? CardName { get; set; }
        public int? Series { get; set; }
        public int? FolioNum { get; set; }
        public string? NROREM { get; set; }
        public decimal? ANIMALES { get; set; }
        public decimal? PESO { get; set; }
    }
}
