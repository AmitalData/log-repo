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
   
    public class DepositCondition
    {
	 string dbms;

        [Key]
        [ForeignKey("Deposit")]
        [Column("DepositId" ,Order = 1)]
	    public string DepositId { get; set; }
	      
        public virtual Deposit Deposit { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("ReturnCondition")]
        [Column("DepositConditionCode" ,Order = 2)]
	    public string DepositConditionCode { get; set; }
	      
        public virtual ReturnCondition ReturnCondition { get; set; }
        [Column("DepositAmount")]
	    public decimal? DepositAmount { get; set; }
    }
}
	 