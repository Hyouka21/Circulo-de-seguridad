using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Entidades
{
    public class Evento
    {
        public int Id { get; set; }
        public float CoordenadaX { get; set; }
        public float CoordenadaY { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
