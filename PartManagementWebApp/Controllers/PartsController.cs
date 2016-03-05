using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using PartManagementWebApp.Models;
using SharpRepository.Repository;

namespace PartManagementWebApp.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using PartManagementWebApp.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Part>("Parts");
    builder.EntitySet<ApplicationUser>("Users"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PartsController : ODataController
    {
        private readonly IRepository<Part> _partRepository; 
        private ApplicationDbContext db = new ApplicationDbContext();

        public PartsController(IRepository<Part> partRepository)
        {
            _partRepository = partRepository;
        }

        // GET: odata/Parts
        [EnableQuery]
        public IQueryable<Part> GetParts()
        {
            return _partRepository.AsQueryable();
        }

        // GET: odata/Parts(5)
        [EnableQuery]
        public SingleResult<Part> GetPart([FromODataUri] int key)
        {
            return SingleResult.Create(db.Parts.Where(part => part.Id == key));
        }

        // PUT: odata/Parts(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Part> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Part part = db.Parts.Find(key);
            if (part == null)
            {
                return NotFound();
            }

            patch.Put(part);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(part);
        }

        // POST: odata/Parts
        public IHttpActionResult Post(Part part)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Parts.Add(part);
            db.SaveChanges();

            return Created(part);
        }

        // PATCH: odata/Parts(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Part> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Part part = db.Parts.Find(key);
            if (part == null)
            {
                return NotFound();
            }

            patch.Patch(part);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(part);
        }

        // DELETE: odata/Parts(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Part part = db.Parts.Find(key);
            if (part == null)
            {
                return NotFound();
            }

            db.Parts.Remove(part);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Parts(5)/Owner
        [EnableQuery]
        public SingleResult<ApplicationUser> GetOwner([FromODataUri] int key)
        {
            return SingleResult.Create(db.Parts.Where(m => m.Id == key).Select(m => m.Owner));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PartExists(int key)
        {
            return db.Parts.Count(e => e.Id == key) > 0;
        }
    }
}
