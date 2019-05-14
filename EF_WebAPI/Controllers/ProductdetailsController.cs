using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EF_WebAPI.Models;

namespace EF_WebAPI.Controllers
{
    public class ProductdetailsController : ApiController
    {
        private TestDBEntities1 db = new TestDBEntities1();

        // GET: api/Productdetails
        public  IQueryable<Productdetail> GetProductdetails()
        {
            return db.Productdetails;
        }

        // GET: api/Productdetails/5
        [ResponseType(typeof(Productdetail))]
        public IHttpActionResult GetProductdetail(int id)
        {
            
            Productdetail productdetail =db.Productdetails.Find(id);
            if (productdetail == null)
            {
                return NotFound();
            }

            return  Ok(productdetail);
        }

        // PUT: api/Productdetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProductdetail(int id, Productdetail productdetail)
        {
         

            if (id != productdetail.id)
            {
                return BadRequest();
            }

            db.Entry(productdetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductdetailExists(id))
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

        // POST: api/Productdetails
        [ResponseType(typeof(Productdetail))]
        public IHttpActionResult PostProductdetail(Productdetail productdetail)
        {
       
            db.Productdetails.Add(productdetail);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProductdetailExists(productdetail.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = productdetail.id }, productdetail);
        }

        // DELETE: api/Productdetails/5
        [ResponseType(typeof(Productdetail))]
        public IHttpActionResult DeleteProductdetail(int id)
        {
            Productdetail productdetail = db.Productdetails.Find(id);
            if (productdetail == null)
            {
                return NotFound();
            }

            db.Productdetails.Remove(productdetail);
            db.SaveChanges();

            return Ok(productdetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductdetailExists(int id)
        {
            return db.Productdetails.Count(e => e.id == id) > 0;
        }
    }
}