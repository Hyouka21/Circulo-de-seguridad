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
    [Route("gruposusuarios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GrupoUsuarioController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        public GrupoUsuarioController(IMapper mapper, IConfiguration configuration, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost("subscribirse")]
        public async Task<ActionResult<int>> subscribirse(IdentificadorDto subcripcion)
        {

            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var grupo = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == subcripcion.Identificador);
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                if (grupo == null)
                {
                    return BadRequest("El grupo no existe");
                }
                var usuGru = new UsuarioGrupo
                {
                    UsuarioId = usu.Id,
                    GrupoId = grupo.Id,
                    Estado = false,
                    Fecha = DateTime.Now
                };
                var respuesta = await context.UsuariosGrupos.AnyAsync(x => x.GrupoId == usuGru.GrupoId && x.UsuarioId == usuGru.UsuarioId);
                if (respuesta)
                {
                    return BadRequest("La subcripcion ya se realizo anteriormente");
                }
                context.Add(usuGru);

                return await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("desubscribirse")]
        public async Task<ActionResult<int>> desubscribirse(IdentificadorDto subcripcion)
        {

            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var grupo = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == subcripcion.Identificador);
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                
                var respuesta = await context.UsuariosGrupos.SingleOrDefaultAsync(x => x.GrupoId == grupo.Id && x.UsuarioId == usu.Id);

                context.Remove(respuesta);
                return await context.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("editarsubscripcion")]
        public async Task<ActionResult<int>> editarSubscripcion(EditarSubscripcion editarSubscripcion)
        {

            try
            {
                var usuSub = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == editarSubscripcion.Email);
                if (usuSub == null)
                {
                    return BadRequest("El Email no corresponde a ningun usuario");
                }
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var grupo = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == editarSubscripcion.Identificador && x.AdminId == usu.Id);
                if (grupo == null)
                {
                    return BadRequest("Usted no es administrador");
                }

                var subscripcion = await context.UsuariosGrupos.SingleOrDefaultAsync(x => x.UsuarioId == usuSub.Id && x.GrupoId == grupo.Id);
                subscripcion.Estado = editarSubscripcion.Estado;
                context.Update(subscripcion);
                
                return await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("obtenersubcripciones")]
        public async Task<ActionResult<List<SubscripcionDto>>> obtenerSubscripcion(IdentificadorDto identificador)
        {

            try
            {

                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var grupo = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == identificador.Identificador);
                if (grupo == null)
                {
                    return BadRequest("El grupo no existe");
                }
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var subscripciones = await context.UsuariosGrupos.Include(g => g.Grupo).Include(u => u.Usuario).Where(x => x.GrupoId == grupo.Id && x.Grupo.AdminId == usu.Id&&x.UsuarioId!=usu.Id).ToListAsync();
                var subs = mapper.Map<List<SubscripcionDto>>(subscripciones);
                return subs;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
    