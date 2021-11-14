using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class SubscripcionDto
    {
        public string Email { get; set; }
        public string NickName { get; set; }
        public bool Estado { get; set; }
        public string Avatar { get; set; }
    }
}
