using AutoMapper;
using Circulo_de_seguridad.Dtos;
using Circulo_de_seguridad.Entidades;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

        public UsuarioController(IMapper mapper,IConfiguration configuration,ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.context = context;
        }

         
            [HttpPost("registrar")]
            public async Task<ActionResult<TokenDto>> Registrar(RegistrarUsuario registrarUsuario)
            {
            try
            {
                var resultado = await context.Usuarios.AnyAsync(x => x.Email == registrarUsuario.Email);
                if (!resultado)
                {
                    return BadRequest("Ya hay un usuario registrado con ese nombre");
                }
                var usuario = mapper.Map<Usuario>(registrarUsuario);
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                usuario.Clave = hashed;
                await context.AddAsync(usuario);
                return  ConstruirToken(usuario.Email, usuario.Id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
             
            }
            [HttpPost("login")]
            public async Task<ActionResult<TokenDto>> Login(LoginUsuario credencialesUsuario)
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
                return  ConstruirToken(usuario.Email, usuario.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         
            private  TokenDto ConstruirToken(string email , int usuarioId)
            {
                var claim = new List<Claim>()
            {
                new Claim("email",email),
     
            };
                
                var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
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

