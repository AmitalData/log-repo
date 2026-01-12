using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
   public  class MasavInterface
    {
       [Key]
       public string Id { get; set; }
       public int Tenant { get; set; }
       public string CreatedByUserId { get; set; }
       public string UpdatedByUserId { get; set; }
       public string Name { get; set; }
       public DateTime? CreateDate { get; set; }
       public DateTime? UpdateDate { get; set; }
       public DateTime? FromDate { get; set; }
       public DateTime? ToDate { get; set; }
       public DateTime? PaymentDate { get; set; }
       public string SearchFields { get; set; }
        [ForeignKey("StatusCode")]
        public virtual MasavInterfaceStatus Status { get; set; }
        public string StatusCode { get; set; }




    }
}
