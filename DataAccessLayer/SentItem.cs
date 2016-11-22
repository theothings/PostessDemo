using System.Collections;
using System.Collections.Generic;

namespace DataAccessLayer
{
    /// <summary>
    /// The SentItem model stores all static infomation about a post item to be delivered for audit and processing
    /// This cannot be modified once a campaign is sent
    /// </summary>
    public class SentItem
    {
        public SentItem()
        {
            this.SentItemStateId = SentItemState.DefaultState;
        }

        public int Id { get; set; }
        public string FrontHtml { get; set; }
        public string BackHtml { get; set; }
        public string PreviewImage { get; set; }
        public int AttemptsToSendCount { get; set; }
        public virtual RecipientProfile RecipientProfile { get; set; }
        public virtual RecipientProfile FromRecipient { get; set; }
        public int SentItemStateId { get; set; }
        public virtual SentItemState SentItemState { get; set; }
        public int ItemTypeId { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public int LockedInPrice { get; set; }

    }
}