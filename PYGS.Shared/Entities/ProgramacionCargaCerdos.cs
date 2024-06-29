using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.Entities
{
    [Table("programacion_carga_cerdos")]
    public class ProgramacionCargaCerdos
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Cliente { get; set; }
        public int Cantidad { get; set; }
        public string? Tipo { get; set; }
    }
}
