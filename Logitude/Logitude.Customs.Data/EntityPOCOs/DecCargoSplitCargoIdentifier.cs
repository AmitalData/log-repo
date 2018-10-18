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
   
    public class DecCargoSplitCargoIdentifier
    {
	 string dbms;

        [Key]
        [ForeignKey("DeclarationCargoSplit")]
        [Column("DeclarationCargoSplitId" ,Order = 1)]
	    public string DeclarationCargoSplitId { get; set; }
	      
        public virtual DeclarationCargoSplit DeclarationCargoSplit { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("LineNumber" ,Order = 2)]
	    public int LineNumber { get; set; }
        [Column("CargoIdentifierKey1")]
	    public string CargoIdentifierKey1 { get; set; }
        [Column("CargoIdentifierKey2")]
	    public string CargoIdentifierKey2 { get; set; }
        [Column("CargoIdentifierKey3")]
	    public string CargoIdentifierKey3 { get; set; }
    }
}
	 