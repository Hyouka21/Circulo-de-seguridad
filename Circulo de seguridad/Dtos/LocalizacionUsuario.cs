using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class LocalizacionUsuario
    {
        public float CoordenadaX { get; set; }
        public float CoordenadaY { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string UrlAvatar { get; set; }
        public DateTime Fecha { get; set; }

    }
}
