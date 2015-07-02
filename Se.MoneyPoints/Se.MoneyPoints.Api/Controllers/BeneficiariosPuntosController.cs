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
    public class BeneficiariosPuntosController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/BeneficiariosPuntos
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<BeneficiariosPunto> GetBeneficiariosPuntos()
        {
            return db.BeneficiariosPuntos.Include(d=>d.BeneficiariosCliente);
        }

        // GET: api/BeneficiariosPuntos/5
        [ResponseType(typeof(BeneficiariosPunto))]
        public IHttpActionResult GetBeneficiariosPunto(int id)
        {
            BeneficiariosPunto beneficiariosPunto = db.BeneficiariosPuntos.Include(o=>o.BeneficiariosCliente).FirstOrDefault(o=>o.BeneficiariosClienteId == id);
            if (beneficiariosPunto == null)
            {
                return NotFound();
            }

            return Ok(beneficiariosPunto);
        }

        // PUT: api/BeneficiariosPuntos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBeneficiariosPunto(int id, BeneficiariosPunto beneficiariosPunto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beneficiariosPunto.BeneficiariosPuntoId)
            {
                return BadRequest();
            }

            db.Entry(beneficiariosPunto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeneficiariosPuntoExists(id))
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

        // POST: api/BeneficiariosPuntos
        [ResponseType(typeof(BeneficiariosPunto))]
        public IHttpActionResult PostBeneficiariosPunto(BeneficiariosPunto beneficiariosPunto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            beneficiariosPunto.Fecha = DateTime.Now;

            //Calcular valor según equivalencias

            var benefClientes = db.BeneficiariosClientes.Where(b => b.BeneficiariosClienteId == beneficiariosPunto.BeneficiariosClienteId).FirstOrDefault();

            if (benefClientes == null) throw new Exception("No se encontro la información solicitada");

            var clienteId = benefClientes.ClienteId;

            if (clienteId == 0) throw new Exception("No se encontró el cliente");

            var equiv = db.Equivalencias.Where(e => e.ClienteId == clienteId).OrderByDescending(o => o.EquivalenciaId).First();

            if (equiv == null) throw new Exception("No se encontraron equivalencias");

            var rel = equiv.Valor / equiv.Puntos;

            beneficiariosPunto.Valor = beneficiariosPunto.Puntos * rel;

            benefClientes.Saldo += beneficiariosPunto.Valor;
            benefClientes.Puntos += beneficiariosPunto.Puntos;

            db.Entry(benefClientes).State = EntityState.Modified;

            db.BeneficiariosPuntos.Add(beneficiariosPunto);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = beneficiariosPunto.BeneficiariosPuntoId }, beneficiariosPunto);
        }

        // DELETE: api/BeneficiariosPuntos/5
        [ResponseType(typeof(BeneficiariosPunto))]
        public IHttpActionResult DeleteBeneficiariosPunto(int id)
        {
            BeneficiariosPunto beneficiariosPunto = db.BeneficiariosPuntos.Find(id);
            if (beneficiariosPunto == null)
            {
                return NotFound();
            }

            db.BeneficiariosPuntos.Remove(beneficiariosPunto);
            db.SaveChanges();

            return Ok(beneficiariosPunto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeneficiariosPuntoExists(int id)
        {
            return db.BeneficiariosPuntos.Count(e => e.BeneficiariosPuntoId == id) > 0;
        }
    }
}