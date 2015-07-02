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
    public class BeneficiariosClientesComprasController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/BeneficiariosClientesCompras
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<BeneficiariosClientesCompra> GetBeneficiariosClientesCompras()
        {
            return db.BeneficiariosClientesCompras.Include(q=>q.Compra).Include(l=>l.BeneficiariosCliente);
        }

        // GET: api/BeneficiariosClientesCompras/5
        [ResponseType(typeof(BeneficiariosClientesCompra))]
        public IHttpActionResult GetBeneficiariosClientesCompra(int id)
        {
            BeneficiariosClientesCompra beneficiariosClientesCompra = db.BeneficiariosClientesCompras.Include(x=>x.Compra).Include(x=>x.BeneficiariosCliente).FirstOrDefault(x=>x.BeneficiariosClientesCompraId == id);
            if (beneficiariosClientesCompra == null)
            {
                return NotFound();
            }

            return Ok(beneficiariosClientesCompra);
        }

        // PUT: api/BeneficiariosClientesCompras/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBeneficiariosClientesCompra(int id, BeneficiariosClientesCompra beneficiariosClientesCompra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beneficiariosClientesCompra.BeneficiariosClientesCompraId)
            {
                return BadRequest();
            }

            db.Entry(beneficiariosClientesCompra).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeneficiariosClientesCompraExists(id))
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

        // POST: api/BeneficiariosClientesCompras
        [ResponseType(typeof(BeneficiariosClientesCompra))]
        public IHttpActionResult PostBeneficiariosClientesCompra(BeneficiariosClientesCompra beneficiariosClientesCompra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BeneficiariosClientesCompras.Add(beneficiariosClientesCompra);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BeneficiariosClientesCompraExists(beneficiariosClientesCompra.BeneficiariosClientesCompraId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = beneficiariosClientesCompra.BeneficiariosClientesCompraId }, beneficiariosClientesCompra);
        }

        // DELETE: api/BeneficiariosClientesCompras/5
        [ResponseType(typeof(BeneficiariosClientesCompra))]
        public IHttpActionResult DeleteBeneficiariosClientesCompra(int id)
        {
            BeneficiariosClientesCompra beneficiariosClientesCompra = db.BeneficiariosClientesCompras.Find(id);
            if (beneficiariosClientesCompra == null)
            {
                return NotFound();
            }

            db.BeneficiariosClientesCompras.Remove(beneficiariosClientesCompra);
            db.SaveChanges();

            return Ok(beneficiariosClientesCompra);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeneficiariosClientesCompraExists(int id)
        {
            return db.BeneficiariosClientesCompras.Count(e => e.BeneficiariosClientesCompraId == id) > 0;
        }
    }
}