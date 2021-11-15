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
        public async Task<ActionResult<int>> miLocalizacion(CrearLocalizacion crearLocalizacion)
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var local = mapper.Map<Localizacion>(crearLocalizacion);
                local.UsuarioId = usu.Id;
                local.Fecha = DateTime.Now;
                await context.AddAsync(local);
                
                return await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("obtenerLocalizacion")]

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
                var localizaciones =  await context.Localizaciones.FromSqlInterpolated($"SELECT L.* FROM (SELECT MAX(L.Id)AS Id FROM(SELECT U.* FROM Usuarios U  INNER Join UsuariosGrupos G on(U.Id = G.UsuarioId AND G.Estado = 1 AND G.GrupoId = {grupo.Id}))  U INNER join Localizaciones  L on U.Id = L.UsuarioId  GROUP BY U.Id) X INNER JOIN Localizaciones L ON X.Id = L.Id ").Include(x=>x.Usuario).ToListAsync();

                var local = mapper.Map<List<LocalizacionUsuario>>(localizaciones);

                var evento = await context.Eventos.Include(x=>x.Usuario).Where(x => x.GrupoId == grupo.Id && x.FechaFinalizacion > DateTime.Now).ToListAsync();
                if (evento != null)
                {
                    foreach (var eve in evento)
                    {
                        local.Add(new LocalizacionUsuario
                        {
                            CoordenadaX = eve.CoordenadaX,
                            CoordenadaY = eve.CoordenadaY,
                            Email = eve.Usuario.Email,
                            NickName = eve.Nombre,
                            Fecha = eve.FechaFinalizacion,
                            UrlAvatar = "imagenes/marcador.png"
                        });
                    }
                }

                return local;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
