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
   
    public class PendingByKeyword
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("CourierPendingReason")]
        [Column("CourierPendingReasonCode")]
	    public string CourierPendingReasonCode { get; set; }
	      
        public virtual CourierPendingReason CourierPendingReason { get; set; }
        [Column("KeywordsList")]
	    public string KeywordsList { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("SearchByFieldCode")]
	    public string SearchByFieldCode { get; set; }
    }
}
	 