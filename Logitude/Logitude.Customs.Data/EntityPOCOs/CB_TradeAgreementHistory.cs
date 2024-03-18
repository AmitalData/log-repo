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
   
    public class CB_TradeAgreementHistory
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public int ID { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime? EndDate { get; set; }
        [ForeignKey("CustomsEntityStatus")]
        [Column("EntityStatusID")]
	    public string EntityStatusID { get; set; }
	      
        public virtual CustomsEntityStatus CustomsEntityStatus { get; set; }
        [Column("Version")]
	    public int? Version { get; set; }
        [ForeignKey("CB_TradeAgreementId")]
        [Column("TradeAgreementID")]
	    public int TradeAgreementID { get; set; }
	      
        public virtual CB_TradeAgreement CB_TradeAgreementId { get; set; }
        [Column("ChangeRequestTypePriority")]
	    public int ChangeRequestTypePriority { get; set; }
    }
}
	 