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
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Controllers
{
    [ApiController]
    [Route("grupo")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LocalizacionController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        public LocalizacionController(IMapper mapper, IConfiguration configuration, ApplicationDbContext context)
        {
                this.mapper = mapper;
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost("miLocalizacion")]
        public async Task<ActionResult> miLocalizacion(CrearLocalizacion crearLocalizacion)
        {
            try
            {
                var email = HttpContext.User.FindFirst("email").Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var local = mapper.Map<Localizacion>(crearLocalizacion);
                local.UsuarioId = usu.Id;
                local.Fecha = DateTime.Now;
                await context.AddAsync(local);
                await context.SaveChangesAsync();
                return NoContent();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("obtenerLocalizacion")]
        public async Task<ActionResult<List<LocalizacionUsuario>>> obtenerLocalizacion(IdentificadorDto identificadorDto)
        {
            try
            {
                var email = HttpContext.User.FindFirst("email").Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var grupo = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == identificadorDto.Identificador);
                if (grupo == null)
                {
                    return BadRequest("El grupo no existe");
                }
                var subscripcion = await context.UsuariosGrupos
                    .SingleOrDefaultAsync(x => x.UsuarioId == usu.Id && x.GrupoId == grupo.Id && x.Estado==true );
                if (subscripcion == null)
                {
                    return BadRequest("Usted no pertenece a ese grupo");
                }
                var usuariosGrupo = await context.Usuarios.Join(
                    context.UsuariosGrupos.Where(x => x.GrupoId==grupo.Id && x.Estado==true),
                    usu => usu.Id,
                    usuGru => usuGru.UsuarioId,
                    (usu, usuGru) => usu)
                    .ToListAsync();
                List<LocalizacionUsuario> lista =  new List<LocalizacionUsuario>();
                foreach( var usuario in usuariosGrupo)
                {
                    var localizacion = await context.Localizaciones
                            .FirstOrDefaultAsync(x => x.UsuarioId == usuario.Id && x.Fecha > DateTime.Now.AddMinutes(-2));
                    lista.Add(new LocalizacionUsuario
                    {
                        CoordenadaX= localizacion.CoordenadaX,
                        CoordenadaY=localizacion.CoordenadaY,
                        NickName=usuario.NickName,
                        Email = usuario.Email,
                        UrlAvatar = usuario.Avatar
                    });
                }
                return Ok(lista);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
