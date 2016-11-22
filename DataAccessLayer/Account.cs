using System.Collections.Generic;

namespace DataAccessLayer
{
    /// <summary>
    /// Account model for the users account
    /// There is a distinction between user and account for the ability for an account to be managed by multiple users
    /// </summary>
    public class Account
    {
        public int Id { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<PaymentAccount> PaymentAccounts { get; set; }
        public virtual PaymentAccount DefaultPaymentAccount { get; set; }
        public virtual ICollection<TemplateDesign> TemplateDesigns { get; set; }
        public virtual ICollection<ProfileList> ProfileLists { get; set; }
    }
}