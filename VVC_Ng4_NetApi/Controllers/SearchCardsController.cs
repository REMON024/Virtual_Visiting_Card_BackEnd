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
    public class SearchCardsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "user")]
        [Route("api/searchCards/{id}")]
        public IHttpActionResult Get(string id)
        {
            var result = from Card in db.Cards.Where(x => x.Field1.Contains(id))
                         join Organization in db.Organizations on Card.OrganizationId equals Organization.OrganizationId
                         select new {Card.UserId ,Card.Field1, Organization.Name };

            return Ok(result) ;
        }
    }
}