using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DecTaxesByTaxTypeCodeView
    {
        [Key]
        public string DeclarationId { get; set; }
        public int Tenant { get; set; }   
        public decimal MehesFee { get; set; }
        public decimal VATReshimonFee { get; set; }
        public decimal SecurityFee { get; set; }
        public decimal ComputerFee { get; set; }

    }
}
