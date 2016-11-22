using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccessLayer;
using API.Models;
using API.Helpers;

namespace API.Controllers
{

    /// <summary>
    /// Campaign controller for creating campaigns
    /// </summary>
    [Authorize]
    public class CampaignsController : BaseApiController
    {
        private IPostessDB db = new PostessDB();

        public CampaignsController() { }

        public CampaignsController(IPostessDB context)
        {
            db = context;
        }

        // GET: api/Campaigns
        public IQueryable<Campaign> GetCampaigns()
        {
            int accountId = this.GetAccountId();
            return db.Campaigns.Where(c => c.AccountId == accountId && c.IsDeleted == false);
        }

        // GET: api/Campaigns/5
        [ResponseType(typeof(Campaign))]
        public IHttpActionResult GetCampaign(int id)
        {
            int accountId = this.GetAccountId();
            Campaign campaign = db.Campaigns.Where(c => c.AccountId == accountId && c.Id == id).FirstOrDefault();
            if (campaign == null || campaign.IsDeleted == true)
            {
                return NotFound();
            }

            return Ok(campaign);
        }

        // PUT: api/Campaigns/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCampaign(int id, Campaign campaign)
        {
            int accountId = this.GetAccountId();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != campaign.Id || accountId != campaign.AccountId || !campaign.CanEditCampaign())
            {
                return BadRequest();
            }

            // Reset the price set state as it needs to be recalculated
            campaign.CampaignStateId = CampaignState.DefaultState;

            db.SetModified(campaign);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampaignExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Campaigns
        [ResponseType(typeof(Campaign))]
        public IHttpActionResult PostCampaign(Campaign campaign)
        {
            int accountId = this.GetAccountId();
            Account account = db.Accounts.Find(accountId);

            if (!ModelState.IsValid || campaign == null)
            {
                return BadRequest(ModelState);
            }

            campaign.AccountId = accountId;
            campaign.ProfileList.AccountId = accountId;
            account.Campaigns.Add(campaign);
            db.SetModified(account);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = campaign.Id }, campaign);
        }

        [HttpPost]
        [ActionName("ProcessCampaign")]
        [Route("api/Campaigns/ProcessCampaign")]
        public IHttpActionResult ProcessCampaign(int campaignId)
        {
            int accountId = this.GetAccountId();

            // @todo: do the following asynconously

            // Get the campaign requested, if owned by user and not deleted
            Campaign campaign = db.Campaigns.Where(c => c.AccountId == accountId && c.Id == campaignId).FirstOrDefault();

            if (campaign == null || campaign.IsDeleted == true || !campaign.CanEditCampaign())
            {
                return BadRequest();
            }

            this.db.GenerateCampaignSentItems(campaign);
            db.SetModified(campaign);

            db.SaveChanges();
            return Ok();
        }

        public IHttpActionResult ConfirmCampaign(int campaignId)
        {
            int accountId = this.GetAccountId();
            // Get the campaign requested, if owned by user and not deleted
            Campaign campaign = db.Campaigns.Where(c => c.AccountId == accountId && c.Id == campaignId).FirstOrDefault();

            if (campaign == null || campaign.IsDeleted == true || campaign.CampaignStateId != CampaignState.PriceSet)
            {
                return BadRequest();
            }
            
            campaign.CampaignStateId = CampaignState.Confirmed;
            
            db.SetModified(campaign);

            db.SaveChanges();

            // Trigger the payment
            ChargeHelper chargeHelper = new ChargeHelper();
            chargeHelper.ChargeCampaign(campaignId);

            return Ok();
        }

        public IHttpActionResult HasCampaignBeenProcessed(int campaignId)
        {
            int accountId = this.GetAccountId();
            // Get the campaign requested, if owned by user and not deleted
            Campaign campaign = db.Campaigns.Where(c => c.AccountId == accountId && c.Id == campaignId).FirstOrDefault();

            if (campaign == null || campaign.IsDeleted == true || !campaign.CanEditCampaign())
            {
                return BadRequest();
            }

            if (campaign.CampaignStateId == CampaignState.PriceSet)
            {
                return Ok(new CampaignProgressModel { HasCampaignBeenProcessed = true });
            }
            else
            {
                return Ok(new CampaignProgressModel { HasCampaignBeenProcessed = false });
            }
        }

        // DELETE: api/Campaigns/5
        [ResponseType(typeof(Campaign))]
        public IHttpActionResult DeleteCampaign(int id)
        {
            int accountId = this.GetAccountId();
            Campaign campaign = db.Campaigns.Where(c => c.Id == id && c.AccountId == accountId).FirstOrDefault();
            if (campaign == null || campaign.IsDeleted == true)
            {
                return NotFound();
            }

            campaign.IsDeleted = true;
            db.SetModified(campaign);
            db.SaveChanges();

            return Ok(campaign);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CampaignExists(int id)
        {
            return db.Campaigns.Count(e => e.Id == id) > 0;
        }
    }
}