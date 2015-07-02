using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Se.MoneyPoints.Model.Bussiness.Entities;
using System.Web.Http.OData;
using Se.MoneyPoints.Model.Bussiness.Entities.enums;

namespace Se.MoneyPoints.Api.Controllers
{
    public class ConciliacionesController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/Conciliaciones
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Conciliacione> GetConciliaciones()
        {
            return db.Conciliaciones.Include(x=>x.AfiliadosCliente);
        }

        // GET: api/Conciliaciones/5
        [ResponseType(typeof(Conciliacione))]
        public async Task<IHttpActionResult> GetConciliacione(int id)
        {
            Conciliacione conciliacione = await db.Conciliaciones.FindAsync(id);
            if (conciliacione == null)
            {
                return NotFound();
            }

            return Ok(conciliacione);
        }

        [HttpGet]
        [EnableQuery]
        [Route("api/Conciliaciones/ConciliacionesByAfiliadosClientes/{afliliadoClienteId}")]
        public IQueryable<Conciliacione> GetConciliacionesByAfiliadosClientes(int afliliadoClienteId)
        {
            return db.Conciliaciones.Where(c => c.AfiliadosClienteId == afliliadoClienteId);
        }

        [HttpGet]
        [EnableQuery]
        [Route("api/Conciliaciones/ConciliacionesByCliente/{clienteId}")]
        public IQueryable<Conciliacione> GetConciliacionesByCliente(int clienteId)
        {
            return db.Conciliaciones.Where(e => e.AfiliadosCliente.ClienteId == clienteId);
        }

        [HttpGet]
        [EnableQuery]
        [Route("api/Conciliaciones/ConciliacionesByAliado/{clienteId}")]
        public IQueryable<Conciliacione> GetConciliacionesByAliado(int clienteId)
        {
            var aliado = db.Afiliados.FirstOrDefault(a => a.ClienteId == clienteId);

            var aliadoId = 0;

            if (aliado != null) aliadoId = aliado.AfiliadoId;

            return db.Conciliaciones.Where(e => e.AfiliadosCliente.AfiliadoId == aliadoId).Include(x => x.AfiliadosCliente);
        }

        // PUT: api/Conciliaciones/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutConciliacione(int id, Conciliacione conciliacione)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != conciliacione.ConciliacionId)
            {
                return BadRequest();
            }

            conciliacione.AfiliadosCliente = null;
            //conciliacione.Estado = EstadosConciliaciones.Enviada.ToString();
            db.Entry(conciliacione).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConciliacioneExists(id))
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

        [HttpGet]
        [EnableQuery]
        [Route("api/Conciliaciones/ConciliacionexCliente/{afiliadoClienteId}")]
        public IQueryable<Conciliacione> PostConciliacionexCliente(int clienteId)
        {
            return db.Conciliaciones.Where(e => e.AfiliadosCliente.ClienteId == clienteId).Include(x => x.AfiliadosCliente);
        }

        // POST: api/Conciliaciones
        [ResponseType(typeof(Conciliacione))]
        public async Task<IHttpActionResult> PostConciliacione(Conciliacione conciliacione)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            conciliacione.Estado = EstadosConciliaciones.Enviada.ToString();
            conciliacione.Fecha = DateTime.Now;

            db.Conciliaciones.Add(conciliacione);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = conciliacione.ConciliacionId }, conciliacione);
        }

        // DELETE: api/Conciliaciones/5
        [ResponseType(typeof(Conciliacione))]
        public async Task<IHttpActionResult> DeleteConciliacione(int id)
        {
            Conciliacione conciliacione = await db.Conciliaciones.FindAsync(id);
            if (conciliacione == null)
            {
                return NotFound();
            }

            //db.Conciliaciones.Remove(conciliacione);
            conciliacione.Fecha = DateTime.Now;
            db.Entry(conciliacione).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok(conciliacione);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConciliacioneExists(int id)
        {
            return db.Conciliaciones.Count(e => e.ConciliacionId == id) > 0;
        }
    }
}