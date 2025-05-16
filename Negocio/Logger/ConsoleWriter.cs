using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Logger
{
    public class ConsoleWriter : ILoggerWriter
    {
        public ConsoleWriter() { }

        public void Write(string msg) { 
            Debug.WriteLine(msg);
        }

        public void Write(string user, string msg)
        {
            Write(msg);
        }

        public void Write(string id, string user, string msg)
        {
            Write(msg);
        }

        public void Write(string id, string user, string msg, string data)
        {
            Write(msg);
        }
    }
}
