using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.Entities.HANASQL
{
    [Table("programacion_cerdos_mensual")]
    public class ProgramacionCerdoMensual
    {
        public int id { get; set; }

        public DateTime fecha_ini {  get; set; }

        public DateTime fecha_fin {  get; set; }

        public string? semana_anho { get; set; }

        public decimal? cant_animales { get; set; }

        public decimal? cant_cargas { get; set; }

        public int? novex { get; set; }
        public int? pirayu { get; set; }
        public int? itabo { get; set; }

        public int? compras { get; set; }


    }
}
