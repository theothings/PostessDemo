namespace DataAccessLayer
{
    /// <summary>
    /// The state of an item which is awaiting delivery
    /// </summary>
    public class SentItemState
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public const int DefaultState = 1;
        public const int SendingMail = 2;
        public const int MailSent = 3;
        public const int MailSendingError = 4;
    }
}