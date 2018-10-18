using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class VATTypesGroup
    {
        [Key]
        public string GroupVATTypeId { get; set; }
        [Key]
        public string SingleVATTypeId { get; set; }

        public int Tenant { get; set; }

        [ForeignKey("GroupVATTypeId")]
        public virtual VatType GroupVATType { get; set; }

        [ForeignKey("SingleVATTypeId")]
        public virtual VatType SingleVATType { get; set; }
    }
}
