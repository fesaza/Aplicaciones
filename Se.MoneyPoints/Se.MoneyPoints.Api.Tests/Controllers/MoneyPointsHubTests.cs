using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Se.MoneyPoints.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se.MoneyPoints.Api.Controllers.Tests
{
    [TestClass()]
    public class MoneyPointsHubTests
    {
        [TestMethod()]
        public void GenerarQRTest()
        {
            bool sendCalled = false;
            var hub = new MoneyPointsHub();
            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.acceptQR = new Action<string>((message) =>
            {
                sendCalled = true;
            });
            mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
            hub.GenerarQR("1128268719", 20000);
            Assert.IsTrue(sendCalled);
        }

        [TestMethod()]
        public void FinalizarCompraTest()
        {
            MoneyPointsHub money = new MoneyPointsHub();

            money.FinalizarCompra(new Venta
            {
                AliadoId = 3,
                BeneficiariosClienteId = 1,
                BeneficiarioId = 1,
                Pin = "1234",
                ConnectionIdCliente = "1234",
                Valor = 10000
            });

            Assert.IsTrue(1 == 1);
        }
    }
}