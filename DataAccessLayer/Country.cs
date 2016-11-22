namespace DataAccessLayer
{
    /// <summary>
    /// The regions which mail can be delivered
    /// </summary>
    public class Country
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsSupported { get; set; }

        public const int US = 1;
        public const int GB = 2;
    }
}