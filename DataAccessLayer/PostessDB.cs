namespace DataAccessLayer
{
    using Helpers;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class PostessDB : IdentityDbContext<ApplicationUser>, IPostessDB
    {
        // Your context has been configured to use a 'PostessDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DataAccessLayer.PostessDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'PostessDB' 
        // connection string in the application configuration file.
        public PostessDB()
            : base("name=PostessDB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<LockedTemplate> LockedTemplates { get; set; }
        public virtual DbSet<SentItem> SentItems { get; set; }
        public virtual DbSet<TemplateDesign> TemplateDesigns { get; set; }
        public virtual DbSet<RecipientProfile> RecipientProfiles { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<SentItemState> SentItemStates { get; set; }
        public virtual DbSet<PaymentAccount> PaymentAccount { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public virtual DbSet<CampaignState> CampaignStates { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<ProfileList> ProfileLists { get; set; }
        public virtual DbSet<Price> Prices { get; set; }


        /// <summary>
        /// Create the sentItems (locked infomation on price and content about the campaign before confirmation and payment)
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public bool GenerateCampaignSentItems(Campaign campaign)
        {
            if (campaign.CampaignStateId != CampaignState.DefaultState)
            {
                return false;
            }

            // Get all the profiles from the campaigns list
            var recipientProfiles = this.RecipientProfiles.Where(p => p.ProfileListId == campaign.ProfileListId);
            var template = this.LockedTemplates.Find(campaign.LockedTemplateId);

            // Remove all existing sentItems
            campaign.SentItems.Clear();

            // foreach recipient loop over and create sentItems for each, using the template
            foreach(var profile in recipientProfiles)
            {
                SentItem sentItem = this.CreateSentItem(template, profile);
                campaign.SentItems.Add(sentItem);
            }

            campaign.CampaignStateId = CampaignState.PriceSet;

            // Update the price of the campaign
            campaign.SetCampaignPrice();

            this.SetModified(campaign);
            this.SaveChanges();

            return true;
            
        }

        private SentItem CreateSentItem(LockedTemplate template, RecipientProfile profile)
        {
            SentItem sentItem = new SentItem();

            // Deep copy the relevant profile into a new profile
            sentItem.RecipientProfile = new RecipientProfile( profile );

            // Lock the profiles as a sent item so it cannot be modified
            sentItem.RecipientProfile.IsProfileLocked = true;

            // Parse the template with profile info
            TemplateParser parser = new TemplateParser();
            sentItem.FrontHtml = parser.ParseTemplate(template.FrontHtml, profile);
            sentItem.BackHtml = parser.ParseTemplate(template.BackHtml, profile);
            sentItem.ItemTypeId = template.ItemTypeId;

            // Get the price of the mail item
            sentItem.LockedInPrice = this.Prices.Where(p => p.CountryId == profile.CountryId && p.ItemTypeId == template.ItemTypeId).FirstOrDefault().Ammount;
            
            return sentItem;
        }
        
        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using modelBuilder here
            modelBuilder.Entity<Campaign>()
                .HasRequired(c => c.Account)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Campaign>()
                .HasRequired(c => c.LockedTemplate)
                .WithMany()
                .WillCascadeOnDelete(false);


            base.OnModelCreating(modelBuilder);
        }
    }
}