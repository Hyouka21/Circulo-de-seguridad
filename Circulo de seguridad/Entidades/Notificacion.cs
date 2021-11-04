using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Entidades
{
    public class Notificacion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int GrupoId { get; set; }
        public int UsuarioId { get; set; }
        public bool Estado { get; set; }
        public Grupo Grupo { get; set; }
        public Usuario Usuario { get; set; }
    }
}
