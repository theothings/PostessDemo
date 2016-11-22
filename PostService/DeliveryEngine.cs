using DataAccessLayer;
using PostService.Exceptions;
using PostService.PostItems;
using PostService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace PostService
{
    /// <summary>
    /// Handles sending of post
    /// Determines which PostService to use based on address
    /// Completes the billing and state for pending messages
    /// </summary>
    public class DeliveryEngine
    {

        public DeliveryEngine()
        {
        }
        

        public void SendCampaign(int campaignId)
        {
            List<SentItem> sentItems = new List<SentItem>();

            using(var db = new PostessDB())
            {
                var campaign = db.Campaigns.Find(campaignId);

                // Ensure the campaign has been charged for
                if(campaign.CampaignStateId != CampaignState.ChargedSuccessful)
                {
                    throw new CampaignNotChargedException();
                }

                // This will need to be optimised for large campaigns with huge lists as it can't be handled in memory
                // Also optimise to not include items already sent, in processing, or failed to send
                sentItems = campaign.SentItems.ToList();

                campaign.CampaignStateId = CampaignState.DeliveryInProgress;
                campaign.DateProcessingStarted = DateTime.Now;

                db.SaveChanges();
            }

            try
            {
                // For each sentItem in the campaign, deliver it to the underlying service
                foreach(var sentItem in sentItems)
                {
                    // Delivery item to underlying service
                    this.SendItem(sentItem);
                }
            }
            catch (Exception e)
            {
                // Handle uncaught exception by setting delivery status to failed
                using (var db = new PostessDB())
                {
                    var campaign = db.Campaigns.Find(campaignId);
                    campaign.CampaignStateId = CampaignState.DeliveryFailed;
                    db.SaveChanges();
                }

                throw e;
            }

            // Update status to success
            using (var db = new PostessDB())
            {
                var campaign = db.Campaigns.Find(campaignId);
                campaign.DateProcessingCompleted = DateTime.Now;
                campaign.CampaignStateId = CampaignState.DeliveryComplete;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Method to send an item and control the state
        /// </summary>
        /// <param name="sentItem"></param>
        private void SendItem(SentItem sentItem)
        {
            // Get the service necessary for the given country
            IPostService postService = this.GetServiceForCountry(sentItem.RecipientProfile.CountryId);

            // populate the item into a sendItem
            IPostItem postItem = this.PopulateSendItem(sentItem);

            // Validate the item can be sent
            string outputMessage;
            if(postService.ValidatePostItem(postItem, out outputMessage))
            {
                // Update to sending item status to SendingMail
                using (var db = new PostessDB())
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
                {
                    var sentItemDB = db.SentItems.Find(sentItem.Id);

                    // Do not attempt to send if the item is not in a default state
                    if (sentItemDB.SentItemStateId == SentItemState.SendingMail
                        || sentItemDB.SentItemStateId == SentItemState.MailSent
                        || sentItemDB.SentItemStateId == SentItemState.MailSendingError
                        )
                    {
                        return;
                    }

                    sentItemDB.SentItemStateId = SentItemState.SendingMail;
                    db.SaveChanges();
                }

                try
                {
                    postService.SendPost(postItem);
                }
                catch(Exception e)
                {
                    // Update sending item status to MailSendingError
                    using (var db = new PostessDB())
                    {
                        // Mail sending error
                        var sentItemDB = db.SentItems.Find(sentItem.Id);
                        sentItemDB.SentItemStateId = SentItemState.MailSendingError;
                        db.SaveChanges();
                    }
                }

                // Update sending item status to SendingSuccess
                using (var db = new PostessDB())
                {
                    var sentItemDB = db.SentItems.Find(sentItem.Id);
                    sentItemDB.SentItemStateId = SentItemState.MailSent;
                    db.SaveChanges();
                }
            }
            else
            {
                // Process that this item could not be sent because of outputMessage
            }
            
        }

        /// <summary>
        /// Determine which underlying service to use depending on the country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        private IPostService GetServiceForCountry(int countryId)
        {
            switch (countryId)
            {
                case Country.US:
                    return new LobService(Properties.Settings.Default.LobApiKey);
                case Country.GB:
                    throw new UnsupportedCountryException();
                default:
                    throw new UnsupportedCountryException();
            }
        }

        /// <summary>
        /// Handle the populating of database model object to the relevant PostCard, Letter model for processing
        /// </summary>
        /// <param name="sentItem"></param>
        /// <returns></returns>
        private IPostItem PopulateSendItem(SentItem sentItem)
        {
            switch (sentItem.ItemTypeId)
            {
                case ItemType.PostCard:
                    return PopulatePostCardItem(sentItem);
                case ItemType.Letter:
                    return PopulateLetterItem(sentItem);
                default:
                    throw new UnsupportedPostItemException();
            }
        }

        /// <summary>
        /// Populate a stnadard sentItem model from database into a Postcard object
        /// </summary>
        /// <param name="sentItem"></param>
        /// <returns></returns>
        private PostCard PopulatePostCardItem(SentItem sentItem)
        {
            var postcard = new PostCard();
            postcard.FrontHtml = sentItem.FrontHtml;
            postcard.BackHtml = sentItem.BackHtml;
            postcard.ToAddress = this.GetAddressFromRecipientProfile(sentItem.RecipientProfile);
            postcard.FromAddress = this.GetAddressFromRecipientProfile(sentItem.FromRecipient);
            return postcard;
        }

        /// <summary>
        /// Transfer a sentItem model from database to a standard letter
        /// </summary>
        /// <param name="sentItem"></param>
        /// <returns></returns>
        private Letter PopulateLetterItem(SentItem sentItem)
        {
            var letter = new Letter();
            letter.FrontHtml = sentItem.FrontHtml;
            letter.BackHtml = sentItem.BackHtml;
            letter.ToAddress = this.GetAddressFromRecipientProfile(sentItem.RecipientProfile);
            letter.FromAddress = this.GetAddressFromRecipientProfile(sentItem.FromRecipient);
            return letter;
        }

        /// <summary>
        /// Get the address from a recipient profile model
        /// </summary>
        /// <param name="recipientProfile"></param>
        /// <returns></returns>
        private Address GetAddressFromRecipientProfile(RecipientProfile recipientProfile)
        {
            var address = new Address();
            address.Name = recipientProfile.Name;
            address.AddressLine1 = recipientProfile.AddressLine1;
            address.AddressLine2 = recipientProfile.AddressLine2;
            address.City = recipientProfile.City;
            address.State = recipientProfile.State;
            address.AreaCode = recipientProfile.AreaCode;
            address.Country = recipientProfile.Country.Code;

            return address;
        }
    }
}


