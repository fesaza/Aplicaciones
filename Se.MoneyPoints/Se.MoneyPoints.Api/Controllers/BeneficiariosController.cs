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
using Se.MoneyPoints.Model.Bussiness.Entities.enums;

namespace Se.MoneyPoints.Api.Controllers
{
    public class BeneficiariosController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/Beneficiarios
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Beneficiario> GetBeneficiarios()
        {
            return db.Beneficiarios.Include(x => x.BeneficiariosClientes).Include(x => x.Tercero);
        }

        [HttpGet]
        [Route("api/Beneficiarios/GetBeneficiariosCandidatos/{clienteId}")]
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Beneficiario> GetBeneficiariosCandidatos(int clienteId)
        {
            var beneficiariosAdded = db.BeneficiariosClientes.Where(b => b.ClienteId == clienteId).Select(b => b.BeneficiarioId);

            return db.Beneficiarios.Where(b => !beneficiariosAdded.Contains(b.BeneficiarioId)).Include(x => x.BeneficiariosClientes).Include(x => x.Tercero);
        }

        // GET: api/Beneficiarios/5
        [ResponseType(typeof(Beneficiario))]
        public IHttpActionResult GetBeneficiario(int id)
        {
            Beneficiario beneficiario = db.Beneficiarios.Include(x => x.BeneficiariosClientes)
                                                        .Include(x => x.Tercero).FirstOrDefault(x=>x.BeneficiarioId == id);
            if (beneficiario == null)
            {
                return NotFound();
            }

            return Ok(beneficiario);
        }

        // PUT: api/Beneficiarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBeneficiario(int id, Beneficiario beneficiario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beneficiario.BeneficiarioId)
            {
                return BadRequest();
            }



            beneficiario.BeneficiariosClientes = null;
            beneficiario.Tercero.Beneficiarios = null;
            beneficiario.Tercero.Usuarios = null;
            beneficiario.Tercero.Beneficiarios = null;
            beneficiario.Tercero.Clientes = null;
            beneficiario.Tercero.Usuarios = null;
            db.Entry(beneficiario).State = EntityState.Modified;
            db.Entry(beneficiario.Tercero).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeneficiarioExists(id))
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

        // POST: api/Beneficiarios
        [ResponseType(typeof(Beneficiario))]
        [AllowAnonymous]
        public IHttpActionResult PostBeneficiario(Beneficiario beneficiario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var terceroRepetido = db.Terceros.FirstOrDefault(t => t.Identificacion == beneficiario.Tercero.Identificacion);

            if (terceroRepetido != null) throw new Exception("La cédula se encuentra repetida");

            beneficiario.Tercero.FechaIngreso = DateTime.Now;
            if (beneficiario.Tercero.Usuarios.Count == 0) throw new Exception("Faltan los datos de login");
            beneficiario.Tercero.Usuarios.First().RolId = (int)Roles.Beneficiario;
            db.Beneficiarios.Add(beneficiario);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = beneficiario.BeneficiarioId }, beneficiario);
        }

        // DELETE: api/Beneficiarios/5
        [ResponseType(typeof(Beneficiario))]
        public IHttpActionResult DeleteBeneficiario(int id)
        {
            Beneficiario beneficiario = db.Beneficiarios.Find(id);
            if (beneficiario == null)
            {
                return NotFound();
            }

            db.Beneficiarios.Remove(beneficiario);
            db.SaveChanges();

            return Ok(beneficiario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeneficiarioExists(int id)
        {
            return db.Beneficiarios.Count(e => e.BeneficiarioId == id) > 0;
        }
    }
}