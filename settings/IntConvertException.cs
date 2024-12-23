using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar3.settings
{
    class IntConvertException : Exception
    {
        public IntConvertException():base() { }
        public IntConvertException(string message):base(message) { }
    }
}
