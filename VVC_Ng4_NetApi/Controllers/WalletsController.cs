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
    public class WalletsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("api/Wallets/{id}")]
        public IHttpActionResult GetWallet(string id)
        {
            var result = from Wallet in db.Wallets.Where(x => x.OwnerId == id)
                         join Card in db.Cards on Wallet.CardId equals Card.UserId
                         join Organization in db.Organizations on Card.OrganizationId equals Organization.OrganizationId
                         select new { Card.UserId, Card.Field1, Card.Field2, Card.Field3, Card.Field4, Card.Field5,
                             Card.Field6, Card.Field7, Organization.Name };
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/Wallets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWallet(int id, Wallet wallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wallet.Id)
            {
                return BadRequest();
            }

            db.Entry(wallet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WalletExists(id))
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

        // POST: api/Wallets
        [ResponseType(typeof(Wallet))]
        public IHttpActionResult PostWallet(Wallet wallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Wallets.Add(wallet);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = wallet.Id }, wallet);
        }

        // DELETE: api/Wallets/5
        [ResponseType(typeof(Wallet))]
        public IHttpActionResult DeleteWallet(int id)
        {
            Wallet wallet = db.Wallets.Find(id);
            if (wallet == null)
            {
                return NotFound();
            }

            db.Wallets.Remove(wallet);
            db.SaveChanges();

            return Ok(wallet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WalletExists(int id)
        {
            return db.Wallets.Count(e => e.Id == id) > 0;
        }
    }
}