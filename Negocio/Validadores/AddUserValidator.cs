using FluentValidation;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Validadores.Comun;

namespace Negocio.Validadores
{
    /// <summary>
    /// Clase que proporciona reglas de validacion para añadir un usuario.
    /// </summary>
    /// <remarks>This validator ensures that the required fields for a user are provided and meet specific
    public class AddUserValidator : AbstractValidador<Usuario>
    {
        private readonly EpsilonDbContext _contextDB;

        /// <summary>
        /// Constructor del validador de alta de usuario
        /// </summary>
        /// <param name="contextDB"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AddUserValidator(EpsilonDbContext contextDB)
        {
            if (contextDB == null)
            {
                throw new ArgumentNullException();
            }

            _contextDB = contextDB;

            // Reglas simples
            RuleFor(u => u.Nombre).NotEmpty().WithMessage("El nombre del usuario es obligatorio.<br/>");
            RuleFor(u => u.Password).NotEmpty().WithMessage("El password es obligatorio.<br/>").MaximumLength(9).WithMessage("No mas de 9 cifras. <br />");
            RuleFor(u => u.Email).NotEmpty().WithMessage("El email del usuario es obligatorio.<br/>");
            RuleFor(u => u.FechaAlta).NotEmpty().WithMessage("La fecha de alta es obligatoria del usuario es obligatorio.<br/>");

            //Reglas complejas
            //No Duplicados
            RuleFor(u => u.IdUsuario)
                .Custom((usuario, contextDB) =>
                {
                    var user = contextDB.InstanceToValidate;
                    //var usuarioq = context.Usuarios.Where(u => u.IdUsuario == user.IdUsuario);

                    //if (user.IdUsuario == usuarioq.idUsuario)
                    //{
                    //    contextDB.AddFailure($"Usuario ya sado de alta");
                    //}
                });
        }

        /// <summary>
        /// Metodo encargado de obtener la operacion correspondiente
        /// </summary>
        /// <returns></returns>
        public override string GetOperacion()
        {
            return OperacionesValidacion.OPERACION_INSERTAR;
        }
    }
}