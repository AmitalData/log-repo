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
 
    public class DeclarationCourierStatusMap : EntityTypeConfiguration<DeclarationCourierStatus>
    {
	    string dbms;
        public DeclarationCourierStatusMap()
        { 
			  this.ToTable("DeclarationCourierStatuses", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.CourierManifestStatusCode).HasColumnName("CourierManifestStatusCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CourierDeclarationStatusCode).HasColumnName("CourierDeclarationStatusCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CourierPaymentStatusCode).HasColumnName("CourierPaymentStatusCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsCourierMissingClassification).HasColumnName("IsCourierMissingClassification");

            this.Property(t => t.IsClosedForFollowUp).HasColumnName("IsClosedForFollowUp");

            this.Property(t => t.HighLowValue).HasColumnName("HighLowValue").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.DocumentStatusCode).HasColumnName("DocumentStatusCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.TotalInvoiceAmountInUSD).HasColumnName("TotalInvoiceAmountInUSD").HasPrecision(16, 2);

            this.Property(t => t.CourierPendingReasonCode).HasColumnName("CourierPendingReasonCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PendingRemarks).HasColumnName("PendingRemarks").HasMaxLength(1024).IsUnicode(true);

            this.Property(t => t.SpecialActionStatus).HasColumnName("SpecialActionStatus").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.FastIndividualProcessCode).HasColumnName("FastIndividualProcessCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ManualProcessCode).HasColumnName("ManualProcessCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.TerminalSuspentionNumber).HasColumnName("TerminalSuspentionNumber").HasMaxLength(6).IsUnicode(false);

            this.Property(t => t.LastMileStatusCode).HasColumnName("LastMileStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.LastMileStatusDate).HasColumnName("LastMileStatusDate");

            this.Property(t => t.LastMileStatusRemarks).HasColumnName("LastMileStatusRemarks").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.StorageSiteStatusCode).HasColumnName("StorageSiteStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.StorageSiteErrorText).HasColumnName("StorageSiteErrorText").HasMaxLength(1200).IsUnicode(true);

            this.Property(t => t.CourierPendingReasonList).HasColumnName("CourierPendingReasonList").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.LastMileStatusName).HasColumnName("LastMileStatusName").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.Delivered).HasColumnName("Delivered");

            this.Property(t => t.TruckerId).HasColumnName("TruckerId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 