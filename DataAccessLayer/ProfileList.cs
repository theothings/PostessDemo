using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// ProfileList is the grouping for different address profiles to be delivered to
    /// </summary>
    [DataContract]
    public class ProfileList
    {
        public ProfileList()
        {
            this.IsDeleted = false;
            this.IsViewable = true;
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual ICollection<RecipientProfile> RecipientProfiles { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public bool IsDeleted { get; set; }
        
        /// <summary>
        /// Non viewable lists, do not show up for users when viewing their lists, they are used for when
        /// lists are generated dynamically on the fly for a campaign, and shouldn't be seen
        /// </summary>
        public bool IsViewable { get; set; }
    }
}
