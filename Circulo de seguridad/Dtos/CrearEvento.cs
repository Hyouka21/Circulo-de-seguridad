using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class CrearEvento
    {
        [Required]
        public string Identificador { get; set; }
        public float CoordenadaX { get; set; }
        public float CoordenadaY { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string FechaFinalizacion { get; set; }
    }
}
