namespace DataAccessLayer
{
    /// <summary>
    /// The type of post items which can be delivered
    /// </summary>
    public class ItemType
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public const int PostCard = 1;
        public const int Letter = 2;
    }
}