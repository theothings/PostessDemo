using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models
{
    /// <summary>
    /// Data submitted by the front end when registering a stripe credit card
    /// </summary>
    public class StripeBindingModel
    {
        [Required]
        public string CardToken { get; set; }
    }
}