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
    public class EquivalenciasController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/Equivalencias
        [EnableQuery]
        public IQueryable<Equivalencia> GetEquivalencias()
        {
            return db.Equivalencias.Include(x => x.Cliente);
        }

        // GET: api/Equivalencias/5
        [ResponseType(typeof(Equivalencia))]
        public IHttpActionResult GetEquivalencia(int id)
        {
            Equivalencia equivalencia = db.Equivalencias.Include(x => x.Cliente)
                                                        .FirstOrDefault(p => p.EquivalenciaId == id);
            if (equivalencia == null)
            {
                return NotFound();
            }

            return Ok(equivalencia);
        }

        [HttpGet]
        [EnableQuery]
        [Route("api/Equivalencias/EquivalenciasByCliente/{clienteId}")]
        public IQueryable<Equivalencia> GetEquivalenciasByClientes(int clienteId)
        {
            return db.Equivalencias.Where(e => e.ClienteId == clienteId).OrderBy(p => p.EquivalenciaId);
        }

        // PUT: api/Equivalencias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEquivalencia(int id, Equivalencia equivalencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != equivalencia.EquivalenciaId)
            {
                return BadRequest();
            }

            db.Entry(equivalencia).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquivalenciaExists(id))
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

        // POST: api/Equivalencias
        [ResponseType(typeof(Equivalencia))]
        public IHttpActionResult PostEquivalenciaClienteId(Equivalencia equivalencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!(equivalencia.Puntos > 0))
                throw new Exception("El número de puntos debe ser mayor a cero");

            if (!(equivalencia.Valor > 0))
                throw new Exception("El valor debe ser mayor a cero");

            if (equivalencia.ClienteId == 0)
                throw new Exception("Por favor ingrese el id del cliente");


            equivalencia.Cliente = null;
            db.Equivalencias.Add(equivalencia);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = equivalencia.EquivalenciaId }, equivalencia);
        }

        // DELETE: api/Equivalencias/5
        [ResponseType(typeof(Equivalencia))]
        public IHttpActionResult DeleteEquivalencia(int id)
        {
            Equivalencia equivalencia = db.Equivalencias.Find(id);
            if (equivalencia == null)
            {
                return NotFound();
            }

            equivalencia.FechaBaja = DateTime.Now;
            db.Entry(equivalencia).State = EntityState.Modified;

            //db.Equivalencias.Remove(equivalencia);
            db.SaveChanges();

            return Ok(equivalencia);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EquivalenciaExists(int id)
        {
            return db.Equivalencias.Count(e => e.EquivalenciaId == id) > 0;
        }
    }
}