using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.Entities
{
    public class RemisionesD
    {
        public int Id { get; set; }

        public RemisionesH? RemisionesH { get; set; }

        public int RemisionesHId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public decimal cantidad { get; set; } 

        public decimal pesocategoria { get; set; }

        public string FromWhsCod { get; set; }

        public string ToWhsCod { get; set; }


    }
}
