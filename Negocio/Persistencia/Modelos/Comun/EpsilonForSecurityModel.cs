using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos.Comun
{
    public abstract class EpsilonForSecurityModel: EpsilonForModel, ICalipsoforSecurityModel
    {
        public long IdEntidad => GetMiIdEntidad();
        public int IdTipo => GetMiIdTipo();

        protected abstract long GetMiIdEntidad();

        protected abstract int GetMiIdTipo();


    }
}
