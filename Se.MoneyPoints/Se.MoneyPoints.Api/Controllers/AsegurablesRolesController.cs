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
using Se.MoneyPoints.Model.Bussiness.Entities;

namespace Se.MoneyPoints.Api.Controllers
{
    public class AsegurablesRolesController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/AsegurablesRoles
        public IQueryable<AsegurablesRole> GetAsegurablesRoles()
        {
            return db.AsegurablesRoles.Include(i=>i.Asegurable).Include(h=>h.Role);
        }

        [Route("api/AsegurablesRoles/GetAsegurablesRolesByRole/{rolId}")]
        public IQueryable<AsegurablesRole> GetAsegurablesRolesByRole(int rolId)
        {
            return db.AsegurablesRoles.Where(a => a.RolId == rolId).Include(i => i.Asegurable).Include(h => h.Role);
        }

        // GET: api/AsegurablesRoles/5
        [ResponseType(typeof(AsegurablesRole))]
        public IHttpActionResult GetAsegurablesRole(int id)
        {
            AsegurablesRole asegurablesRole = db.AsegurablesRoles.Include(i => i.Asegurable).Include(h => h.Role).FirstOrDefault(x=>x.AsegurablesRolId == id);
            if (asegurablesRole == null)
            {
                return NotFound();
            }

            return Ok(asegurablesRole);
        }

        // PUT: api/AsegurablesRoles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAsegurablesRole(int id, AsegurablesRole asegurablesRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != asegurablesRole.AsegurablesRolId)
            {
                return BadRequest();
            }

            db.Entry(asegurablesRole).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AsegurablesRoleExists(id))
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

        // POST: api/AsegurablesRoles
        [ResponseType(typeof(AsegurablesRole))]
        public IHttpActionResult PostAsegurablesRole(AsegurablesRole asegurablesRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AsegurablesRoles.Add(asegurablesRole);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = asegurablesRole.AsegurablesRolId }, asegurablesRole);
        }

        // DELETE: api/AsegurablesRoles/5
        [ResponseType(typeof(AsegurablesRole))]
        public IHttpActionResult DeleteAsegurablesRole(int id)
        {
            AsegurablesRole asegurablesRole = db.AsegurablesRoles.Find(id);
            if (asegurablesRole == null)
            {
                return NotFound();
            }

            db.AsegurablesRoles.Remove(asegurablesRole);
            db.SaveChanges();

            return Ok(asegurablesRole);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AsegurablesRoleExists(int id)
        {
            return db.AsegurablesRoles.Count(e => e.AsegurablesRolId == id) > 0;
        }
    }
}