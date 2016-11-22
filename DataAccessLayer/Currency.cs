using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// The currencies which are supported by the platform
    /// </summary>
    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public const int DefaultCurrency = 2;
        public const int USD = 1;
        public const int GBP = 2;
    }
}
