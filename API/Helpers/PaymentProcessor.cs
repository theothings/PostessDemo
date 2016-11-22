using DataAccessLayer;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Helpers
{
    /// <summary>
    /// Helper for processing payments by all payment gateways (Currently just stripe)
    /// </summary>
    public class PaymentProcessor
    {
        public bool ChargePaymentAccount(PaymentAccount paymentAccount, int ammount, Currency currency)
        {
            string currencyCode = currency.Code;

            switch(paymentAccount.PaymentMethodId)
            {
                // Use stripe.net to retrieve payment
                case PaymentMethod.Stripe:
                    return this.ChargeStripe(paymentAccount.StripeCustomerToken, ammount, currencyCode);
                default:
                    return false;
            }
        }

        private bool ChargeStripe(string stripeCustomerToken, int ammount, string currencyCode)
        {
            var charge = new StripeChargeCreateOptions();
            charge.Amount = ammount;
            charge.Currency = currencyCode.ToLower();
            charge.CustomerId = stripeCustomerToken;

            var chargeService = new StripeChargeService();
            var stripeCharge = chargeService.Create(charge);

            return stripeCharge.Paid;
        }
    }
}
