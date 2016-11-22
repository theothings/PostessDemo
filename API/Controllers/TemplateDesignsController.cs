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
    public class TemplateDesignsController : ApiController
    {
        private IPostessDB db = new PostessDB();

        public TemplateDesignsController() { }

        public TemplateDesignsController(IPostessDB context)
        {
            db = context;
        }

        // GET: api/TemplateDesigns
        public IQueryable<TemplateDesign> GetTemplateDesigns()
        {
            return db.TemplateDesigns.Where(t => t.IsDeleted == false);
        }

        // GET: api/TemplateDesigns/5
        [ResponseType(typeof(TemplateDesign))]
        public IHttpActionResult GetTemplateDesign(int id)
        {
            TemplateDesign templateDesign = db.TemplateDesigns.Find(id);

            if (templateDesign == null || templateDesign.IsDeleted == true)
            {
                return NotFound();
            }

            return Ok(templateDesign);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TemplateDesignExists(int id)
        {
            return db.TemplateDesigns.Count(e => e.Id == id) > 0;
        }
    }
}