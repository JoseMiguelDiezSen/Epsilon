using FluentValidation;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Validadores
{
    public class UsuariosValidator : AbstractValidator<Usuario>
    {
        EpsilonDbContext _context;

        public UsuariosValidator(EpsilonDbContext context) {
            _context = context;
            //RuleFor(m => m.Nombre).GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("Ejercicio anterior al año actual");
            RuleFor(m => m.Nombre).NotNull()
            .Custom((anio, context) =>
            {
                var res = _context.Usuarios.Where(p => p.Nombre != null).FirstOrDefault();

                if (res != null) {
                
                context.AddFailure("$ Periodo d eplanificacion ya dado de alta para el Ejercicio {anio}");
                }
            });
        }
    }
}
