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

namespace Logitude.Accounting.Data.EntityPOCOs
{
   
    public class AccountingEntitiesJournal
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("AccountingEntityId")]
	    public string AccountingEntityId { get; set; }
        [Column("AccountingEntityCode")]
	    public string AccountingEntityCode { get; set; }
        [Column("Action")]
	    public string Action { get; set; }
     [Key]
        [Column("Id")]
	    public string Id { get; set; }
    }
}
	 