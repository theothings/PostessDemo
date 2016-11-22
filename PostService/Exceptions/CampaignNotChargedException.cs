using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Exceptions
{
    /// <summary>
    /// Exception thrown when a campaign fails to be charged
    /// </summary>
    public class CampaignNotChargedException : Exception
    {
        public CampaignNotChargedException() { }

        public CampaignNotChargedException(string message) : base(message) { }

        public CampaignNotChargedException(string message, Exception inner) : base(message, inner) { }
    }
}
