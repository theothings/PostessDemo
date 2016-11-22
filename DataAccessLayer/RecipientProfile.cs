using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DataAccessLayer
{
    /// <summary>
    /// Model for the different recipient address profiles which exist
    /// </summary>
    [DataContract]
    public class RecipientProfile
    {
        public RecipientProfile()
        {
            this.CountryId = Country.US;
            this.IsProfileLocked = false;
        }

        public RecipientProfile(RecipientProfile profile)
        {
            this.Name = profile.Name;
            this.AddressLine1 = profile.AddressLine1;
            this.AddressLine2 = profile.AddressLine2;
            this.City = profile.City;
            this.State = profile.State;
            this.AreaCode = profile.AreaCode;
            this.CountryId = profile.CountryId;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string AddressLine1 { get; set; }

        [DataMember]
        public string AddressLine2 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string AreaCode { get; set; }

        [DataMember]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public bool IsProfileLocked { get; set; }
        
        public DateTime? DateCreated { get; set; }
        public int? ProfileListId { get; set; }
        public virtual ProfileList ProfileList { get; set; }
    }
}