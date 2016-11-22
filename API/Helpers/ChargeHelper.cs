using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace API.Helpers
{
    /// <summary>
    /// Provides methods for charging stripe credit cards
    /// </summary>
    public class ChargeHelper : IDisposable
    {
        private IPostessDB db;

        public ChargeHelper()
        {
            this.db = new PostessDB();
        }

        /// <summary>
        /// Credit and charge a campaign
        /// </summary>
        /// <param name="campaignId"></param>
        public void ChargeCampaign(int campaignId)
        {
            Campaign campaign;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
            {
                // Get the price of the campaign
                campaign = db.Campaigns.Find(campaignId);
                campaign.SetCampaignPrice();
                // Set campaign state to PriceSet
                campaign.CampaignStateId = CampaignState.PriceSet;
                campaign.CampaignStateId = CampaignState.AttemptingCharge;
                db.SaveChanges();
            }

            // Charge the users account
            bool chargeSuccessful = ChargeAccount(campaignId);

            // Update the status of the campaign once completed
            
            campaign = db.Campaigns.Find(campaignId);

            // if successful, set the campaign state to ChargeSuccesful or failed
            if (chargeSuccessful)
            {
                // Set the campaign state to ChargeSuccessful
                campaign.DateCharged = DateTime.Now;
                campaign.CampaignStateId = CampaignState.ChargedSuccessful;
            }
            else
            {
                // Set the campaign state tChargeFailed
                campaign.CampaignStateId = CampaignState.ChargeFailed;
            }

            db.SaveChanges();
        }

        // Do the actual charging of a users account
        private bool ChargeAccount(int campaignId)
        {
            var campaign = db.Campaigns.Find(campaignId);
            int totalPrice = campaign.PriceOfCampaign;
            Currency currency = db.Currencies.Find(Currency.DefaultCurrency);
            PaymentAccount paymentAccount = campaign.Account.DefaultPaymentAccount;

            // Charge the account
            try
            {
                PaymentProcessor paymentProcessor = new PaymentProcessor();
                paymentProcessor.ChargePaymentAccount(paymentAccount, totalPrice, currency);
            }
            catch (Exception e)
            {
                return false;
            }

            // Add a transaction record
            var paymentTransaction = new PaymentTransaction();
            paymentTransaction.Ammount = totalPrice;
            paymentTransaction.CurrencyId = currency.Id;
            paymentTransaction.Campaign = campaign;
            paymentTransaction.PaymentAccount = paymentAccount;

            db.SaveChanges();

            return true;
        }

        public void Dispose()
        {
            this.db.Dispose();
        }

    }
}