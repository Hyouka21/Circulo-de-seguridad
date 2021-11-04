using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Entidades
{
    public class Localizacion
    {
        public int Id { get; set; }
        public float CoordenadaX { get; set; }
        public float CoordenadaY { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public Usuario Usuario { get; set; }
    }
}
