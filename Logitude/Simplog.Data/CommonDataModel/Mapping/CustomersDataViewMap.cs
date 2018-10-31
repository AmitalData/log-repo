using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomersDataViewMap : EntityTypeConfiguration<CustomersDataView>
    {
        public CustomersDataViewMap()
        {
            this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
         
            // Table & Column Mappings
            this.ToTable("CustomersDataView");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ReadyForActivationDate).HasColumnName("ReadyForActivationDate");
            this.Property(t => t.PayablesAccountingCard).HasColumnName("PayablesAccountingCard");
            this.Property(t => t.AccountManagerUserEnglishName).HasColumnName("AccountManagerUserEnglishName");
            this.Property(t => t.BeforeDeactiveStatusCode).HasColumnName("BeforeDeactiveStatusCode");
            this.Property(t => t.BillToId).HasColumnName("BillToId");
            this.Property(t => t.BillToName).HasColumnName("BillToName");            
            this.Property(t => t.ClassifierId).HasColumnName("ClassifierId");
            this.Property(t => t.ClassifierName).HasColumnName("ClassifierName");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CollectorId).HasColumnName("CollectorId");
            this.Property(t => t.CollectorName).HasColumnName("CollectorName");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.CreatedByUserName).HasColumnName("CreatedByUserName");
            this.Property(t => t.CustomerStatusCode).HasColumnName("CustomerStatusCode");
            this.Property(t => t.CustomerStatusName).HasColumnName("CustomerStatusName");
            this.Property(t => t.CustomsAgentId).HasColumnName("CustomsAgentId");
            this.Property(t => t.CustomsAgentName).HasColumnName("CustomsAgentName");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
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
            this.Property(t => t.ForwarderId).HasColumnName("ForwarderId");
            this.Property(t => t.ForwarderName).HasColumnName("ForwarderName");
            this.Property(t => t.FreelancerId).HasColumnName("FreelancerId");
            this.Property(t => t.FreelancerName).HasColumnName("FreelancerName");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.IndustryName).HasColumnName("IndustryName");
            this.Property(t => t.InvitationDate).HasColumnName("InvitationDate");
            this.Property(t => t.InvoiceCurrencyId).HasColumnName("InvoiceCurrencyId");
            this.Property(t => t.IsCustomer).HasColumnName("IsCustomer");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.LastShipmentDate).HasColumnName("LastShipmentDate");
            this.Property(t => t.LeadDescription).HasColumnName("LeadDescription");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.MediatorId).HasColumnName("MediatorId");
            this.Property(t => t.MediatorName).HasColumnName("MediatorName");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.PaymentTermEnglishName).HasColumnName("PaymentTermEnglishName");
            this.Property(t => t.PaymentTermId).HasColumnName("PaymentTermId");
            this.Property(t => t.PrimaryContactId).HasColumnName("PrimaryContactId");
            this.Property(t => t.PrimaryContactName).HasColumnName("PrimaryContactName");
            this.Property(t => t.RankCode).HasColumnName("RankCode");
            this.Property(t => t.RankName).HasColumnName("RankName");
            this.Property(t => t.ReadyForActivationDate).HasColumnName("ReadyForActivationDate");
            this.Property(t => t.RegionId).HasColumnName("RegionId");
            this.Property(t => t.RegionName).HasColumnName("RegionName");
            this.Property(t => t.SalesmanUserEnglishName).HasColumnName("SalesmanUserEnglishName");
            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.SharedLogisticsInvitationStatusName).HasColumnName("SharedLogisticsInvitationStatusName");
            this.Property(t => t.SharedLogisticsInvitationStatusCode).HasColumnName("SharedLogisticsInvitationStatusCode");
            this.Property(t => t.StartWorkingDate).HasColumnName("StartWorkingDate");
            this.Property(t => t.StartWorkingManuallySet).HasColumnName("StartWorkingManuallySet");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.UpdatedByUserName).HasColumnName("UpdatedByUserName");
            this.Property(t => t.VatNumber).HasColumnName("VatNumber");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.Website).HasColumnName("Website");
            this.Property(t => t.AccountManagerUserId).HasColumnName("AccountManagerUserId");
            this.Property(t => t.EnableConsolidationInvoices).HasColumnName("EnableConsolidationInvoices");
            this.Property(t => t.CityName).HasColumnName("CityName");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.CountryCode).HasColumnName("CountryCode");
            this.Property(t => t.CountryName).HasColumnName("CountryName");
            this.Property(t => t.ActivityWatch).HasColumnName("ActivityWatch");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.RankId).HasColumnName("RankId");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
            this.Property(t => t.LeadSourceId).HasColumnName("LeadSourceId");
            this.Property(t => t.LastQuoteDate).HasColumnName("LastQuoteDate");
            this.Property(t => t.InvoiceCurrencyCode).HasColumnName("InvoiceCurrencyCode");
            this.Property(t => t.KnownConsignor).HasColumnName("KnownConsignor");
            this.Property(t => t.PartnerTypeId).HasColumnName("PartnerTypeId");
            this.Property(t => t.KCExpirationDate).HasColumnName("KCExpirationDate");
            this.Property(t => t.ExternalAccountingBusinessArea).HasColumnName("ExternalAccountingBusinessArea");
            this.Property(t => t.IsCreditLimitEnabled).HasColumnName("IsCreditLimitEnabled");
            this.Property(t => t.CreditLimitAmount).HasColumnName("CreditLimitAmount");
            this.Property(t => t.CreditLimitOpenBalance).HasColumnName("CreditLimitOpenBalance");
            this.Property(t => t.CreditLimitWarningPercentage).HasColumnName("CreditLimitWarningPercentage");
            this.Property(t => t.BlockNewInvoiceCreation).HasColumnName("BlockNewInvoiceCreation");
            this.Property(t => t.BlockNewShipmentCreation).HasColumnName("BlockNewShipmentCreation");
            this.Property(t => t.ExternalId2).HasColumnName("ExternalId2");
            this.Property(t => t.ActivationDate).HasColumnName("ActivationDate");
            this.Property(t => t.InactiveDate).HasColumnName("InactiveDate");
            this.Property(t => t.ActivationRequestDate).HasColumnName("ActivationRequestDate");
            this.Property(t => t.ActivatedByUserId).HasColumnName("ActivatedByUserId");
            this.Property(t => t.SetAsInactiveByUserId).HasColumnName("SetAsInactiveByUserId");
            this.Property(t => t.ActivationRequestedByUserId).HasColumnName("ActivationRequestedByUserId");
            this.Property(t => t.ActivatedByUserName).HasColumnName("ActivatedByUserName");
            this.Property(t => t.SetAsInactiveByName).HasColumnName("SetAsInactiveByName");
            this.Property(t => t.ActivationRequestedByUserName).HasColumnName("ActivationRequestedByUserName");
        }
    }
}
