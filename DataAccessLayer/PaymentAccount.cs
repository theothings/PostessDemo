using System;

namespace DataAccessLayer
{
    /// <summary>
    /// The model for storing payment infomation
    /// </summary>
    public class PaymentAccount
    {
        public PaymentAccount(int paymentMethodId, string token)
        {
            this.IsDeleted = false;

            switch (paymentMethodId)
            {
                case PaymentMethod.Stripe:
                    this.SetStripePayment(token);
                    break;
                default:
                    throw new Exception("Unrecognised payment method");
            }
        }

        public int Id { get; set; }
        public string StripeCustomerToken { get; set; }
        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public bool IsDeleted { get; set; }

        public void SetStripePayment(string stripeCustomerToken)
        {
            this.StripeCustomerToken = stripeCustomerToken;
            this.PaymentMethodId = PaymentMethod.Stripe;
        }
    }
}