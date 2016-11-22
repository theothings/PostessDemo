using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataAccessLayer
{
    /// <summary>
    /// Interface for the database context, allowing for mocking
    /// </summary>
    public interface IPostessDB : IDisposable
    {
        DbSet<Account> Accounts { get; }
        DbSet<Campaign> Campaigns { get; }
        DbSet<LockedTemplate> LockedTemplates { get; }
        DbSet<SentItem> SentItems { get; }
        DbSet<TemplateDesign> TemplateDesigns { get; }
        DbSet<RecipientProfile> RecipientProfiles { get; }
        DbSet<Country> Countries { get; }
        DbSet<ItemType> ItemTypes { get; }
        DbSet<SentItemState> SentItemStates { get; }
        DbSet<PaymentAccount> PaymentAccount { get; }
        DbSet<PaymentTransaction> PaymentTransactions { get; }
        DbSet<CampaignState> CampaignStates { get; }
        DbSet<PaymentMethod> PaymentMethods { get; }
        DbSet<Currency> Currencies { get; }
        DbSet<ProfileList> ProfileLists { get; }
        DbSet<Price> Prices { get; }

        bool GenerateCampaignSentItems(Campaign campaign);
        void SetModified(object entity);
        int SaveChanges();
    }
}
