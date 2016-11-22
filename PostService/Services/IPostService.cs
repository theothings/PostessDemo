using PostService.PostItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Services
{
    public interface IPostService
    {
        /// <summary>
        /// Send an item in the post using the underlying API
        /// </summary>
        /// <param name="postItem"></param>
        void SendPost(IPostItem postItem);

        /// <summary>
        /// Validate that a post item can be sent to the address
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns>true if postItem can be sent by the underlying API</returns>
        bool ValidatePostItem(IPostItem postItem, out string message);

    }
}
