using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Entidades
{
    public class UsuarioGrupo
    {
        public int GrupoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
        public Grupo Grupo { get; set; }
        public Usuario Usuario { get; set; }
    }
}
