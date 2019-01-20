using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AccountManagerUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SalesmanUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.RankId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BillToId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Field1).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field2).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field3).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field4).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field5).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field6).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field7).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field8).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field9).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field10).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.IndustryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LeadSourceId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LeadDescription).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.CustomerStatusCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.ClassifierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CollectorId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FreelancerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ForwarderId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CustomsAgentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MediatorId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BeforeDeactiveStatusCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.RegionId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CustomerSizeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ActivityWatch).IsRequired();
            this.Property(t => t.KnownConsignor).HasMaxLength(9).IsUnicode(false);
            this.Property(t => t.CompetitorFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.PrimaryContactName).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.PrimaryContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.PrimaryContactPhone).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ActivatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SetAsInactiveByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ActivationRequestedByUserId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Customers");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AccountManagerUserId).HasColumnName("AccountManagerUserId");
            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId");
            this.Property(t => t.RankId).HasColumnName("RankId");
            this.Property(t => t.StartWorkingDate).HasColumnName("StartWorkingDate");
            this.Property(t => t.StartWorkingManuallySet).HasColumnName("StartWorkingManuallySet");
            this.Property(t => t.LastShipmentDate).HasColumnName("LastShipmentDate");
            this.Property(t => t.BillToId).HasColumnName("BillToId");
            this.Property(t => t.Field1).HasColumnName("Field1");
            this.Property(t => t.Field2).HasColumnName("Field2");
            this.Property(t => t.Field3).HasColumnName("Field3");
            this.Property(t => t.Field4).HasColumnName("Field4");
            this.Property(t => t.Field5).HasColumnName("Field5");
            this.Property(t => t.Field6).HasColumnName("Field6");
            this.Property(t => t.Field7).HasColumnName("Field7");
            this.Property(t => t.Field8).HasColumnName("Field8");
            this.Property(t => t.Field9).HasColumnName("Field9");
            this.Property(t => t.Field10).HasColumnName("Field10");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
            this.Property(t => t.LeadSourceId).HasColumnName("LeadSourceId");
            this.Property(t => t.CreditLimit).HasColumnName("CreditLimit");
            this.Property(t => t.LeadDescription).HasColumnName("LeadDescription");
            this.Property(t => t.IsCustomer).HasColumnName("IsCustomer");
            this.Property(t => t.CustomerStatusCode).HasColumnName("CustomerStatusCode");
            this.Property(t => t.CollectorId).HasColumnName("CollectorId");
            this.Property(t => t.ClassifierId).HasColumnName("ClassifierId");
            this.Property(t => t.MediatorId).HasColumnName("MediatorId");
            this.Property(t => t.FreelancerId).HasColumnName("FreelancerId");
            this.Property(t => t.ForwarderId).HasColumnName("ForwarderId");
            this.Property(t => t.CustomsAgentId).HasColumnName("CustomsAgentId");
            this.Property(t => t.BeforeDeactiveStatusCode).HasColumnName("BeforeDeactiveStatusCode");
            this.Property(t => t.ReadyForActivationDate).HasColumnName("ReadyForActivationDate");
            this.Property(t => t.RegionId).HasColumnName("RegionId");
            this.Property(t => t.CustomerSizeId).HasColumnName("CustomerSizeId");
            this.Property(t => t.LastOpportunityDate).HasColumnName("LastOpportunityDate");
            this.Property(t => t.LastMeetingDate).HasColumnName("LastMeetingDate");
            this.Property(t => t.LastCallDate).HasColumnName("LastCallDate");
            this.Property(t => t.FirstShipmentDate).HasColumnName("FirstShipmentDate");
            this.Property(t => t.FirstInvoiceDate).HasColumnName("FirstInvoiceDate");
            this.Property(t => t.LastQuoteDate).HasColumnName("LastQuoteDate");
            this.Property(t => t.LastInteractionDate).HasColumnName("LastInteractionDate");
            this.Property(t => t.ActivityWatch).HasColumnName("ActivityWatch");
            this.Property(t => t.KnownConsignor).HasColumnName("KnownConsignor");
            this.Property(t => t.KCExpirationDate).HasColumnName("KCExpirationDate");
            this.Property(t => t.LogBoxActivated).HasColumnName("LogBoxActivated");
            this.Property(t => t.IsPrivateLabelCustomer).HasColumnName("IsPrivateLabelCustomer");
            this.Property(t => t.IsCreditLimitEnabled).HasColumnName("IsCreditLimitEnabled");
            this.Property(t => t.CreditLimitAmount).HasColumnName("CreditLimitAmount");
            this.Property(t => t.CreditLimitOpenBalance).HasColumnName("CreditLimitOpenBalance");
            this.Property(t => t.CreditLimitWarningPercentage).HasColumnName("CreditLimitWarningPercentage");
            this.Property(t => t.BlockNewInvoiceCreation).HasColumnName("BlockNewInvoiceCreation");
            this.Property(t => t.BlockNewShipmentCreation).HasColumnName("BlockNewShipmentCreation");
            this.Property(t => t.CompetitorFields).HasColumnName("CompetitorFields");
            this.Property(t => t.PrimaryContactName).HasColumnName("PrimaryContactName");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.PrimaryContactPhone).HasColumnName("PrimaryContactPhone");
            this.Property(t => t.ActivationDate).HasColumnName("ActivationDate");
            this.Property(t => t.InactiveDate).HasColumnName("InactiveDate");
            this.Property(t => t.ActivationRequestDate).HasColumnName("ActivationRequestDate");
            this.Property(t => t.ActivatedByUserId).HasColumnName("ActivatedByUserId");
            this.Property(t => t.SetAsInactiveByUserId).HasColumnName("SetAsInactiveByUserId");
            this.Property(t => t.ActivationRequestedByUserId).HasColumnName("ActivationRequestedByUserId");

            // Relationships
            this.HasOptional(t => t.BillToCard).WithMany().HasForeignKey(d => d.BillToId);
            this.HasRequired(t => t.Card).WithOptional(t => t.Customer);
            this.HasOptional(t => t.AccountManagerUser).WithMany().HasForeignKey(d => d.AccountManagerUserId);
            this.HasOptional(t => t.SalesmanUser).WithMany().HasForeignKey(d => d.SalesmanUserId);
            this.HasOptional(t => t.Rank).WithMany().HasForeignKey(d => d.RankId);
            this.HasOptional(t => t.LeadSource).WithMany().HasForeignKey(d => d.LeadSourceId);
            this.HasOptional(t => t.Industry).WithMany().HasForeignKey(d => d.IndustryId);
            this.HasOptional(t => t.CustomerStatus).WithMany().HasForeignKey(d => d.CustomerStatusCode);
            this.HasOptional(t => t.Collector).WithMany().HasForeignKey(d => d.CollectorId);
            this.HasOptional(t => t.Classifier).WithMany().HasForeignKey(d => d.ClassifierId);
            this.HasOptional(t => t.Mediator).WithMany().HasForeignKey(d => d.MediatorId);
            this.HasOptional(t => t.CustomsAgent).WithMany().HasForeignKey(d => d.CustomsAgentId);
            this.HasOptional(t => t.Forwarder).WithMany().HasForeignKey(d => d.ForwarderId);
            this.HasOptional(t => t.Freelancer).WithMany().HasForeignKey(d => d.FreelancerId);
            this.HasOptional(t => t.BeforeDeactiveStatus).WithMany().HasForeignKey(d => d.BeforeDeactiveStatusCode);
            this.HasOptional(t => t.CustomerSize).WithMany().HasForeignKey(d => d.CustomerSizeId);
            this.HasOptional(t => t.ActivatedByUser).WithMany().HasForeignKey(d => d.ActivatedByUserId);
            this.HasOptional(t => t.SetAsInactiveByUser).WithMany().HasForeignKey(d => d.SetAsInactiveByUserId);
            this.HasOptional(t => t.ActivationRequestedByUser).WithMany().HasForeignKey(d => d.ActivationRequestedByUserId);
        }
    }
}
