using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Exceptions
{
    /// <summary>
    /// Exception thrown when a type of postal item (e.g. postcard) is not supported for a particular service of region
    /// </summary>
    public class UnsupportedPostItemException : Exception
    {
        public UnsupportedPostItemException() { }

        public UnsupportedPostItemException(string message) : base(message) { }

        public UnsupportedPostItemException(string message, Exception inner) : base(message, inner) { }
    }
}
