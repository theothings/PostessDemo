using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    // Modeul representing data submited by user when creating and viewing a campaign
    public class CampaignBindingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// List of addresses for the campaign
        /// </summary>
        public virtual ProfileList ProfileList { get; set; }

        /// <summary>
        /// A copy of the template used when the campaign is finalised
        /// </summary>
        public virtual LockedTemplate LockedTemplate { get; set; }
        
    }
}