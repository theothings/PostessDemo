using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PostService.PostItems
{
    /// <summary>
    /// Letter PostITem to be sent by underlying service
    /// </summary>
    class Letter : PostItem
    {
        [Required]
        new public Address FromAddress { get; set; }

        public bool IsColor = true;
    }
}
