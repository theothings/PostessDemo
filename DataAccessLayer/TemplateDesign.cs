namespace DataAccessLayer
{
    /// <summary>
    /// Model for the Template designs for post delivery items
    /// This is shared by all users
    /// </summary>
    public class TemplateDesign
    {
        public int Id { get; set; }
        public string HtmlTemplateDesign { get; set; }
        public string HtmlTemplatePreviewData { get; set; }
        public bool IsDeleted { get; set; }
    }
}