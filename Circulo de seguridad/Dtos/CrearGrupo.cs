
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class CrearGrupo
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public string AvatarGrupo { get; set; }
    }
}
