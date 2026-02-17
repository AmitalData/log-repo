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
   
    public class DeclarationErrorMapping
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("DocumentSectionCode")]
	    public string DocumentSectionCode { get; set; }
        [Column("TagID")]
	    public string TagID { get; set; }
        [Column("Field")]
	    public string Field { get; set; }
        [Column("Entity")]
	    public string Entity { get; set; }
        [Column("Skip")]
	    public bool Skip { get; set; }
    }
}
	 