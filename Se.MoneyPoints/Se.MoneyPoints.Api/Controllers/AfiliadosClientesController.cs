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
    public class AfiliadosClientesController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/AfiliadosClientes
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<AfiliadosCliente> GetAfiliadosClientes()
        {
            return db.AfiliadosClientes.Include(p=>p.Afiliado).Include(i=>i.Cliente).Include(s=>s.Conciliaciones);
        }

        // GET: api/AfiliadosClientes/5
        [ResponseType(typeof(AfiliadosCliente))]
        public IHttpActionResult GetAfiliadosCliente(int id)
        {           
            //AfiliadosCliente afiliadosCliente = db.AfiliadosClientes.Find(id);
            AfiliadosCliente afiliadosCliente = db.AfiliadosClientes.Include(p => p.Afiliado)
                                                                    .Include(i => i.Cliente)
                                                                    .Include(s => s.Conciliaciones)
                                                                    .FirstOrDefault(x => x.AfiliadosClienteId == id);
            if (afiliadosCliente == null)
            {
                return NotFound();
            }

            return Ok(afiliadosCliente);
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        [Route("api/AfiliadosClientes/AfiliadosClientesByCliente/{clienteId}")]
        public IQueryable<AfiliadosCliente> GetAfiliadosClientesByClientes(int clienteId)
        {
            return db.AfiliadosClientes .Where(af => af.ClienteId == clienteId)
                                        .Include(p => p.Afiliado.Cliente.Tercero)
                                        .Include(i => i.Cliente)
                                        .Include(s => s.Conciliaciones);
        }

        // PUT: api/AfiliadosClientes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAfiliadosCliente(int id, AfiliadosCliente afiliadosCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != afiliadosCliente.AfiliadosClienteId)
            {
                return BadRequest();
            }

            db.Entry(afiliadosCliente).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AfiliadosClienteExists(id))
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

        // POST: api/AfiliadosClientes
        [ResponseType(typeof(AfiliadosCliente))]
        public IHttpActionResult PostAfiliadosCliente(AfiliadosCliente afiliadosCliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AfiliadosClientes.Add(afiliadosCliente);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = afiliadosCliente.AfiliadosClienteId }, afiliadosCliente);
        }

        // DELETE: api/AfiliadosClientes/5
        [ResponseType(typeof(AfiliadosCliente))]
        public IHttpActionResult DeleteAfiliadosCliente(int id)
        {
            AfiliadosCliente afiliadosCliente = db.AfiliadosClientes.Find(id);
            if (afiliadosCliente == null)
            {
                return NotFound();
            }

            db.AfiliadosClientes.Remove(afiliadosCliente);
            db.SaveChanges();

            return Ok(afiliadosCliente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AfiliadosClienteExists(int id)
        {
            return db.AfiliadosClientes.Count(e => e.AfiliadosClienteId == id) > 0;
        }
    }
}