using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostService.PostItems;
using PostService.LobAPI;
using PostService.Exceptions;
using PostService.Properties;
using System.ComponentModel.DataAnnotations;

namespace PostService.Services
{
    /// <summary>
    /// Service implentation for sending post programmaticaly though Lob in the USA
    /// </summary>
    public class LobService : IPostService
    {
        private Lob lobClient;

        public LobService(string apiKey)
        {
            this.lobClient = new Lob(apiKey);
        }

        public void SendPost(IPostItem postItem)
        {

            // Send the lob item depending on the post type
            switch (postItem.GetType().Name)
            {
                // Send a postcard though LOB API
                case "PostCard":
                    SendPostCard((PostCard)postItem);
                    break;

                // Send a letter though the LOB API
                case "Letter":
                    SendLetter((Letter)postItem);
                    break;

                // Throw an unsupported post item exception for item types which are not handled
                default:
                    throw new UnsupportedPostItemException();
            }

        }

        public bool ValidatePostItem(IPostItem postItem, out string message)
        {
            var validationContext = new ValidationContext(postItem, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = false;

            // Send the lob item depending on the post type
            switch (postItem.GetType().Name)
            {
                // Send a postcard though LOB API
                case "PostCard":
                    isValid = Validator.TryValidateObject((PostCard)postItem, validationContext, validationResults);
                    message = validationResults[0].ToString();
                    return isValid;

                // Send a letter though the LOB API
                case "Letter":
                    isValid = Validator.TryValidateObject((PostCard)postItem, validationContext, validationResults);
                    message = validationResults[0].ToString();
                    return isValid;

                // Throw an unsupported post item exception for item types which are not handled
                default:
                    message = "This item cannot be sent in this country";
                    return false;
            }
        }

        private void SendPostCard(PostCard postcard)
        {
            var response = lobClient.Postcards.Create(new
            {
                to = new
                {
                    name = postcard.ToAddress.Name,
                    address_line1 = postcard.ToAddress.AddressLine1,
                    address_line2 = postcard.ToAddress.AddressLine2,
                    address_country = postcard.ToAddress.Country,
                    address_city = postcard.ToAddress.City,
                    address_state = postcard.ToAddress.State,
                    address_zip = postcard.ToAddress.AreaCode
                },
                front = postcard.FrontHtml,
                back = postcard.BackHtml,
                size = "4x6"
            });
        }

        private void SendLetter(Letter letter)
        {
            var response = lobClient.Letters.Create(new
            {
                to = new
                {
                    name = letter.ToAddress.Name,
                    address_line1 = letter.ToAddress.AddressLine1,
                    address_line2 = letter.ToAddress.AddressLine2,
                    address_country = letter.ToAddress.Country,
                    address_city = letter.ToAddress.City,
                    address_state = letter.ToAddress.State,
                    address_zip = letter.ToAddress.AreaCode
                },
                from = new
                {
                    name = letter.FromAddress.Name,
                    address_line1 = letter.FromAddress.AddressLine1,
                    address_line2 = letter.FromAddress.AddressLine2,
                    address_country = letter.FromAddress.Country,
                    address_city = letter.FromAddress.City,
                    address_state = letter.FromAddress.State,
                    address_zip = letter.FromAddress.AreaCode
                },
                color = letter.IsColor.ToString().ToLower(),
                file = letter.FrontHtml,
                insert_blank_page = true
            });
        }

    }
}
