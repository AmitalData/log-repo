using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class CourierDeclaration
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("Declaration")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
     [Key]
        [ForeignKey("CourierMaster")]
        [Column("CourierMasterId" ,Order = 2)]
	    public string CourierMasterId { get; set; }
	      
        public virtual CourierMaster CourierMaster { get; set; }
        [Column("SequenceNumeric")]
	    public int? SequenceNumeric { get; set; }
    }
}
	 