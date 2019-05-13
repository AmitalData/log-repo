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

namespace Logitude.TariffModule.Data.EntityPOCOs
{
   
    public class TariffVersion
    {
	 string dbms;

        [Key]
        [Column("TariffId")]
	    public string TariffId { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("ExpirationDate")]
	    public DateTime? ExpirationDate { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
     [Key]
        [Column("Version")]
	    public int Version { get; set; }
        [Column("IsDraft")]
	    public bool IsDraft { get; set; }
        [Column("ApproveDate")]
	    public DateTime? ApproveDate { get; set; }
        [ForeignKey("ApprovedByUser")]
        [Column("ApprovedByUserId")]
	    public string ApprovedByUserId { get; set; }
	      
        public virtual User ApprovedByUser { get; set; }
        [Column("ParentVersionId")]
	    public string ParentVersionId { get; set; }
    }
}
	 