using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class NotificacionDto
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string NombreUsuario { get; set; }
        public string Avatar { get; set; }
        public bool Estado { get; set; }
    }
}
