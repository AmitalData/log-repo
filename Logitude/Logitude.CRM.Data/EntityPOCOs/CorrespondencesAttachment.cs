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
using Logitude.Infrastructure.Data.EntityPOCOs;
namespace Logitude.CRM.Data.EntityPOCOs
{
   
    public class CorrespondencesAttachment
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("DocumentsFiling")]
        [Column("DocumentFilingId")]
	    public string DocumentFilingId { get; set; }
	      
        public virtual DocumentsFiling DocumentsFiling { get; set; }
        [ForeignKey("Correspondence")]
        [Column("CorrespondenceId")]
	    public string CorrespondenceId { get; set; }
	      
        public virtual Correspondence Correspondence { get; set; }
    }
}
	 