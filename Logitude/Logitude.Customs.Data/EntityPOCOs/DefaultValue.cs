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
   
    public class DefaultValue
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("DefaultType")]
        [Column("DefaultTypeId")]
	    public string DefaultTypeId { get; set; }
	      
        public virtual DefaultType DefaultType { get; set; }
        [Column("Distr")]
	    public string Distr { get; set; }
        [ForeignKey("Branch")]
        [Column("BranchId")]
	    public string BranchId { get; set; }
	      
        public virtual Branch Branch { get; set; }
        [Column("CardId")]
	    public string CardId { get; set; }
        [Column("ShortValue")]
	    public string ShortValue { get; set; }
        [Column("DefValue")]
	    public string DefValue { get; set; }
    }
}
	 