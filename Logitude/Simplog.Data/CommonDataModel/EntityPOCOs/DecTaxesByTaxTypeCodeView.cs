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
        public int MehesFee { get; set; }
        public int VATReshimonFee { get; set; }
        public int SecurityFee { get; set; }
        public int ComputerFee { get; set; }

    }
}
