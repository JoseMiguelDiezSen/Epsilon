using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Negocio.Persistencia;
using Negocio.Persistencia.Extensiones;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using Negocio.Servicios.Comun;
using System.Security.Principal;
using Test.Shared;

namespace Test.Negocio
{
    public class SeguridadTest : TestContextAbstract<EpsilonDbContext>
    {
        Mock<ILogger<Seguridad>> loggerFake;
        Mock<IIdentity> identityFake;
        Usuario usuarioFake;
        Mock<IPrincipal> principalFake;
        Mock<Seguridad> srvSeguridadFake;
        DbSet<Usuario> usuariosFake;
        Seguridad srvSeguridad;

        [SetUp]
        public void Setup()
        {
            string nombreUsuarioFake = @"IEF\usuario";
            loggerFake = new Mock<ILogger<Seguridad>>();
            //registroValidadoresFake = new Mock<IValidadoresProgesfor>();
            identityFake = new Mock<IIdentity>();
            identityFake.SetupGet(i => i.Name).Returns(nombreUsuarioFake);
            principalFake = new Mock<IPrincipal>();
            principalFake.SetupGet(p => p.Identity).Returns(identityFake.Object);

            int IDPFake = 120000;
            usuarioFake = new Usuario();
            usuarioFake.IdUsuario = IDPFake;
            usuarioFake.Nombre = nombreUsuarioFake;

            usuariosFake = GetQueryableMockDbSet<Usuario>(new List<Usuario>() { usuarioFake });
            contextFake.SetupGet(c => c.Usuarios).Returns(usuariosFake);
            //srvSeguridad = new Seguridad(contextFake.Object, loggerFake.Object, principalFake.Object );
            srvSeguridadFake = new Mock<Seguridad>(contextFake.Object, loggerFake.Object,principalFake.Object) { CallBase = true };
        }

        [Test]
        public void GetUsuario_Ok()
        {
            // Preparación
            var usuariosFake = GetQueryableMockDbSet<Usuario>(new List<Usuario>() { usuarioFake });
            contextFake.SetupGet(c => c.Usuarios).Returns(usuariosFake);
            //srvSeguridadFake.Setup(s => s.GetUsuario()).CallBase();

            // Ejecución
            //var usuarioActual = srvSeguridadFake.Object.GetUsuario();

            // Comprobación
            //Assert.That(usuarioFake, Is.EqualTo(usuarioActual));
        }

        [Test]
        public void GetDatosUsuario_Ok()
        {
            // Preparación
            DatosUsuario dusuFake = new DatosUsuario();
            dusuFake.IdUsuario = usuarioFake.IdUsuario;
            dusuFake.Nombre = usuarioFake.Nombre;
            var dususFake = GetQueryableMockDbSet<DatosUsuario>(new List<DatosUsuario>() { dusuFake });
            contextFake.SetupGet(c => c.DatosUsuarios).Returns(dususFake);
            //srvSeguridadFake.Setup(s => s.GetDatosUsuario()).CallBase();

            // Ejecución
            //var dusu = srvSeguridadFake.Object.GetDatosUsuario();

            // Comprobación
            //Assert.That(dusuFake, Is.EqualTo(dusu));
        }

        [Test]
        public void GetDatosUsuario_IdUsuario_Ok()
        {
            // Preparación
            //DatosUsuario dusuFake = new DatosUsuario();
            //dusuFake.IdUsuario = 10;
            //dusuFake.IdUsuario = usuarioFake.IdUsuario;
            //dusuFake.Nombre = usuarioFake.Nombre;
            //var dususFake = GetQueryableMockDbSet<DatosUsuario>(new List<DatosUsuario>() { dusuFake });
            //contextFake.SetupGet(c => c.DatosUsuarios).Returns(dususFake);
            //srvSeguridadFake.Setup(s => s.GetDatosUsuario(dusuFake.IdUsuario)).CallBase();

            //// Ejecución
            //var dusu = srvSeguridadFake.Object.GetDatosUsuario(dusuFake.IdUsuario);

            //// Comprobación
            //Assert.That(dusuFake, Is.EqualTo(dusu));
        }

        [Test]
        public void GetDatosUsuario_IdUsuario_Null()
        {
            // Preparación
            //DatosUsuario dusuFake = new DatosUsuario();
            //dusuFake.IdUsuario = 10;
            //dusuFake.Nombre = usuarioFake.Nombre;

            //var dususFake = GetQueryableMockDbSet<DatosUsuario>(new List<DatosUsuario>() { dusuFake });
            //contextFake.SetupGet(c => c.DatosUsuarios).Returns(dususFake);
            //srvSeguridadFake.Setup(s => s.GetDatosUsuario(1)).CallBase();

            //// Ejecución
            //var dusu = srvSeguridadFake.Object.GetDatosUsuario(1);

            //// Comprobación
            //Assert.That(dusu, Is.Null);
        }

        [Test]
        public void GetDatosUsuarioPorArea_Ok()
        {
            // Preparación
            //DatosUsuario dusuFake = new DatosUsuario();
            //dusuFake.IdUsuario = usuarioFake.IdUsuario;
            //dusuFake.Nombre = usuarioFake.Nombre;
            //dusuFake.EMail = usuarioFake.Email;
            //// Asignamos las extensiones del contexto al contexto mockeado <-- se deberían mockear las extenciones
            //contextFake.SetupGet(c => c.Extensions).Returns(new ExtensionesEpsilon(contextFake.Object));

            //var dususFake = GetQueryableMockDbSet<DatosUsuario>(new List<DatosUsuario>() { dusuFake });
            //contextFake.SetupGet(c => c.DatosUsuarios).Returns(dususFake);

            //// Ejecución
            //var dusus = srvSeguridadFake.Object.GetDatosUsuariosPorArea();

            //// Comprobación
            //Assert.That(dusuFake, Is.EqualTo(dusus.First()));
        }
        public bool CompararFechas(DateTime fecha1, DateTime fecha2)
        {
            if (fecha2 != fecha1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

