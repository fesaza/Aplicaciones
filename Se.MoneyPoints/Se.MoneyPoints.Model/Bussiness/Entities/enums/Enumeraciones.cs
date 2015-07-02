using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se.MoneyPoints.Model.Bussiness.Entities.enums
{
    public enum EstadosConciliaciones
    {
        Enviada,
        Rechazada,
        Aceptada
    }

    public enum Roles
    {
        Administrador = 1,
        Cliente = 2,
        Beneficiario = 4
    }
}
