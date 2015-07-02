using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se.MoneyPoints.Model.Bussiness.Entities
{
    public partial class AfiliadosCliente
    {
        private string _NombreAfiliado;
        public string NombreAfiliado
        {
            get
            {
                if (this.Cliente != null && this.Cliente.Tercero != null)
                    _NombreAfiliado = Cliente.Tercero.Nombre;
                else
                    _NombreAfiliado = string.Empty;
                return _NombreAfiliado;
            }
            set
            {
                _NombreAfiliado = value;
            }
        }
    }
}
