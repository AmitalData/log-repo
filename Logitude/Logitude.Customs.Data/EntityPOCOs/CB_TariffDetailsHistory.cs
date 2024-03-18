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
   
    public class CB_TariffDetailsHistory
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public string ID { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("TariffID")]
	    public string TariffID { get; set; }
        [ForeignKey("Quota")]
        [Column("QuotaID")]
	    public string QuotaID { get; set; }
	      
        public virtual CB_Quota Quota { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime? EndDate { get; set; }
        [ForeignKey("EntityStatusCode")]
        [Column("EntityStatusID")]
	    public string EntityStatusID { get; set; }
	      
        public virtual CustomsEntityStatus EntityStatusCode { get; set; }
        [ForeignKey("WithinQuota_ComputationMethodDataID")]
        [Column("WithinQuota_ComputMethDataID")]
	    public string WithinQuota_ComputMethDataID { get; set; }
	      
        public virtual CB_ComputationMethodData WithinQuota_ComputationMethodDataID { get; set; }
        [ForeignKey("WithoutQuota_ComputationMethodDataID")]
        [Column("WithoutQuota_ComputMethDataID")]
	    public string WithoutQuota_ComputMethDataID { get; set; }
	      
        public virtual CB_ComputationMethodData WithoutQuota_ComputationMethodDataID { get; set; }
        [Column("ChangeRequestTypePriority")]
	    public int ChangeRequestTypePriority { get; set; }
    }
}
	 