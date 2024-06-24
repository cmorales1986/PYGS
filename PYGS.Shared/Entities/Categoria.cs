using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.Entities
{
    public class Categoria
    {
        public int Id { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo es {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        public string? Name { get; set; }

        public DateTime? FechaAlta { get; set; }

        public DateTime? FechaModi { get; set; }

        public string? UserAlta { get; set; }

        public string? UserModi { get; set; }
    }
}
