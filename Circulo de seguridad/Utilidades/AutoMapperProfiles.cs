using AutoMapper;
using Circulo_de_seguridad.Dtos;
using Circulo_de_seguridad.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Localizacion, CrearLocalizacion>();
            CreateMap<Usuario, RegistrarUsuario>();
            CreateMap<Grupo, CrearGrupo>();
        }
        
        
    }
}
