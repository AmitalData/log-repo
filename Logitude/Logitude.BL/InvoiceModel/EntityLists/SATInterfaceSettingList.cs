using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class SATInterfaceSettingList
    {
        [Key]
        public int Tenant { get; set; }
        public string SATInterfaceCode { get; set; }
        public string Token { get; set; }
        public string CompanyId { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string MetodoPagoCode { get; set; }

    }
}
