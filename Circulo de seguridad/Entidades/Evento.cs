using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Entidades
{
    public class Evento
    {
        public int Id { get; set; }
        public int CoordenadaX { get; set; }
        public int CoordenadaY { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }
    }
}
