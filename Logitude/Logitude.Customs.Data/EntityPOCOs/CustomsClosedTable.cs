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
   
    public class CustomsClosedTable
    {
	 string dbms;

        [Key]
        [Column("Id" ,Order = 1)]
	    public string Id { get; set; }
        [Column("CustomsName")]
	    public string CustomsName { get; set; }
        [Column("CustomsLocalName")]
	    public string CustomsLocalName { get; set; }
        [Column("DbName")]
	    public string DbName { get; set; }
        [Column("LastUpdateDate")]
	    public DateTime? LastUpdateDate { get; set; }
        [ForeignKey("ClosedTableStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual ClosedTableStatus ClosedTableStatus { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("ObjectTable")]
        [Column("ObjectTableId" ,Order = 2)]
	    public string ObjectTableId { get; set; }
	      
        public virtual ObjectTable ObjectTable { get; set; }
        [Column("Existed")]
	    public bool Existed { get; set; }
        [Column("RetreiveDateTime")]
	    public DateTime? RetreiveDateTime { get; set; }
    }
}
	 