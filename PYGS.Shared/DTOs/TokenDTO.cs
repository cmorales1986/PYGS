using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYGS.Shared.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }
    }
}
