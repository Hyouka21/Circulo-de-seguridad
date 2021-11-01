using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
