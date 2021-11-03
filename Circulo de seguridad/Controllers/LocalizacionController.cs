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
    [Route("localizacion")]
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
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
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
                
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type== ClaimTypes.Email).Value;
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
                var localizaciones=    await context.Localizaciones.FromSqlInterpolated($"EXECUTE obtenerLocalizacionGrupoMayor  @idGrupo = {grupo.Id} ").ToListAsync();
               
                List<LocalizacionUsuario> lista = new List<LocalizacionUsuario>();
                foreach( var localiza in localizaciones)
                {
                    var usur = await context.Usuarios.SingleOrDefaultAsync(x => x.Id == localiza.UsuarioId);
                    lista.Add(new LocalizacionUsuario
                    {
                        CoordenadaX= localiza.CoordenadaX,
                        CoordenadaY= localiza.CoordenadaY,
                        NickName= usur.NickName,
                        Email = usur.Email,
                        UrlAvatar = usur.Avatar,
                        Fecha = localiza.Fecha
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
