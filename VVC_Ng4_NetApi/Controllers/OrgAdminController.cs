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
using VVC_Ng4_NetApi.Models;

namespace VVC_Ng4_NetApi.Controllers
{
    public class OrgadminController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/Orgadmin/Cardrequest/{id}")]
        public IHttpActionResult CardRequest(string id)
        {
            var result = from Organization in db.Organizations.Where(x => x.AdminId == id)
                         join Card in db.Cards.Where(x => x.RequestAccept == false) on Organization.OrganizationId equals Card.OrganizationId
                         select new { Card.UserId ,Card.Field1, Card.Field2, Card.Field3, Card.Field4, Card.Field5,
                                    Card.Field6, Card.Field7, Card.OrganizationId};
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/Orgadmin/Card/{id}")]
        public IHttpActionResult Card(string id)
        {
            var result = from Organization in db.Organizations.Where(x => x.AdminId == id)
                         join Card in db.Cards.Where(x=> x.RequestAccept == true) on Organization.OrganizationId equals Card.OrganizationId
                         select new
                         {
                             Card.Field1,
                             Card.Field2,
                             Card.Field3,
                             Card.Field4,
                             Card.Field5,
                             Card.Field6,
                             Card.Field7
                         };

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("api/Orgadmin/Card/Update/{id}")]
        public IHttpActionResult CardUpdate(string id, Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != card.UserId)
            {
                return BadRequest();
            }

            db.Entry(card).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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



        [HttpDelete]
        [Route("api/Orgadmin/Card/Delete/{id}")]
        public IHttpActionResult CardDelete(string id)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            db.Cards.Remove(card);
            db.SaveChanges();

            return Ok(card);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardExists(string id)
        {
            return db.Cards.Count(e => e.UserId == id) > 0;
        }
    }
}