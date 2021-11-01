using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Entidades
{
    public class Localizacion
    {
        public int Id { get; set; }
        public int CoordenadaX { get; set; }
        public int CoordenadaY { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public Usuario Usuario { get; set; }
    }
}
