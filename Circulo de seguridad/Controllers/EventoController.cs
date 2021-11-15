using AutoMapper;
using Circulo_de_seguridad.Dtos;
using Circulo_de_seguridad.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Controllers
{
    [ApiController]
    [Route("evento")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EventoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        public EventoController(IMapper mapper, IConfiguration configuration, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost("crear")]
        public async Task<ActionResult<int>> crearEvento(CrearEvento crear)
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var grupo = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == crear.Identificador);
                if (grupo == null)
                {
                    return BadRequest("El grupo no existe");
                }
                var subscripcion = await context.UsuariosGrupos
                   .SingleOrDefaultAsync(x => x.UsuarioId == usu.Id && x.GrupoId == grupo.Id && x.Estado == true);
                if (subscripcion == null)
                {
                    return BadRequest("Usted no pertenece a ese grupo");
                }
                var evento = mapper.Map<Evento>(crear);
                evento.GrupoId = grupo.Id;
                evento.UsuarioId = usu.Id;
                evento.FechaCreacion = DateTime.Now;
                context.Add(evento);
                
                var notificacion = new Notificacion
                {
                    UsuarioId = usu.Id,
                    GrupoId = grupo.Id,
                    Titulo = "Evento",
                    Mensaje = "El usuario " + usu.NickName + " creo un evento",
                    FechaCreacion = DateTime.Now,
                    Estado = false
                };
                 context.Add(notificacion);
                
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("eliminar")]
        public async Task<ActionResult> eliminarEvento(EliminarEvento eliminar)
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var grupo = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == eliminar.Identificador);
                if (grupo == null)
                {
                    return BadRequest("El grupo no existe");
                }
                var subscripcion = await context.UsuariosGrupos
                   .SingleOrDefaultAsync(x => x.UsuarioId == usu.Id && x.GrupoId == grupo.Id && x.Estado == true);
                if (subscripcion == null)
                {
                    return BadRequest("Usted no pertenece a ese grupo");
                }
                var evento = await context.Eventos.SingleOrDefaultAsync(x => x.GrupoId == grupo.Id && x.FechaCreacion == DateTime.Parse(eliminar.Fecha) && x.UsuarioId == usu.Id);
                if (evento == null)
                {
                    return BadRequest("El evento no existe");
                }
                context.Remove(evento);
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("obtener")]
        public async Task<ActionResult<List<EventosDto>>> obtenerEvento(IdentificadorDto ident)
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var grupo = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == ident.Identificador);
                if (grupo == null)
                {
                    return BadRequest("El grupo no existe");
                }
                var subscripcion = await context.UsuariosGrupos
                   .SingleOrDefaultAsync(x => x.UsuarioId == usu.Id && x.GrupoId == grupo.Id && x.Estado == true);
                if (subscripcion == null)
                {
                    return BadRequest("Usted no pertenece a ese grupo");
                }
                var evento = await context.Eventos.Where(x => x.GrupoId == grupo.Id && x.FechaFinalizacion>DateTime.Now).ToListAsync();
                if (evento == null)
                {
                    return NoContent();
                }
                var eventosdtos = mapper.Map<List<EventosDto>>(evento);

                return eventosdtos;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
