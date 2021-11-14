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
            CreateMap<Localizacion, LocalizacionUsuario>()
                .ForMember(d => d.NickName, o => o.MapFrom(s => s.Usuario.NickName))
                .ForMember(d => d.UrlAvatar, o => o.MapFrom(s => s.Usuario.Avatar))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Usuario.Email));
            CreateMap<Usuario, RegistrarUsuario>().ReverseMap();
            CreateMap<UsuarioDto, Usuario>().ReverseMap();
            CreateMap<Grupo, CrearGrupo>().ReverseMap();
            CreateMap<GrupoDto, Grupo>().ReverseMap();
            CreateMap<NotificacionDto,Notificacion>().ReverseMap()
                .ForMember(d=>d.NombreGrupo,o=>o.MapFrom(s=>s.Grupo.Nombre))
                .ForMember(d => d.NombreUsuario, o => o.MapFrom(s => s.Usuario.NickName));
            CreateMap<CrearGrupo, Grupo>().ReverseMap();
            CreateMap<UsuarioGrupo, GrupoDto>()
                .ForMember(d => d.Descripcion, o => o.MapFrom(s => s.Grupo.Descripcion))
                .ForMember(d => d.FechaCreacion, o => o.MapFrom(s => s.Grupo.FechaCreacion))
                .ForMember(d => d.Identificador, o => o.MapFrom(s => s.Grupo.Identificador))
                .ForMember(d => d.Nombre, o => o.MapFrom(s => s.Grupo.Nombre))
                .ForMember(d => d.AvatarGrupo, o => o.MapFrom(s => s.Grupo.AvatarGrupo)); ;
            CreateMap<CrearEvento, Evento>();
            CreateMap<Evento, EventosDto>();
            CreateMap<UsuarioGrupo, SubscripcionDto>()
                .ForMember(d => d.NickName, o => o.MapFrom(s => s.Usuario.NickName))
                .ForMember(d => d.Estado, o => o.MapFrom(s => s.Estado))
                .ForMember(d => d.Avatar, o => o.MapFrom(s => s.Usuario.Avatar))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Usuario.Email));
        }
        
        
    }
}
