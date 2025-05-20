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
   
    public class MagayaCommunicationLog
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("CommunicationLog")]
        [Column("CommunicationId")]
	    public string CommunicationId { get; set; }
	      
        public virtual CommunicationLog CommunicationLog { get; set; }
        [ForeignKey("MagayaStep")]
        [Column("Step")]
	    public string Step { get; set; }
	      
        public virtual MagayaStep MagayaStep { get; set; }
        [ForeignKey("MagayaStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual MagayaStatus MagayaStatus { get; set; }
        [Column("Exception")]
	    public string Exception { get; set; }
    }
}
	 