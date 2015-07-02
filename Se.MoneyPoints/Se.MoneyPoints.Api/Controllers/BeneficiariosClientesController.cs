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
    public class BeneficiariosClientesController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/BeneficiariosClientes
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<BeneficiariosCliente> GetBeneficiariosClientes()
        {
          
            return db.BeneficiariosClientes.Include(x => x.BeneficiariosPuntos)
                                            .Include(x => x.Beneficiario)
                                            .Include(x => x.Cliente)
                                            .Include(x => x.BeneficiariosClientesCompras);
        }

        // GET: api/BeneficiariosClientes/5
        [ResponseType(typeof(BeneficiariosCliente))]
        public IHttpActionResult GetBeneficiariosCliente(int id)
        {
            BeneficiariosCliente beneficiariosCliente = db.BeneficiariosClientes.Include(x => x.BeneficiariosPuntos)
                                                                                .Include(x => x.Beneficiario)
                                                                                .Include(x => x.Cliente)
                                                                                .Include(x => x.BeneficiariosClientesCompras)
                                                                                .FirstOrDefault(x => x.BeneficiariosClienteId == id);
            if (beneficiariosCliente == null)
            {
                return NotFound();
            }

            return Ok(beneficiariosCliente);
        }

        /// <summary>
        /// Permite consultar los programas beneficiarios de un cliente
        /// </summary>
        /// <param name="clienteId">Id del cliente</param>
        /// <returns>lista de BeneficiariosClientes</returns>
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        [Route("api/BeneficiariosClientes/BeneficiariosClientesByCliente/{clienteId}")]
        public IQueryable<BeneficiariosCliente> GetBeneficiariosClientesByClientes(int clienteId)
        {
            return db.BeneficiariosClientes.Where(b => b.ClienteId == clienteId)
                                            .Include(x => x.BeneficiariosPuntos)
                                            .Include(x => x.Beneficiario)
                                            .Include(x => x.Cliente)
                                            .Include(x => x.BeneficiariosClientesCompras);
        }

        /// <summary>
        /// Permite consultar las cuentras de un beneficiario por aliado
        /// </summary>
        /// <param name="aliadoId">Id del Aliado</param>
        /// <param name="beneficiarioId">Id del beneficiario</param>
        /// <returns>Lista de programas de un beneficiario por aliado</returns>
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        [Route("api/BeneficiariosClientes/GetCuentasByAliadoAndBeneficiario/{aliadoId}/{beneficiarioId}")]
        public IQueryable<BeneficiariosCliente> GetCuentasByAliadoAndBeneficiario(int aliadoId, int beneficiarioId)
        {
            var clientesIds = db.AfiliadosClientes.Where(a => a.AfiliadoId == aliadoId).Select(a => a.ClienteId);
            return db.BeneficiariosClientes.Where(b => b.BeneficiarioId == beneficiarioId && clientesIds.Contains(b.ClienteId))
                                           .Include(x => x.BeneficiariosPuntos)
                                           .Include(x => x.Beneficiario)
                                           .Include(x => x.Cliente)
                                           .Include(x => x.BeneficiariosClientesCompras);
        }

        // PUT: api/BeneficiariosClientes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBeneficiariosCliente(int id, BeneficiariosCliente beneficiariosCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beneficiariosCliente.BeneficiariosClienteId)
            {
                return BadRequest();
            }

            db.Entry(beneficiariosCliente).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeneficiariosClienteExists(id))
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

        // POST: api/BeneficiariosClientes
        [ResponseType(typeof(BeneficiariosCliente))]
        public IHttpActionResult PostBeneficiariosCliente(BeneficiariosCliente beneficiariosCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            beneficiariosCliente.FechaIngreso = DateTime.Now;
            beneficiariosCliente.Saldo = 0;
            beneficiariosCliente.Puntos = 0;

            db.BeneficiariosClientes.Add(beneficiariosCliente);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = beneficiariosCliente.BeneficiariosClienteId }, beneficiariosCliente);
        }

        // DELETE: api/BeneficiariosClientes/5
        [ResponseType(typeof(BeneficiariosCliente))]
        public IHttpActionResult DeleteBeneficiariosCliente(int id)
        {
            BeneficiariosCliente beneficiariosCliente = db.BeneficiariosClientes.Find(id);
            if (beneficiariosCliente == null)
            {
                return NotFound();
            }

            db.BeneficiariosClientes.Remove(beneficiariosCliente);
            db.SaveChanges();

            return Ok(beneficiariosCliente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeneficiariosClienteExists(int id)
        {
            return db.BeneficiariosClientes.Count(e => e.BeneficiariosClienteId == id) > 0;
        }
    }
}