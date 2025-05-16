using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Logger
{
    public interface ILoggerWriter
    {
        void Write(string msg);
        void Write(string user, string msg);
        void Write(string id, string user, string msg);
        void Write(string id, string user, string msg, string data);
    }
}
