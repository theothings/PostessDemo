using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// Model for the price of individual items for different regions
    /// </summary>
    public class Price
    {
        public int Id { get; set; }
        public int ItemTypeId { get; set; }
        public virtual ItemType ItemType { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int Ammount { get; set; }
    }
}
