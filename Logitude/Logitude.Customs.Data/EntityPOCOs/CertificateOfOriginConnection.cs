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
   
    public class CertificateOfOriginConnection
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("CooStatusCodeEnum")]
        [Column("CooStatus")]
	    public string CooStatus { get; set; }
	      
        public virtual CertificateOfOriginStatusCodeEnum CooStatusCodeEnum { get; set; }
        [ForeignKey("CooReasonCodeEnum")]
        [Column("CooReason")]
	    public string CooReason { get; set; }
	      
        public virtual RequestReasonCodeEnum CooReasonCodeEnum { get; set; }
        [Column("Active")]
	    public bool Active { get; set; }
    }
}
	 