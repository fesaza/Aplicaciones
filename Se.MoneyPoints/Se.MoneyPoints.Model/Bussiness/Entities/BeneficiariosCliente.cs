//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Se.MoneyPoints.Model.Bussiness.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BeneficiariosCliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BeneficiariosCliente()
        {
            this.BeneficiariosClientesCompras = new HashSet<BeneficiariosClientesCompra>();
            this.BeneficiariosPuntos = new HashSet<BeneficiariosPunto>();
        }
    
        public int BeneficiariosClienteId { get; set; }
        public int ClienteId { get; set; }
        public int BeneficiarioId { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public Nullable<System.DateTime> FechaBaja { get; set; }
        public decimal Saldo { get; set; }
        public int Puntos { get; set; }
    
        public virtual Beneficiario Beneficiario { get; set; }
        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BeneficiariosClientesCompra> BeneficiariosClientesCompras { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BeneficiariosPunto> BeneficiariosPuntos { get; set; }
    }
}