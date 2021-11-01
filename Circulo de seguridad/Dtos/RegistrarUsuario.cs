using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Dtos
{
    public class RegistrarUsuario
    {
        [Required]
        public string NickName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Clave { get; set; }
        public string Avatar { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
