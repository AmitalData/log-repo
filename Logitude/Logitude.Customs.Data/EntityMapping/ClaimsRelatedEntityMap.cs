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
 
    public class ClaimsRelatedEntityMap : EntityTypeConfiguration<ClaimsRelatedEntity>
    {
	    string dbms;
        public ClaimsRelatedEntityMap()
        { 
			  this.ToTable("ClaimsRelatedEntities", "Customs");
		
		    this.HasKey(t => new { t.ClaimId, t.EntityCounterKey });
	 
            this.Property(t => t.ClaimId).HasColumnName("ClaimId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.EntityCounterKey).HasColumnName("EntityCounterKey").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.ClaimEntityTypeCode).HasColumnName("ClaimEntityTypeCode").IsRequired().HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.ClaimEntityNumber).HasColumnName("ClaimEntityNumber").IsRequired().HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.ExternalClaimNumber).HasColumnName("ExternalClaimNumber").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CourtCode).HasColumnName("CourtCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ProceedingNumber).HasColumnName("ProceedingNumber").HasMaxLength(22).IsUnicode(false);

            this.Property(t => t.IsFinancialRefundDemand).HasColumnName("IsFinancialRefundDemand");

            this.Property(t => t.SeconderyClaimEntityCode).HasColumnName("SeconderyClaimEntityCode").HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.SeconderyClaimEntityID).HasColumnName("SeconderyClaimEntityID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.ClaimAmount).HasColumnName("ClaimAmount").HasPrecision(16, 2);

            this.Property(t => t.DeclarationVersion).HasColumnName("DeclarationVersion").HasPrecision(5, 3);

            this.Property(t => t.CommitteeDecisionNumber).HasColumnName("CommitteeDecisionNumber").HasMaxLength(22).IsUnicode(false);

            this.Property(t => t.AbandonmentDestructionReferenc).HasColumnName("AbandonmentDestructionReferenc").HasMaxLength(22).IsUnicode(false);

            this.Property(t => t.WarehouseTypeCode).HasColumnName("WarehouseTypeCode").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.ClaimExplanation).HasColumnName("ClaimExplanation").IsRequired().HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.ContinuousMessagesTypeCode).HasColumnName("ContinuousMessagesTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ClaimRequestNumber).HasColumnName("ClaimRequestNumber").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.CustomsExceptions).HasColumnName("CustomsExceptions").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.TapagNumber).HasColumnName("TapagNumber").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Numeral).HasColumnName("Numeral");

            this.Property(t => t.CustomsBranchCode).HasColumnName("CustomsBranchCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.DecisionCode).HasColumnName("DecisionCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DecisionNote).HasColumnName("DecisionNote").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.EilatVatRefoundDecision).HasColumnName("EilatVatRefoundDecision").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.DepositingAmount).HasColumnName("DepositingAmount").HasPrecision(16, 2);

            this.Property(t => t.RefundAmount).HasColumnName("RefundAmount").HasPrecision(16, 2);

            this.Property(t => t.ContinuousRequestTypeCode).HasColumnName("ContinuousRequestTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Explanation).HasColumnName("Explanation").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.Note).HasColumnName("Note").HasMaxLength(256).IsUnicode(true);
        }
    }
}
	 