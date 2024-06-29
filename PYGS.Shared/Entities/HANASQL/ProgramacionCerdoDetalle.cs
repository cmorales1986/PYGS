using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.Entities.HANASQL
{
    [Table("programacion_cerdos_detalle")]
    public class ProgramacionCerdoDetalle
    {
        public int Id { get; set; }

        public DateTime fecha { get; set; }

        public int cantidad_carga { get; set; }

        public int cerdos_x_carda { get; set; }

        public decimal total { get; set; }
    }
}
