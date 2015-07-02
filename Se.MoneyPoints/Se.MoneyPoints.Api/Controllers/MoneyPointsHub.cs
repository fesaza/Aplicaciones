using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Se.MoneyPoints.Model.Bussiness.Entities;
using System.Data.Entity;

namespace Se.MoneyPoints.Api.Controllers
{
    [HubName("money")]
    public class MoneyPointsHub : Hub
    {
        public void GreetAll()
        {
            Clients.All.acceptGreet("Buenos dias! The time is " + DateTime.Now.ToString());
        }

        public void GenerarQR(string cedula, double valor)
        {
            Clients.All.acceptQR("Hola QR");
        }

        public void SincronizarCompra(string connectionIdCliente)
        {
            //Agrupar clientes (Cliente y beneficiario)
            Groups.Add(connectionIdCliente, connectionIdCliente);
            Groups.Add(Context.ConnectionId, connectionIdCliente);

            Clients.Group(connectionIdCliente).confirmarCompra("Conexión establecida");
        }

        public void FinalizarCompra(Venta venta)
        {
            try
            {
                using (var en = new MoneyPoints_dlloEntities())
                {
                    var compra = new Compra
                    {
                        AfiliadoId = venta.AliadoId,
                        Fecha = DateTime.Now,
                        Valor = venta.Valor,
                        NumeroFactura = Guid.NewGuid().ToString().Split('-')[0], //Cambiar forma de generar numero de factura por un consecutivo,
                        Estado = "Finalizada" //Cambiar para enumeracion
                    };

                    compra.BeneficiariosClientesCompras.Add(new BeneficiariosClientesCompra
                    {
                        BeneficiariosClienteId = venta.BeneficiariosClienteId
                    });

                    compra.DetallesCompras.Add(new DetallesCompra { Valor = venta.Valor });

                    en.Compras.Add(compra);

                    var benefCliente = en.BeneficiariosClientes.Find(venta.BeneficiariosClienteId);

                    //Restamos el saldo
                    benefCliente.Saldo -= venta.Valor;

                    if (benefCliente.Saldo < 0) throw new Exception("Fondos insuficientes para esta compra");

                    //todo: falta restar los puntos, se debe validar contra la equivalencia
                    var equiv = en.Equivalencias.FirstOrDefault(e => e.ClienteId == benefCliente.ClienteId);

                    var rel = equiv.Puntos / equiv.Valor;

                    benefCliente.Puntos = Convert.ToInt32(Math.Round(benefCliente.Puntos * rel, 0));

                    en.Entry(benefCliente).State = EntityState.Modified;

                    en.SaveChanges();
                }

                Clients.Group(venta.ConnectionIdCliente).finalizarCompraCompleted("Compra realizada exitosamente.");
            }
            catch (Exception ex)
            {
                string details = "";
                if (ex.InnerException != null) details = ex.InnerException.Message;
                Clients.Group(venta.ConnectionIdCliente).finalizarCompraCompleted("No se pudo finalizar la compra: " + ex.Message + details);
            }
        }


    }

    public class Venta
    {
        public int AliadoId { get; set; }

        public int BeneficiarioId { get; set; }

        public string ConnectionIdCliente { get; set; }

        public decimal Valor { get; set; }

        public string Pin { get; set; }

        public int BeneficiariosClienteId { get; set; }
    }
}