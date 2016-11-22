using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Exceptions
{
    /// <summary>
    /// When mail is sent to a country which is not supported
    /// </summary>
    public class UnsupportedCountryException : Exception
    {
        public UnsupportedCountryException() { }

        public UnsupportedCountryException(string message) : base(message) { }

        public UnsupportedCountryException(string message, Exception inner) : base(message, inner) { }
    }
}
