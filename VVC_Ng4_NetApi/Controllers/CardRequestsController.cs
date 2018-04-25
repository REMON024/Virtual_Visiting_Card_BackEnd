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
    [Authorize(Roles = "user")]
    public class CardRequestsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("api/CardRequests/{id}")]
        public HttpResponseMessage GetCardRequests(string id)
        {
            var result = from CardRequest in db.CardRequests.Where(x => x.ToUserId == id && x.RequestAccept == false)
                         join Card in db.Cards on CardRequest.FromUserId equals Card.UserId
                         join Organization in db.Organizations on Card.OrganizationId equals Organization.OrganizationId
                         select new {CardRequest.Id ,Card.Field1, Organization.Name, CardRequest.FromUserId, CardRequest.ToUserId };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        
        [Route("api/CardRequests",Name = "PostCardRequest")]
        public IHttpActionResult PostCardRequest(CardRequest cardRequest)
        {
            cardRequest.Time = DateTime.Now.TimeOfDay;
            cardRequest.Date = DateTime.Now.Date;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CardRequests.Add(cardRequest);
            db.SaveChanges();

            return CreatedAtRoute("PostCardRequest", new { id = cardRequest.Id }, cardRequest);
        }

        [Route("api/CardRequests/update/{id}")]
        public IHttpActionResult PutCardRequest(int id, CardRequest cardRequest)
        {
            cardRequest.Date = DateTime.Now.Date;
            cardRequest.Time = DateTime.Now.TimeOfDay;
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cardRequest.Id)
            {
                return BadRequest();
            }

            db.Entry(cardRequest).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardRequestExists(id))
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

        [Route("api/CardRequests/delete/{id}")]
        public IHttpActionResult DeleteCardRequest(int id)
        {
            CardRequest cardRequest = db.CardRequests.Find(id);
            if (cardRequest == null)
            {
                return NotFound();
            }

            db.CardRequests.Remove(cardRequest);
            db.SaveChanges();

            return Ok(cardRequest);
        }

        private bool CardRequestExists(int id)
        {
            return db.CardRequests.Count(e => e.Id == id) > 0;
        }
    }
}