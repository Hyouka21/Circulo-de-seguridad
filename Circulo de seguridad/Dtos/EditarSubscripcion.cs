using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class EditarSubscripcion
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Identificador { get; set; }
        [Required]
        public string Estado { get; set; }
    }
}
