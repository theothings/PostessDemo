using System;
using System.Runtime.Serialization;

namespace DataAccessLayer
{
    /// <summary>
    /// The model for storing template for campaigns once the campaign is locked
    /// These templates cannot be modified
    /// </summary>
    [DataContract]
    public class LockedTemplate
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FrontHtml { get; set; }
        [DataMember]
        public string BackHtml { get; set; }
        public string PreviewImage { get; set; }
        public string PreviewPdf { get; set; }
        public DateTime? DateCreated { get; set; }
        public string HtmlTemplatePreviewData { get; set; }
        [DataMember]
        public int ItemTypeId { get; set; }
        public virtual ItemType ItemType { get; set; }
    }
}