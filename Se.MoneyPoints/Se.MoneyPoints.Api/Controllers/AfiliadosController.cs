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

namespace Se.MoneyPoints.Api.Controllers
{
    public class AfiliadosController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/Afiliados
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Afiliado> GetAfiliados()
        {
            return db.Afiliados.Include(o => o.AfiliadosClientes).Include(q => q.Cliente).Include("Cliente.Tercero").Include(i => i.Compras);
        }

        // GET: api/Afiliados/5
        [ResponseType(typeof(Afiliado))]
        public IHttpActionResult GetAfiliado(int id)
        {
            Afiliado afiliado = db.Afiliados.Include(o=>o.AfiliadosClientes).Include(q=>q.Cliente).Include("Cliente.Tercero").Include(i=>i.Compras).FirstOrDefault(x=>x.AfiliadoId == id);
            if (afiliado == null)
            {
                return NotFound();
            }

            return Ok(afiliado);
        }

        [HttpGet]
        [EnableQuery]
        [Route("api/Afiliados/AfiliadosByAfiliadosCliente/{clienteId}")]
        public IQueryable<Afiliado> GetAfiliadosByAfiliadosClientes(int clienteId)
        {
            return db.Afiliados.Where(a => a.ClienteId == clienteId).Include(o => o.AfiliadosClientes).Include(q => q.Cliente).Include("Cliente.Tercero").Include(i => i.Compras);
        }

        [HttpGet]
        [EnableQuery]
        [Route("api/Afiliados/GetAfiliadosCandidatos/{clienteId}")]
        public IQueryable<Afiliado> GetAfiliadosCandidatos(int clienteId)
        {
            var afiliadosIds = db.AfiliadosClientes
                .Where(a => a.ClienteId == clienteId)
                .Select(s => s.AfiliadoId);

            return db.Afiliados.Where(a => !afiliadosIds.Contains(a.AfiliadoId) && a.ClienteId != clienteId).Include("Cliente.Tercero");
        }

        // PUT: api/Afiliados/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAfiliado(int id, Afiliado afiliado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != afiliado.AfiliadoId)
            {
                return BadRequest();
            }

            db.Entry(afiliado).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AfiliadoExists(id))
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

        // POST: api/Afiliados
        [ResponseType(typeof(Afiliado))]
        public IHttpActionResult PostAfiliado(Afiliado afiliado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Afiliados.Add(afiliado);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = afiliado.AfiliadoId }, afiliado);
        }

        // DELETE: api/Afiliados/5
        [ResponseType(typeof(Afiliado))]
        public IHttpActionResult DeleteAfiliado(int id)
        {
            Afiliado afiliado = db.Afiliados.Find(id);
            if (afiliado == null)
            {
                return NotFound();
            }

            db.Afiliados.Remove(afiliado);
            db.SaveChanges();

            return Ok(afiliado);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AfiliadoExists(int id)
        {
            return db.Afiliados.Count(e => e.AfiliadoId == id) > 0;
        }
    }
}