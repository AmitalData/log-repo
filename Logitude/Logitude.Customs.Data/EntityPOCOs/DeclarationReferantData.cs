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
   
    public class DeclarationReferantData
    {
	 string dbms;

        [Key]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("OrderNumber")]
	    public string OrderNumber { get; set; }
        [ForeignKey("CustomsVendor")]
        [Column("VendorId")]
	    public string VendorId { get; set; }
	      
        public virtual CustomsVendor CustomsVendor { get; set; }
        [Column("ArrivalDate")]
	    public DateTime ArrivalDate { get; set; }
        [Column("EstimatedArrivalDate")]
	    public DateTime EstimatedArrivalDate { get; set; }
        [Column("Weight")]
	    public decimal? Weight { get; set; }
        [Column("ClassificationStatus")]
	    public string ClassificationStatus { get; set; }
        [Column("ControllerStatus")]
	    public string ControllerStatus { get; set; }
        [Column("CollectionOfMoneyStatus")]
	    public string CollectionOfMoneyStatus { get; set; }
        [Column("FollowUpDate")]
	    public DateTime? FollowUpDate { get; set; }
        [Column("IsExceptional")]
	    public bool IsExceptional { get; set; }
        [Column("WithPaper")]
	    public bool WithPaper { get; set; }
        [Column("IsClosedForFollowUp")]
	    public string IsClosedForFollowUp { get; set; }
        [Column("IsClassificationRemarks")]
	    public bool IsClassificationRemarks { get; set; }
        [Column("IsControllerRemarks")]
	    public bool IsControllerRemarks { get; set; }
        [Column("PreClassification")]
	    public string PreClassification { get; set; }
    }
}
	 