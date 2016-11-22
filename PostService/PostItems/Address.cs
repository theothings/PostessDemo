using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PostService.PostItems
{
    /// <summary>
    /// Model used for the address for any service
    /// </summary>
    public class Address
    {
        [Required]
        public string Name;

        [Required, MaxLength(200)]
        public string AddressLine1;

        [MaxLength(200)]
        public string AddressLine2;

        [Required, MaxLength(200)]
        public string City;
        
        [Required, MaxLength(200)]
        public string State;
        
        [Required, MaxLength(40)]
        public string AreaCode;

        [Required, MaxLength(2)]
        public string Country;

    }
}
