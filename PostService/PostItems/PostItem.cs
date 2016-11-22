using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PostService.PostItems
{
    /// <summary>
    /// PostItem represents a generic item to be sent though the post
    /// </summary>
    public class PostItem : IPostItem
    {
        [Required]
        public Address ToAddress { get; set; }

        public Address FromAddress { get; set; }
        
        public string BackHtml { get; set; }

        [Required]
        public string FrontHtml { get; set; }

        [Required]
        public float Height { get; set; }

        [Required]
        public float Width { get; set; }
    }
}
