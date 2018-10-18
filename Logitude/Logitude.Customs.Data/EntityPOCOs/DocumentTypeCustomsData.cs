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
   
    public class DocumentTypeCustomsData
    {
	 string dbms;

        [Key]
        [Column("DocumentTypeId")]
	    public string DocumentTypeId { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("CustomDocumentType")]
        [Column("CustomsDoucumentTypeCode")]
	    public string CustomsDoucumentTypeCode { get; set; }
	      
        public virtual CustomDocumentType CustomDocumentType { get; set; }
    }
}
	 