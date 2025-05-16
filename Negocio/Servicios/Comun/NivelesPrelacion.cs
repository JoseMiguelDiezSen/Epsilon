using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios.Comun
{
    public enum NivelesPrelacion
    {
        Basico = 0,
        Operador = 25,
        Avanzado = 50,
        Supervisor = 100,
        Gestor = 150,
        Planificador = 175,
        Director = 200,
        Administrador = 255
    }
}
