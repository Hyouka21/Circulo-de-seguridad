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
            CreateMap<Localizacion, CrearLocalizacion>().ReverseMap();
            CreateMap<Usuario, RegistrarUsuario>().ReverseMap();
            CreateMap<Grupo, CrearGrupo>().ReverseMap();
            CreateMap<NotificacionDto,Notificacion>().ReverseMap()
                .ForMember(d=>d.NombreGrupo,o=>o.MapFrom(s=>s.Grupo.Nombre))
                .ForMember(d => d.NombreUsuario, o => o.MapFrom(s => s.Usuario.NickName));
            CreateMap<CrearGrupo, Grupo>().ReverseMap();
            CreateMap<CrearEvento, Evento>();
            CreateMap<Evento, EventosDto>();
        }
        
        
    }
}
