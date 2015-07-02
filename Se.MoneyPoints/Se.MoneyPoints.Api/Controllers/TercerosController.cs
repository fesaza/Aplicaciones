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
using System.Web.Http.OData;

namespace Se.MoneyPoints.Api.Controllers
{
    public class TercerosController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/Terceros
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Tercero> GetTerceros()
        {
            return db.Terceros.Include(p=>p.Clientes)
                              .Include(x=>x.Beneficiarios)
                              .Include(o=>o.Usuarios);
        }

        // GET: api/Terceros/5
        [ResponseType(typeof(Tercero))]
        public IHttpActionResult GetTercero(int id)
        {
            Tercero tercero = db.Terceros.Include(p => p.Clientes)
                                         .Include(x => x.Beneficiarios)
                                         .Include(o => o.Usuarios)
                                         .FirstOrDefault(x=>x.TerceroId==id);
            if (tercero == null)
            {
                return NotFound();
            }

            return Ok(tercero);
        }

        

        // PUT: api/Terceros/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTercero(int id, Tercero tercero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tercero.TerceroId)
            {
                return BadRequest();
            }

            db.Entry(tercero).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerceroExists(id))
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

        // POST: api/Terceros
        [ResponseType(typeof(Tercero))]
        public IHttpActionResult PostTercero(Tercero tercero)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Terceros.Add(tercero);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tercero.TerceroId }, tercero);
        }

        // DELETE: api/Terceros/5
        [ResponseType(typeof(Tercero))]
        public IHttpActionResult DeleteTercero(int id)
        {
            Tercero tercero = db.Terceros.Find(id);
            if (tercero == null)
            {
                return NotFound();
            }

            db.Terceros.Remove(tercero);
            db.SaveChanges();

            return Ok(tercero);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TerceroExists(int id)
        {
            return db.Terceros.Count(e => e.TerceroId == id) > 0;
        }
    }
}