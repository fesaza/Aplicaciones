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
    public class ClientesController : ApiController
    {
        private MoneyPoints_dlloEntities db = new MoneyPoints_dlloEntities();

        // GET: api/Clientes
        [EnableQuery(AllowedQueryOptions = System.Web.Http.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Cliente> GetClientes()
        {
            return db.Clientes.Include(u=>u.Afiliados)
                              .Include(o=>o.AfiliadosClientes)
                              .Include(x=>x.BeneficiariosClientes)
                              .Include(x=>x.Tercero)
                              .Include(p=>p.Equivalencias);
        }

        // GET: api/Clientes/5
        [ResponseType(typeof(Cliente))]
        public IHttpActionResult GetCliente(int id)
        {
            Cliente cliente = db.Clientes.Include(u => u.Afiliados)
                              .Include(o => o.AfiliadosClientes)
                              .Include(x => x.BeneficiariosClientes)
                              .Include(x => x.Tercero)
                              .Include(p => p.Equivalencias).FirstOrDefault(p=>p.ClienteId==id);
            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        // PUT: api/Clientes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCliente(int id, Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cliente.ClienteId)
            {
                return BadRequest();
            }

            //if (cliente.IsBeneficiario)
            //{
            //    var afiliado = db.Afiliados.FirstOrDefault(c => c.ClienteId == cliente.ClienteId);
            //    if (afiliado == null)
            //    {
            //        db.Afiliados.Add(new Afiliado
            //        {
            //            ClienteId = cliente.ClienteId
            //        });
            //    }
            //}//todo: falta cuando un cliente deja de ser beneficiario

            cliente.Afiliados = null;
            cliente.AfiliadosClientes = null;
            cliente.BeneficiariosClientes = null;
            cliente.Tercero.Clientes = null;
            cliente.Tercero.Beneficiarios = null;
            cliente.Tercero.Usuarios = null;
            db.Entry(cliente).State = EntityState.Modified;
            db.Entry(cliente.Tercero).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        // POST: api/Clientes
        [ResponseType(typeof(Cliente))]
        public IHttpActionResult PostCliente(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            cliente.Tercero.FechaIngreso = DateTime.Now;

            if (cliente.Tercero.Usuarios.Count == 0) throw new Exception("Faltan los datos de login");

            cliente.Tercero.Usuarios.First().RolId = (int)Roles.Cliente;

            db.Clientes.Add(cliente);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cliente.ClienteId }, cliente);
        }

        // DELETE: api/Clientes/5
        [ResponseType(typeof(Cliente))]
        public IHttpActionResult DeleteCliente(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }

            db.Clientes.Remove(cliente);
            cliente.Tercero.FechaBaja = DateTime.Now;
            db.Entry(cliente).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(cliente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClienteExists(int id)
        {
            return db.Clientes.Count(e => e.ClienteId == id) > 0;
        }
    }
}