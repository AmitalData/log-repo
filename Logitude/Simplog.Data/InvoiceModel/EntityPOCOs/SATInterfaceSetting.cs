using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class SATInterfaceSetting
    {
        [Key]
        public int Tenant { get; set; }
        public string SATInterfaceCode { get; set; }
        public string Token { get; set; }
        public DateTime? ActivationDate { get; set; }
        public bool IsARInvoiceTransferEnabled { get; set; }
        public bool IsCartaPorteTransferEnabled { get; set; }

        [ForeignKey("SATInterfaceCode")]
        public virtual SATInterface SATInterface { get; set; }

        public string MetodoPagoCode { get; set; }

        [ForeignKey("MetodoPagoCode")]
        public virtual MetodoPago MetodoPago { get; set; }
        public string SATCompanyName { get; set; }
        public bool TransferExpenseCharges { get; set; }
    }
}
