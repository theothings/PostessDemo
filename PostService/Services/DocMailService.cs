using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostService.PostItems;

namespace PostService.Services
{
    /// <summary>
    /// Service implementation for DocMail API for sending post programatically
    /// </summary>
    public class DocMailService : IPostService
    {
        private string ApiKeySecret;
        private string ApiKeyPublic;
        

        public DocMailService(string apiKeyPublic, string apiKeySecret)
        {
            this.ApiKeyPublic = apiKeyPublic;
            this.ApiKeySecret = apiKeySecret;
            
            // Initialise Docmail API
        }

        public string CorrecttAddress(string address)
        {
            // Use third part address verification service
            throw new NotImplementedException();
        }

        public void SendPost(IPostItem postItem)
        {
            // Use docail to send the HTML or image form postItem
            throw new NotImplementedException();
        }

        public bool ValidateAddress(string address)
        {
            // Use third party to validate address is valid
            throw new NotImplementedException();
        }
        
        public bool ValidatePostItem(IPostItem postItem, out string message)
        {
            throw new NotImplementedException();
        }
    }
}
