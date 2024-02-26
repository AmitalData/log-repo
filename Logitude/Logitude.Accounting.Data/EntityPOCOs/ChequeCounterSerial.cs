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
   
    public class ChequeCounterSerial
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("BankAccount")]
        [Column("BankAccountId" ,Order = 1)]
	    public string BankAccountId { get; set; }
	      
        public virtual BankAccount BankAccount { get; set; }
        [Column("SeriesId")]
	    public int SeriesId { get; set; }
        [Column("ChequeCounterBegin")]
	    public int ChequeCounterBegin { get; set; }
        [Column("ChequeCounterEnd")]
	    public int ChequeCounterEnd { get; set; }
    }
}
	 