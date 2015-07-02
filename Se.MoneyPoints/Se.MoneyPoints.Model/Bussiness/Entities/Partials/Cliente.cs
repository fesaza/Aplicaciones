using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se.MoneyPoints.Model.Bussiness.Entities
{
    public partial class Cliente
    {
        private string _NombreCliente;
        public string NombreCliente
        {
            get
            {
                if (this.Tercero != null && this.Tercero.Nombre != null)
                    _NombreCliente = Tercero.Nombre;
                else
                    _NombreCliente = string.Empty;
                return _NombreCliente;
            }
            set
            {
                _NombreCliente = value;
            }
        }
    }
    public partial class ClienteBad
    {
        public bool IsBeneficiario { get; set; }
    }
}
