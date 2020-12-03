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
   
    public class CustomsRequiredField
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("ObjectTable")]
        [Column("ObjectTableId")]
	    public string ObjectTableId { get; set; }
	      
        public virtual ObjectTable ObjectTable { get; set; }
        [ForeignKey("ObjectField")]
        [Column("ObjectfieldId")]
	    public string ObjectfieldId { get; set; }
	      
        public virtual ObjectField ObjectField { get; set; }
        [Column("ObjectfieldCode")]
	    public string ObjectfieldCode { get; set; }
        [Column("IsImport")]
	    public bool? IsImport { get; set; }
        [Column("IsExport")]
	    public bool? IsExport { get; set; }
    }
}
	 