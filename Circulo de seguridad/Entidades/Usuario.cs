using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Entidades
{
   // [Index(nameof(Email) ,nameof(NickName), IsUnique = true)]
    public class Usuario
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string Avatar { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [NotMapped]
        public List<UsuarioGrupo> UsuariosGrupos { get; set; }
    }
}
