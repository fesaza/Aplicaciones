using Microsoft.VisualStudio.TestTools.UnitTesting;
using Se.MoneyPoints.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se.MoneyPoints.Api.Controllers.Tests
{
    [TestClass()]
    public class BeneficiariosClientesControllerTests
    {
        [TestMethod()]
        public void GetCuentasByAliadoAndBeneficiarioTest()
        {
            BeneficiariosClientesController controller = new BeneficiariosClientesController();

            var data = controller.GetCuentasByAliadoAndBeneficiario(3, 1);

            Assert.IsTrue(data.ToList().Count > 0);
        }
    }
}