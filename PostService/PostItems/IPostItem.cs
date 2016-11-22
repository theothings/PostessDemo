using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.PostItems
{
    /// <summary>
    /// Interface for items to be delivered through an underlying service
    /// </summary>
    public interface IPostItem
    {
        /// <summary>
        /// The front of the item to be posted as HTML
        /// </summary>
        string FrontHtml { get; set; }

        /// <summary>
        /// The back of the item to be posted as HTML
        /// </summary>
        string BackHtml { get; set; }

        /// <summary>
        /// The address an item should be posted too
        /// </summary>
        Address ToAddress { get; set; }

        /// <summary>
        /// The address an item should be returned to
        /// </summary>
        Address FromAddress { get; set; }

        /// <summary>
        /// The Width of the posted item
        /// </summary>
        float Width { get; set; }

        /// <summary>
        /// The height of the posted item
        /// </summary>
        float Height { get; set; }
    }
}
