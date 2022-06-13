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
   
    public class AvailableStatusField
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("FieldCode")]
	    public string FieldCode { get; set; }
        [Column("IsAvailable")]
	    public bool IsAvailable { get; set; }
        [ForeignKey("StatusFieldTypeEntity")]
        [Column("StatusFieldType")]
	    public string StatusFieldType { get; set; }
	      
        public virtual StatusFieldType StatusFieldTypeEntity { get; set; }
    }
}
	 