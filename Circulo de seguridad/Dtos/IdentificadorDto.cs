using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class IdentificadorDto
    {
        [Required]
        public string Identificador { get; set; }
    }
}
