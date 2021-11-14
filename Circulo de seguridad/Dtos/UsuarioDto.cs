using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class UsuarioDto
    {
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
