using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using Negocio.Validadores.Comun;
using System.Security.Principal;
using Test.Shared;

namespace Test.Negocio
{
    // Tests para la lógica de usuarios. Se usa un Setup compartido para
    // inicializar mocks comunes y evitar repetir la misma configuración en cada prueba.
    // Cada test puede reemplazar la configuración del contexto (contextFake) si necesita datos específicos.
    public class UsuariosTest : TestContextAbstract<EpsilonDbContext>
    {
        // Mocks y objetos compartidos por las pruebas. Pueden ser sobrescritos por cada test.
        private Mock<GestionUsuarios>? srvUsuariosFake;
        private Mock<IIdentity>? identityFake;
        private Usuario? usuarioFake;
        private Mock<IPrincipal>? principalFake;
        private Mock<Seguridad>? srvSeguridadFake;
        private Mock<ILogger<Seguridad>>? loggerFake;
        private DbSet<Usuario>? usuariosFake;

        // Mocks reutilizados por la mayoría de pruebas: movidos al Setup para evitar duplicación
        private Mock<ILogger<GestionUsuarios>>? loggerMock;
        private Mock<ISeguridad>? seguridadMock;
        private Mock<IValidadoresProgesfor>? validadoresMock;

        [SetUp]
        public void Setup()
        {
            // Datos por defecto útiles para varias pruebas; los tests que necesiten
            // otros datos deben configurar su propio DbSet y sobreescribir contextFake.
            string nombreUsuarioFake = @"IEF\\usuario";
            int idUsuarioFake = 1;

            loggerFake = new Mock<ILogger<Seguridad>>();
            identityFake = new Mock<IIdentity>();
            identityFake.SetupGet(i => i.Name).Returns(nombreUsuarioFake);

            usuarioFake = new Usuario
            {
                IdUsuario = idUsuarioFake,
                Nombre = nombreUsuarioFake
            };

            // DbSet mock por defecto con un usuario. Las pruebas individuales pueden reemplazarlo.
            usuariosFake = GetQueryableMockDbSet<Usuario>(new List<Usuario>() { usuarioFake });
            contextFake.SetupGet(c => c.Usuarios).Returns(usuariosFake);

            // Principal mock necesario para construir servicios que lo requieran
            principalFake = new Mock<IPrincipal>();
            principalFake.SetupGet(p => p.Identity).Returns(identityFake.Object);

            // Mock de la clase Seguridad (se usa CallBase=true para permitir ejecutar la lógica real cuando convenga)
            srvSeguridadFake = new Mock<Seguridad>(contextFake.Object, loggerFake.Object, principalFake.Object) { CallBase = true };

            // Mocks compartidos para los servicios de gestión de usuarios (para evitar repetición en cada test)
            loggerMock = new Mock<ILogger<GestionUsuarios>>();
            seguridadMock = new Mock<ISeguridad>();
            validadoresMock = new Mock<IValidadoresProgesfor>();
        }

        [Test]
        public void UpdateUser_ModificaUsuarioCorrectamente()
        {
            // Arrange: lista que representa la "tabla" Usuarios
            var usuarios = new List<Usuario>
            {
                new Usuario
                {
                    IdUsuario = 1,
                    Nombre = "Jose",
                    Email = "jose@test.com",
                    Password = "123",
                    Telefono = 99989769,
                    Activo = true
                }
            };

            // Crear un DbSet mock que soporte consultas y Find
            var dbSetMock = CreateDbSetMock<Usuario>(usuarios.AsQueryable());
            dbSetMock.Setup(d => d.Find(It.IsAny<object[]>())).Returns<object[]>(ids =>
            {
                var id = Convert.ToInt32(ids[0]);
                return usuarios.FirstOrDefault(u => u.IdUsuario == id);
            });

            // Reemplazamos el DbSet del contexto (Setup compartido puede ser sobrescrito)
            contextFake.SetupGet(c => c.Usuarios).Returns(dbSetMock.Object);
            contextFake.Setup(c => c.SaveChanges()).Returns(1);

            var service = new GestionUsuarios(contextFake.Object, loggerMock!.Object, seguridadMock!.Object, validadoresMock!.Object);

            // Act: modificamos usuario mediante el servicio
            var usuarioMod = new Usuario
            {
                IdUsuario = 1,
                Nombre = "Jose Modificado",
                Email = usuarios[0].Email,
                Password = usuarios[0].Password,
                Telefono = usuarios[0].Telefono,
                Activo = usuarios[0].Activo
            };

            service.UpdateUser(usuarioMod);

            // Assert: comprobar que el cambio persiste en la lista simulada
            var actualizado = usuarios.First(u => u.IdUsuario == 1);
            Assert.IsNotNull(actualizado);
            Assert.AreEqual("Jose Modificado", actualizado.Nombre);
        }

        [Test]
        public void AddUser_AgregaUsuarioCorrectamente()
        {
            // Arrange: lista vacía que representará la tabla y DbSet mock
            var usuarios = new List<Usuario>();
            var usuariosDbSet = GetQueryableMockDbSet<Usuario>(usuarios);

            // Reemplazamos el DbSet del contexto configurado en Setup
            contextFake.SetupGet(c => c.Usuarios).Returns(usuariosDbSet);
            contextFake.Setup(c => c.SaveChanges()).Returns(1);

            var service = new GestionUsuarios(contextFake.Object, loggerMock!.Object, seguridadMock!.Object, validadoresMock!.Object);

            var nuevo = new Usuario { IdUsuario = 5, Nombre = "Ana", Email = "ana@test.com" };

            // Act
            var agregado = service.AddUser(nuevo);

            // Assert
            Assert.IsNotNull(agregado);
            Assert.AreEqual(1, usuarios.Count);
            //Assert.That(dusuFake, Is.EqualTo(dusu));
            Assert.AreEqual("Ana", usuarios[0].Nombre);
        }

        [Test]
        public void GetAllUsers_DevuelveTodosLosUsuarios()
        {
            // Arrange: crear lista con dos usuarios
            var usuarios = new List<Usuario>
            {
                new Usuario { IdUsuario = 1, Nombre = "A" },
                new Usuario { IdUsuario = 2, Nombre = "B" }
            };

            var dbSetMock = CreateDbSetMock<Usuario>(usuarios.AsQueryable());
            contextFake.SetupGet(c => c.Usuarios).Returns(dbSetMock.Object);

            var service = new GestionUsuarios(contextFake.Object, loggerMock!.Object, seguridadMock!.Object, validadoresMock!.Object);

            // Act
            var resultado = service.GetAllUsers().ToList();

            // Assert
            Assert.AreEqual(2, resultado.Count);
        }

        [Test]
        public void GetUser_DevuelveUsuarioPorId()
        {
            // Arrange: usuario concreto
            var usuarios = new List<Usuario>
            {
                new Usuario { IdUsuario = 10, Nombre = "X" }
            };

            var dbSetMock = CreateDbSetMock<Usuario>(usuarios.AsQueryable());
            contextFake.SetupGet(c => c.Usuarios).Returns(dbSetMock.Object);

            var service = new GestionUsuarios(contextFake.Object, loggerMock!.Object, seguridadMock!.Object, validadoresMock!.Object);

            // Act
            var usuario = service.GetUser(10);

            // Assert
            Assert.IsNotNull(usuario);
            Assert.AreEqual("X", usuario.Nombre);
        }

        [Test]
        public void DeleteUser_EliminaUsuarioCorrectamente()
        {
            // Arrange: lista con un usuario a eliminar
            var usuarios = new List<Usuario>
            {
                new Usuario { IdUsuario = 20, Nombre = "ToDelete" }
            };

            var dbSetMock = CreateDbSetMock<Usuario>(usuarios.AsQueryable());
            // Al eliminar del DbSet mock actualizamos la lista real
            dbSetMock.Setup(d => d.Remove(It.IsAny<Usuario>())).Callback<Usuario>(u => usuarios.Remove(u));

            contextFake.SetupGet(c => c.Usuarios).Returns(dbSetMock.Object);

            var service = new GestionUsuarios(contextFake.Object, loggerMock!.Object, seguridadMock!.Object, validadoresMock!.Object);

            // Act
            var res = service.DeleteUser(20);

            // Assert
            Assert.IsTrue(res);
            Assert.AreEqual(0, usuarios.Count);
        }
    }
}
