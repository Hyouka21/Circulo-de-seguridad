using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Entidades
{
   // [Index(nameof(Identificador), IsUnique = true)]
    public class Grupo
    {
        public int Id { get; set; }
        public string Identificador { get;set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string AvatarGrupo { get; set; }
        public DateTime FechaCreacion { get; set; }
        [NotMapped]
        public List<UsuarioGrupo> UsuariosGrupos { get; set; }
        public int AdminId { get; set; }
        public Usuario Admin { get; set; }

    }
}
