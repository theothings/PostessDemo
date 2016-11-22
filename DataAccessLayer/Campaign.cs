using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// Campaign model for storing groups of letter bundles
    /// </summary>
    [DataContract]
    public class Campaign
    {
        public Campaign()
        {
            this.CampaignStateId = CampaignState.DefaultState;
            this.IsDeleted = false;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public int AccountId { get; set; }

        /// <summary>
        /// The account for the campaign
        /// </summary>
        public virtual Account Account { get; set; }
        
        public int ProfileListId { get; set; }

        /// <summary>
        /// List of addresses for the campaign
        /// </summary>
        [DataMember]
        public virtual ProfileList ProfileList { get; set; }

        public int LockedTemplateId { get; set; }

        /// <summary>
        /// A copy of the template used when the campaign is finalised
        /// </summary>
        [DataMember]
        public virtual LockedTemplate LockedTemplate { get; set; }

        /// <summary>
        /// A fixed version of the items for each recipient to be delivered
        /// </summary>
        public virtual ObservableCollection<SentItem> SentItems { get; set; }

        public int CampaignStateId { get; set; }

        /// <summary>
        /// The state of the campaign
        /// </summary>
        public virtual CampaignState CampaignState { get; set; }

        /// <summary>
        /// Priority determines when in the que the campaign can be processed (used for future use)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Campaigns are never deleted, set this to true when a campaign is deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Expected date campaign should be delivered
        /// </summary>
        public DateTime? TargetDeliveryDate { get; set; }

        /// <summary>
        /// Date which the campaign was started to process
        /// </summary>
        public DateTime? DateProcessingStarted { get; set; }

        /// <summary>
        /// Date which the campaign finished sending
        /// </summary>
        public DateTime? DateProcessingCompleted { get; set; }

        /// <summary>
        /// The price of the campaign to be charged to the customer in USD cents
        /// </summary>
        public int PriceOfCampaign { get; set; }

        public DateTime? DateCharged { get; set; }

        public bool CanEditCampaign()
        {
            if (this.CampaignStateId == CampaignState.DefaultState || this.CampaignStateId == CampaignState.PriceSet)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Implement to set the campaign price
        /// </summary>
        public int SetCampaignPrice()
        {
            // This method should look up values in the table based on price for different items and regions
            int priceOfCampaign = this.SentItems.Sum(s => s.LockedInPrice);
            // For testing, use a static value of 50, in reality get this from a database table for each item
            this.PriceOfCampaign = priceOfCampaign;
            return this.PriceOfCampaign;
        }
    }
}