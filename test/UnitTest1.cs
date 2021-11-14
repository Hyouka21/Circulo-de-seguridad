using AutoMapper;

using Circulo_de_seguridad;
using Circulo_de_seguridad.Controllers;
using Circulo_de_seguridad.Dtos;
using Microsoft.Extensions.Configuration;
using System;

using Xunit;

namespace test
{
    public class UnitTest1
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        UsuarioController controller;
		public UnitTest1(IMapper mapper, IConfiguration configuration, ApplicationDbContext context)
		{
			controller = new UsuarioController(mapper, configuration, context);
            this.mapper = mapper;
            this.context = context;
        }
		[Fact]
        public async void Test1()
        {

			//var result = await controller.Registrar(new RegistrarUsuario { NickName="gast","" }) ;
			//[Fact]
			//public void MiPerfil()
			//{
			//	string email = "mluzza@ulp.edu.ar";
			//	controller.ControllerContext = new ControllerContext()
			//	{
			//		HttpContext = new DefaultHttpContext() { User = helper.MockLogin(email, "Propietario") }
			//	};
			//	var res = controller.Get().Result.Value;
			//	Assert.Equal(email, res.Email);
			//	Assert.Equal("Mariano", res.Nombre);
			//}

		}
	}
}
