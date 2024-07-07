using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class DeclarationReferantDataMap : EntityTypeConfiguration<DeclarationReferantData>
    {
	    string dbms;
        public DeclarationReferantDataMap()
        { 
			  this.ToTable("DeclarationReferantDatas", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.OrderNumber).HasColumnName("OrderNumber").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.VendorId).HasColumnName("VendorId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ArrivalDate).HasColumnName("ArrivalDate");

            this.Property(t => t.EstimatedArrivalDate).HasColumnName("EstimatedArrivalDate");

            this.Property(t => t.Weight).HasColumnName("Weight").HasPrecision(15, 3);

            this.Property(t => t.ClassificationStatus).HasColumnName("ClassificationStatus").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.ControllerStatus).HasColumnName("ControllerStatus").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CollectionOfMoneyStatus).HasColumnName("CollectionOfMoneyStatus").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.FollowUpDate).HasColumnName("FollowUpDate");

            this.Property(t => t.WithPaper).HasColumnName("WithPaper");

            this.Property(t => t.IsClosedForFollowUp).HasColumnName("IsClosedForFollowUp").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsClassificationRemarks).HasColumnName("IsClassificationRemarks");

            this.Property(t => t.IsControllerRemarks).HasColumnName("IsControllerRemarks");

            this.Property(t => t.PreClassification).HasColumnName("PreClassification").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.ExceptionReasonsList).HasColumnName("ExceptionReasonsList").HasMaxLength(1000).IsUnicode(false);

            this.Property(t => t.ClassifiedUserId).HasColumnName("ClassifiedUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ControllerUserId).HasColumnName("ControllerUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CollectorUserId).HasColumnName("CollectorUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NewFile).HasColumnName("NewFile");

            this.Property(t => t.Favorite).HasColumnName("Favorite");

            this.Property(t => t.LastStatusName).HasColumnName("LastStatusName").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.LastStatusDate).HasColumnName("LastStatusDate");

            this.Property(t => t.OrderMoney).HasColumnName("OrderMoney");

            this.Property(t => t.Team).HasColumnName("Team").HasMaxLength(15).IsUnicode(true);

            this.Property(t => t.ImporterFile).HasColumnName("ImporterFile").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.FileOpenDate).HasColumnName("FileOpenDate");

            this.Property(t => t.FclLcl).HasColumnName("FclLcl").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.PackageQuantity).HasColumnName("PackageQuantity");

            this.Property(t => t.ForwarderId).HasColumnName("ForwarderId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsManualPayment).HasColumnName("IsManualPayment");

            this.Property(t => t.PackageTypeCode).HasColumnName("PackageTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Commodity).HasColumnName("Commodity").HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.LastStatusRemarks).HasColumnName("LastStatusRemarks").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.Mawb).HasColumnName("Mawb").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Hawb).HasColumnName("Hawb").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.ImporterApproval).HasColumnName("ImporterApproval");

            this.Property(t => t.DeclarationIdToDisplay).HasColumnName("DeclarationIdToDisplay").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MawbDate).HasColumnName("MawbDate");

            this.Property(t => t.Vessel).HasColumnName("Vessel").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FlightVoyageNumber).HasColumnName("FlightVoyageNumber").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.CarrierCode).HasColumnName("CarrierCode").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 