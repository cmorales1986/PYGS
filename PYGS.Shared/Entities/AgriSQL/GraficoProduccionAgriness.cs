using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.Entities.AgriSQL
{
    [Table("graficos_produccion_agriness")]
    public class GraficoProduccionAgriness
    {
        public int id { get; set; }
        public int? mesnum { get; set; }

        public string? mes {  get; set; }

        public int? anhoo { get; set; }

        public int? categoria_num { get; set; }

        public string? categoria { get; set;}

        public int? reportenum { get; set; }

        public string? reporte { get; set; }

        public string? periodo { get; set; }

        public decimal? cantidad { get; set; }

        public decimal? meta { get; set; }

        public decimal? media {  get; set; }

        public decimal? media_2023 { get; set; }

    }
}
