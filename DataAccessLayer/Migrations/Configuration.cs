namespace DataAccessLayer.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccessLayer.PostessDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataAccessLayer.PostessDB context)
        {

            //  This method will be called after migrating to the latest version.

            // Set the static campaign state values
            context.CampaignStates.AddOrUpdate( c => c.Id,
                new CampaignState { Id = CampaignState.DefaultState, Description = "The default state of a campaign once its created" },
                new CampaignState { Id = CampaignState.PriceSet, Description = "The price for the campaign has been accurately calculated, for the ammount to charge" },
                new CampaignState { Id = CampaignState.Confirmed, Description = "The user has confirmed the payment and everything should be locked for editing" },
                new CampaignState { Id = CampaignState.AttemptingCharge, Description = "Attempting to charge for campaign to users account" },
                new CampaignState { Id = CampaignState.ChargedSuccessful, Description = "The campaign has been charged for succesfuly" },
                new CampaignState { Id = CampaignState.ChargeFailed, Description = "The attempt to charge the user has failed" },
                new CampaignState { Id = CampaignState.DeliveryInProgress, Description = "The campaign is currently being processed for delivery" },
                new CampaignState { Id = CampaignState.DeliveryComplete, Description = "The campaign has been sent succesfuly to the underlying services" },
                new CampaignState { Id = CampaignState.DeliveryFailed, Description = "The campaign was not sent successfuly to the underlying services" }
                );
            
            // Set the static region values
            context.Countries.AddOrUpdate(r => r.Id,
                new Country { Id = Country.US, Code = "US", Description = "United States of America", IsSupported = true },
                new Country { Id = Country.GB, Code = "GB", Description = "United Kingdom", IsSupported = false }
                );


            // Set the static item type values
            context.SentItemStates.AddOrUpdate(r => r.Id,
                new SentItemState { Id = SentItemState.DefaultState, Description = "Default state when created" },
                new SentItemState { Id = SentItemState.SendingMail, Description = "In process of sending mail for an item" },
                new SentItemState { Id = SentItemState.MailSent, Description = "Mail sent successfully" },
                new SentItemState { Id = SentItemState.MailSendingError, Description = "Error sending mail" }
                );

            // Set the static item type values
            context.ItemTypes.AddOrUpdate(r => r.Id,
                new ItemType { Id = ItemType.PostCard, Description = "Postcard format" },
                new ItemType { Id = ItemType.Letter, Description = "Letter format" }
                );

            // Set the static payment method values
            context.PaymentMethods.AddOrUpdate(r => r.Id,
                new PaymentMethod { Id = PaymentMethod.Stripe, Description = "Stripe, using customer token" }
                );

            // Set the static currency values
            context.Currencies.AddOrUpdate(r => r.Id,
                new Currency { Id = Currency.USD, Code = "USD", Description = "Unite States Dollar" },
                new Currency { Id = Currency.GBP, Code = "GBP", Description = "British Pounds Sterling (UK)" }
                );
            

            // Set the static pricing values
            context.Prices.AddOrUpdate(p => new { p.Country, p.ItemType },
                // UK default prices in $usd
                new Price { CountryId = Country.GB, ItemTypeId = ItemType.Letter, Ammount = 62 },
                new Price { CountryId = Country.GB, ItemTypeId = ItemType.PostCard, Ammount = 53 },
                // USA default prices in $usd
                new Price { CountryId = Country.US, ItemTypeId = ItemType.Letter, Ammount = 95 },
                new Price { CountryId = Country.US, ItemTypeId = ItemType.PostCard, Ammount = 90 }
                );

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
