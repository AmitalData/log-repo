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
   
    public class TariffVersionUploadedExcel
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("UploadDate")]
	    public DateTime UploadDate { get; set; }
        [ForeignKey("UploadedByUser")]
        [Column("UploadedByUserId")]
	    public string UploadedByUserId { get; set; }
	      
        public virtual User UploadedByUser { get; set; }
        [ForeignKey("Tariff")]
        [Column("TariffId")]
	    public string TariffId { get; set; }
	      
        public virtual Tariff Tariff { get; set; }
        [Column("Version")]
	    public int Version { get; set; }
        [ForeignKey("Document")]
        [Column("DocumentId")]
	    public string DocumentId { get; set; }
	      
        public virtual Document Document { get; set; }
        [Column("NumberOfLines")]
	    public int NumberOfLines { get; set; }
        [Column("Index")]
	    public int Index { get; set; }
    }
}
	 