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
    [Route("notificacion")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificacionController:ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        public NotificacionController(IMapper mapper, IConfiguration configuration, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost("emergencia")]
        public async Task<ActionResult> emergencia(IdentificadorDto identificadorDto)
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var gru = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == identificadorDto.Identificador);
                if (gru == null)
                {
                    return BadRequest("El grupo no se encuentra");
                }
                var notificacion = new Notificacion
                {
                    UsuarioId = usu.Id,
                    GrupoId = gru.Id,
                    Titulo = "Emergencia",
                    Mensaje = "Algo le ocurrio a " + usu.NickName + " chekea su ubicacion",
                    FechaCreacion = DateTime.Now,
                    Estado = false
                };
                await context.AddAsync(notificacion);
                await context.SaveChangesAsync();
                return NoContent();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("obtener")]
        public async Task<ActionResult<List<NotificacionDto>>> obtener(IdentificadorDto identificadorDto)//pedir grupo buscar por grupo y no devolver notificacion
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var grupo = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == identificadorDto.Identificador);
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
                var notificacion = await context.Notificaciones.Where(x => x.GrupoId==grupo.Id && x.FechaCreacion.AddDays(1)>DateTime.Now).ToListAsync();
                if (notificacion == null)
                {
                    return NoContent();
                }
                var not = mapper.Map<List<NotificacionDto>>(notificacion);
                notificacion.ForEach(x=>x.Estado=true);
                await context.SaveChangesAsync();
                return not;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
