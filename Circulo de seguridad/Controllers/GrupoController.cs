﻿using AutoMapper;
using AutoMapper.Configuration;
using Circulo_de_seguridad.Dtos;
using Circulo_de_seguridad.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public GrupoController(IMapper mapper, IConfiguration configuration, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.context = context;
        }
        [HttpPost("crear")]
        public async Task<ActionResult> crearGrupo(CrearGrupo crear)
        {
            try
            {
                var email = HttpContext.User.FindFirst("email").Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                var grupo = mapper.Map<Grupo>(crear);
               
                grupo.FechaCreacion = DateTime.Now;
                grupo.AdminId = usu.Id;
                context.Add(grupo);
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: grupo.AdminId+grupo.Id+"",
                        salt: System.Text.Encoding.ASCII.GetBytes("SALYAZUCAR21"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                grupo.Identificador = hashed;
                await context.SaveChangesAsync();
                return NoContent();
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
     
    }
}
