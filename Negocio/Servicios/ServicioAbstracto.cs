using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios
{
    public abstract class ServicioAbstracto<T> where T : DbContext
    {
        protected ILogger logger;

        public T Context { get; internal set; }

        public Usuario? CurrentCalipsoforUser { get; internal set; }

        protected ServicioAbstracto(T context, ILogger milogger) {     
            this.Context = context;
            this.logger = milogger;
        }

        protected EventId GetEventId() {
            return new EventId((GetType().FullName ?? "Servicio").GetHashCode(),GetType().FullName);
        }
    }
}
