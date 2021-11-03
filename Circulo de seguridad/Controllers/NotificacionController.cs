﻿using AutoMapper;
using AutoMapper.Configuration;
using Circulo_de_seguridad.Dtos;
using Circulo_de_seguridad.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Circulo_de_seguridad.Controllers
{
    [ApiController]
    [Route("grupo")]
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
                    FechaCreacion = DateTime.Now
                };
                await context.AddAsync(notificacion);
                await context.SaveChangesAsync();
                return NoContent();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("obtener")]
        public async Task<ActionResult<List<Notificacion>>> obtener()//no devolver la notificacion
        {
            try
            {
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var usu = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == email);
                return await context.Notificaciones.Where(x => x.UsuarioId == usu.Id).ToListAsync();
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
