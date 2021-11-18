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
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public UsuarioController(IMapper mapper,IConfiguration configuration,ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.context = context;
            this.environment = environment;
        }

         
            [HttpPost("registrar")]
            public async Task<ActionResult<int>> Registrar(RegistrarUsuario registrarUsuario)
            {
            try
            {
                var resultado = await context.Usuarios.AnyAsync(x => x.Email == registrarUsuario.Email);
                if (resultado)
                {
                    return BadRequest("Ya hay un usuario registrado con ese email");
                }
               
                
                if (registrarUsuario.Avatar != null)
                {
                    var stream1 = new MemoryStream(Convert.FromBase64String(registrarUsuario.Avatar));
                    IFormFile ImagenInmo = new FormFile(stream1, 0, stream1.Length, "usuario", ".jpg");
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Random r = new Random();
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "usuario_" + r.Next(0, 100000) + Path.GetExtension(ImagenInmo.FileName);
                    string pathCompleto = Path.Combine(path, fileName);

                    registrarUsuario.Avatar = Path.Combine("Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        ImagenInmo.CopyTo(stream);
                    }
                }
                var usuario = mapper.Map<Usuario>(registrarUsuario);
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                usuario.Clave = hashed;
                context.Add(usuario);
                
                return await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
             
            }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("obtenerUsuario")]
        public async Task<ActionResult<UsuarioDto>> obtenerUsuario()
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                
                    var map =mapper.Map<UsuarioDto>(usu);
                return map;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
                [HttpPost("login")]
            public async Task<ActionResult<String>> Login([FromForm]LoginUsuario credencialesUsuario)
            {
            try
            {
      
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                       password: credencialesUsuario.Clave,
                       salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                       prf: KeyDerivationPrf.HMACSHA1,
                       iterationCount: 1000,
                       numBytesRequested: 256 / 8));
                var usuario = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == credencialesUsuario.Email && x.Clave == hashed);
                if (usuario == null)
                {
                    return BadRequest("Contraseña o Email Incorrectos");
                }
                var token = ConstruirToken(usuario);
                return token.Token;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         
            private  TokenDto ConstruirToken( Usuario usu )
            {
                var claim = new List<Claim>()
            {
                new Claim("email",usu.Email),
     
            };
                
                var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:ClaveSecreta"]));
                var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
                var expiracion = DateTime.UtcNow.AddYears(1);
                var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claim, expires: expiracion, signingCredentials: creds);
                return new TokenDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    Expiracion = expiracion
                };

            }
       
        }
    }

