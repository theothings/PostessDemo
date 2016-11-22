namespace DataAccessLayer
{
    /// <summary>
    /// The model for the different payment gateways and services supported (currently just stripe)
    /// </summary>
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public const int Stripe = 1;
    }
}