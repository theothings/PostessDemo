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
    public class RecipientProfilesController : ApiController
    {
        private IPostessDB db = new PostessDB();

        public RecipientProfilesController() { }

        public RecipientProfilesController(IPostessDB context)
        {
            db = context;
        }

        // GET: api/RecipientProfiles
        public IQueryable<RecipientProfile> GetRecipientProfiles()
        {
            return db.RecipientProfiles;
        }

        // GET: api/RecipientProfiles/5
        [ResponseType(typeof(RecipientProfile))]
        public IHttpActionResult GetRecipientProfile(int id)
        {
            RecipientProfile recipientProfile = db.RecipientProfiles.Find(id);
            if (recipientProfile == null)
            {
                return NotFound();
            }

            return Ok(recipientProfile);
        }

        // PUT: api/RecipientProfiles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRecipientProfile(int id, RecipientProfile recipientProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipientProfile.Id)
            {
                return BadRequest();
            }

            db.SetModified(recipientProfile);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipientProfileExists(id))
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

        // POST: api/RecipientProfiles
        [ResponseType(typeof(RecipientProfile))]
        public IHttpActionResult PostRecipientProfile(RecipientProfile recipientProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RecipientProfiles.Add(recipientProfile);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recipientProfile.Id }, recipientProfile);
        }

        // DELETE: api/RecipientProfiles/5
        [ResponseType(typeof(RecipientProfile))]
        public IHttpActionResult DeleteRecipientProfile(int id)
        {
            RecipientProfile recipientProfile = db.RecipientProfiles.Find(id);
            if (recipientProfile == null)
            {
                return NotFound();
            }

            db.RecipientProfiles.Remove(recipientProfile);
            db.SaveChanges();

            return Ok(recipientProfile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RecipientProfileExists(int id)
        {
            return db.RecipientProfiles.Count(e => e.Id == id) > 0;
        }
    }
}