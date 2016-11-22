using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccessLayer;

namespace API.Controllers
{
    [Authorize]
    public class ProfileListsController : BaseApiController
    {
        private IPostessDB db = new PostessDB();

        public ProfileListsController() { }

        public ProfileListsController(IPostessDB context)
        {
            db = context;
        }

        // GET: api/ProfileLists
        public IQueryable<ProfileList> GetProfileLists()
        {
            int accountId = this.GetAccountId();
            return db.ProfileLists.Where(p => p.AccountId == accountId && p.IsViewable == true);
        }

        // GET: api/ProfileLists/5
        [ResponseType(typeof(ProfileList))]
        public IHttpActionResult GetProfileList(int id)
        {
            int accountId = this.GetAccountId();
            ProfileList profileList = db.ProfileLists.Where(p => p.Id == id && p.AccountId == accountId).FirstOrDefault();
            if (profileList == null || profileList.IsDeleted == true || profileList.IsViewable == false)
            {
                return NotFound();
            }

            return Ok(profileList);
        }

        // PUT: api/ProfileLists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProfileList(int id, ProfileList profileList)
        {
            int accountId = this.GetAccountId();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profileList.Id || profileList.AccountId != accountId)
            {
                return BadRequest();
            }

            db.SetModified(profileList);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileListExists(id))
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

        // POST: api/ProfileLists
        [ResponseType(typeof(ProfileList))]
        public IHttpActionResult PostProfileList(ProfileList profileList)
        {
            int accountId = this.GetAccountId();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            profileList.AccountId = accountId;

            db.ProfileLists.Add(profileList);

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = profileList.Id }, profileList);
        }

        // POST: api/ProfileLists
        [ResponseType(typeof(ProfileList))]
        [HttpPost]
        public IHttpActionResult PostInvisbleProfileList(ProfileList profileList)
        {
            int accountId = this.GetAccountId();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            profileList.AccountId = accountId;
            profileList.IsViewable = true;

            db.ProfileLists.Add(profileList);

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = profileList.Id }, profileList);
        }

        // DELETE: api/ProfileLists/5
        [ResponseType(typeof(ProfileList))]
        public IHttpActionResult DeleteProfileList(int id)
        {
            int accountId = this.GetAccountId();

            ProfileList profileList = db.ProfileLists.Where(p => p.Id ==id && p.AccountId == accountId).FirstOrDefault();
            if (profileList == null || profileList.IsDeleted == true)
            {
                return NotFound();
            }

            profileList.IsDeleted = true;
            db.SetModified(profileList);
            db.SaveChanges();

            return Ok(profileList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfileListExists(int id)
        {
            return db.ProfileLists.Count(e => e.Id == id) > 0;
        }
    }
}