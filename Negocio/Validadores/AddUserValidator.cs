using FluentValidation;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Validadores.Comun;

namespace Negocio.Validadores
{

    public class AddUserValidator : AbstractValidator<Usuario>
    {
        EpsilonDbContext _contextDB;

        public AddUserValidator(EpsilonDbContext contextDB)
        {
            if (contextDB == null)
            {
                throw new ArgumentNullException();
            }

            _contextDB = contextDB;

            // Reglas simples
            RuleFor(u => u.Nombre).NotEmpty()
                .WithMessage("El nombre del usuario es obligatorio.<br/>");
            RuleFor(u => u.Password).NotEmpty()
                .WithMessage("El password es obligatorio.<br/>")
                .MaximumLength(9).WithMessage("No mas de 9 cifras. <br />");
            RuleFor(u => u.Email).NotEmpty().WithMessage("El email del usuario es obligatorio.<br/>");
            RuleFor(u => u.FechaAlta).NotEmpty().WithMessage("La fecha de alta es obligatoria del usuario es obligatorio.<br/>");

            //Reglas complejas

            //No Duplicados
            RuleFor(u => u.IdUsuario)
                .Custom((usuario, contextDB) =>
                {
                    //var user = contextDB.InstanceToValidate;
                    //var usuarioq = contextDB.Us.Where(u => u.IdUsuario == user.IdUsuario);

                    //if (user.IdUsuario == usuarioq.idUsuario)
                    //{
                    //    contextDB.AddFailure($"Usuario ya sado de alta");
                    //}
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public override string GetOperacion()
        //{
        //    return OperacionesValidacion.OPERACION_INSERTAR;
        //}
    }
}