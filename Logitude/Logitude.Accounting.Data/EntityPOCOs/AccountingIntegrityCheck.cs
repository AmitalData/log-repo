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

namespace Logitude.Accounting.Data.EntityPOCOs
{
   
    public class AccountingIntegrityCheck
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDateTimeUTC")]
	    public DateTime CreateDateTimeUTC { get; set; }
        [ForeignKey("IntegrityCheckStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual IntegrityCheckStatus IntegrityCheckStatus { get; set; }
        [Column("ParametersXML")]
	    public string ParametersXML { get; set; }
        [Column("ResultXML")]
	    public string ResultXML { get; set; }
        [Column("HasException")]
	    public bool HasException { get; set; }
        [Column("DoneDateTimeUTC")]
	    public DateTime? DoneDateTimeUTC { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("ShouldFix")]
	    public bool ShouldFix { get; set; }
    }
}
	 