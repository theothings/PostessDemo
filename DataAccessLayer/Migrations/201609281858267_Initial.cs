namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefaultPaymentAccount_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentAccounts", t => t.DefaultPaymentAccount_Id)
                .Index(t => t.DefaultPaymentAccount_Id);
            
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        IsDeleted = c.Int(nullable: false),
                        TargetDeliveryDate = c.DateTime(nullable: false),
                        DateProcessingStarted = c.DateTime(nullable: false),
                        DateProcessingCompleted = c.DateTime(nullable: false),
                        CampaignState_Id = c.Int(),
                        LockedTemplate_Id = c.Int(),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignStates", t => t.CampaignState_Id)
                .ForeignKey("dbo.LockedTemplates", t => t.LockedTemplate_Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.CampaignState_Id)
                .Index(t => t.LockedTemplate_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.CampaignStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LockedTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HtmlTemplate = c.String(),
                        PreviewImage = c.String(),
                        PreviewPdf = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecipientProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Campaign_Id = c.Int(),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.Campaign_Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Campaign_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.SentItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        Pdf = c.String(),
                        Html = c.String(),
                        PreviewImage = c.String(),
                        AttemptsToSendCount = c.Int(nullable: false),
                        SentItemState_Id = c.Int(),
                        Campaign_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SentItemStates", t => t.SentItemState_Id)
                .ForeignKey("dbo.Campaigns", t => t.Campaign_Id)
                .Index(t => t.SentItemState_Id)
                .Index(t => t.Campaign_Id);
            
            CreateTable(
                "dbo.SentItemStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.TemplateDesigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HtmlTemplateDesign = c.String(),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.PaymentTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EmailAddress = c.String(),
                        Password = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DefaultAccount_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.DefaultAccount_Id)
                .Index(t => t.DefaultAccount_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "DefaultAccount_Id", "dbo.Accounts");
            DropForeignKey("dbo.TemplateDesigns", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.RecipientProfiles", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.PaymentAccounts", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "DefaultPaymentAccount_Id", "dbo.PaymentAccounts");
            DropForeignKey("dbo.Campaigns", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.SentItems", "Campaign_Id", "dbo.Campaigns");
            DropForeignKey("dbo.SentItems", "SentItemState_Id", "dbo.SentItemStates");
            DropForeignKey("dbo.RecipientProfiles", "Campaign_Id", "dbo.Campaigns");
            DropForeignKey("dbo.Campaigns", "LockedTemplate_Id", "dbo.LockedTemplates");
            DropForeignKey("dbo.Campaigns", "CampaignState_Id", "dbo.CampaignStates");
            DropIndex("dbo.Users", new[] { "DefaultAccount_Id" });
            DropIndex("dbo.TemplateDesigns", new[] { "Account_Id" });
            DropIndex("dbo.PaymentAccounts", new[] { "Account_Id" });
            DropIndex("dbo.SentItems", new[] { "Campaign_Id" });
            DropIndex("dbo.SentItems", new[] { "SentItemState_Id" });
            DropIndex("dbo.RecipientProfiles", new[] { "Account_Id" });
            DropIndex("dbo.RecipientProfiles", new[] { "Campaign_Id" });
            DropIndex("dbo.Campaigns", new[] { "Account_Id" });
            DropIndex("dbo.Campaigns", new[] { "LockedTemplate_Id" });
            DropIndex("dbo.Campaigns", new[] { "CampaignState_Id" });
            DropIndex("dbo.Accounts", new[] { "DefaultPaymentAccount_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Regions");
            DropTable("dbo.PaymentTransactions");
            DropTable("dbo.TemplateDesigns");
            DropTable("dbo.PaymentAccounts");
            DropTable("dbo.SentItemStates");
            DropTable("dbo.SentItems");
            DropTable("dbo.RecipientProfiles");
            DropTable("dbo.LockedTemplates");
            DropTable("dbo.CampaignStates");
            DropTable("dbo.Campaigns");
            DropTable("dbo.Accounts");
        }
    }
}
