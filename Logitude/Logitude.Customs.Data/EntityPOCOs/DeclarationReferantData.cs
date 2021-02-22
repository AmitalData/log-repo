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
	    public DateTime? ArrivalDate { get; set; }
        [Column("EstimatedArrivalDate")]
	    public DateTime? EstimatedArrivalDate { get; set; }
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
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("ExceptionReasonsList")]
	    public string ExceptionReasonsList { get; set; }
        [ForeignKey("ClassifiedUser")]
        [Column("ClassifiedUserId")]
	    public string ClassifiedUserId { get; set; }
	      
        public virtual User ClassifiedUser { get; set; }
        [ForeignKey("ControllerUser")]
        [Column("ControllerUserId")]
	    public string ControllerUserId { get; set; }
	      
        public virtual User ControllerUser { get; set; }
        [ForeignKey("CollectorUser")]
        [Column("CollectorUserId")]
	    public string CollectorUserId { get; set; }
	      
        public virtual User CollectorUser { get; set; }
        [Column("NewFile")]
	    public bool NewFile { get; set; }
        [Column("Favorite")]
	    public bool Favorite { get; set; }
        [Column("LastStatusName")]
	    public string LastStatusName { get; set; }
        [Column("LastStatusDate")]
	    public DateTime? LastStatusDate { get; set; }
        [Column("OrderMoney")]
	    public bool OrderMoney { get; set; }
        [ForeignKey("ReferantTeam")]
        [Column("Team")]
	    public string Team { get; set; }
	      
        public virtual ReferantTeam ReferantTeam { get; set; }
        [Column("ImporterFile")]
	    public string ImporterFile { get; set; }
        [Column("FileOpenDate")]
	    public DateTime? FileOpenDate { get; set; }
        [ForeignKey("FclLclCodeTable")]
        [Column("FclLcl")]
	    public string FclLcl { get; set; }
	      
        public virtual FclLclCode FclLclCodeTable { get; set; }
        [Column("PackageQuantity")]
	    public int? PackageQuantity { get; set; }
        [ForeignKey("ForwarderCard")]
        [Column("ForwarderId")]
	    public string ForwarderId { get; set; }
	      
        public virtual Card ForwarderCard { get; set; }
    }
}
	 