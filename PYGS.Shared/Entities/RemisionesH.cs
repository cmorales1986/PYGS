using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.Entities
{
    public class RemisionesH
    {
        public int Id { get; set; }

        public int series {  get; set; }

        public DateTime fecha { get; set; }

        public DateTime fechaConta { get; set; }

        public int NroFolio { get; set; }

        public string? establecimiento { get; set; }

        public string? emision { get; set; }

        public string? timbrado { get; set; }

        public string? CardCode { get; set; }

        public string? CardName { get; set; }

        public string? RUC { get; set; }

        public string? Chofer { get; set; }

        public string? Marca { get; set; } 

        public string? Matricula { get; set; }

        public int FolioNum { get; set; }

        public ICollection<RemisionesD>? remisionesD { get; set; }

    }
}
