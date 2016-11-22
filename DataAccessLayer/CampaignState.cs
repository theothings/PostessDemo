namespace DataAccessLayer
{
    /// <summary>
    /// The state of the campaign in the different stages of its lifetime
    /// Used to determine if the campaigns can be forwarded to the relevent 3rd party service
    /// </summary>
    public class CampaignState
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public const int DefaultState = 1;
        public const int PriceSet = 2;
        public const int Confirmed = 3;
        public const int AttemptingCharge = 4;
        public const int ChargedSuccessful = 5;
        public const int ChargeFailed = 6;
        public const int DeliveryInProgress = 7;
        public const int DeliveryComplete = 8;
        public const int DeliveryFailed = 9;
    }
}