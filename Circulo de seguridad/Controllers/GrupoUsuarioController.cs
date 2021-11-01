﻿using AutoMapper;
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
        [HttpPost("susbribirse")]
        public async Task<ActionResult> susbribirse(SubcripcionGrupo subcripcion)
        {

            try
            {
                var email = HttpContext.User.FindFirst("email").Value;
                var grupo =await  context.Grupos.SingleOrDefaultAsync(x=>x.Identificador == subcripcion.Identificador);
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email==email);
                var usuGru = new UsuarioGrupo
                {
                    UsuarioId = usu.Id,
                    GrupoId = grupo.Id,
                    Estado = false
                };
                var respuesta = await context.UsuariosGrupos.AnyAsync(x => x.GrupoId == usuGru.GrupoId && x.UsuarioId == usuGru.UsuarioId);
                if (respuesta)
                {
                    return BadRequest("La subcripcion ya fue anteriormente realizada, debe esperar a ser aceptado");
                }
                context.Add(usuGru);
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