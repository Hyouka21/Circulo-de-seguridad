using AutoMapper;
using Circulo_de_seguridad.Dtos;
using Circulo_de_seguridad.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Controllers
{
    [ApiController]
    [Route("grupo")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GrupoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public GrupoController(IMapper mapper, IConfiguration configuration, ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.context = context;
            this.environment = environment;
        }
        [HttpPost("crear")]
        public async Task<ActionResult<int>> crearGrupo(CrearGrupo crear)
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var grupo = mapper.Map<Grupo>(crear);

                grupo.FechaCreacion = DateTime.Now;
                grupo.AdminId = usu.Id;
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: grupo.AdminId + grupo.Id + ""+grupo.FechaCreacion,
                        salt: System.Text.Encoding.ASCII.GetBytes("SALYAZUCAR21"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                grupo.Identificador = hashed;
                if (crear.AvatarGrupo != null)
                {
                    var stream1 = new MemoryStream(Convert.FromBase64String(crear.AvatarGrupo));
                    IFormFile ImagenInmo = new FormFile(stream1, 0, stream1.Length, "grupo", ".jpg");
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Random r = new Random();
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "grupo_"  + r.Next(0, 100000) + Path.GetExtension(ImagenInmo.FileName);
                    string pathCompleto = Path.Combine(path, fileName);

                    grupo.AvatarGrupo = Path.Combine("Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        ImagenInmo.CopyTo(stream);
                    }
                }
                    context.Add(grupo);
                await context.SaveChangesAsync();
                var grupoA = await context.Grupos.SingleOrDefaultAsync(x => x.Identificador == grupo.Identificador);
                var grupoUsu = new UsuarioGrupo
                {
                    GrupoId = grupoA.Id,
                    UsuarioId = usu.Id,
                    Estado = true,
                    Fecha = DateTime.Now
                };
                context.Add(grupoUsu);
                
                return await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("obtener")]

        public async Task<ActionResult<List<GrupoDto>>> obtener()
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var Grupos = await context.UsuariosGrupos.Include(x => x.Grupo).Where(x => x.UsuarioId == usu.Id && x.Estado == true).ToListAsync();
                return mapper.Map<List<GrupoDto>>(Grupos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("obtenerAdmin")]

        public async Task<ActionResult<List<GrupoDto>>> obtenerAdmin()
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var Grupos = await context.Grupos.Where(x=>x.AdminId==usu.Id).ToListAsync();
                return mapper.Map<List<GrupoDto>>(Grupos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
