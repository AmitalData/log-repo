using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TenantMap : EntityTypeConfiguration<Tenant>
    {
        public TenantMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Company).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.CurrencyId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Email).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.Website).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.AddressId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LocalAddressId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Format).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.Language).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.Direction).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.CASSCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.IATA).HasMaxLength(7).IsUnicode(false);
            this.Property(t => t.Signature).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.GrossWeightUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.VolumeUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.DimensionsUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.ExportFreightPrepaidCollectId).IsRequired().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.ExportOtherPrepaidCollectId).IsRequired().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.ImportFreightPrepaidCollectId).IsRequired().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.ImportOtherPrepaidCollectId).IsRequired().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.FreightCurrencyId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.OtherChargesCurrencyId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QuoteSaleCurrencyId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PaymentTermId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ProfitCurrencyId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AgentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PasswordPolicyCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.MasterExportFreightPrepaidCollectId).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.MasterExportOtherPrepaidCollectId).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.MasterImportFreightPrepaidCollectId).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.MasterImportOtherPrepaidCollectId).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.ChargeableWeightUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.WeightMeasurementUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.DateTimeFormat).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.InvoiceSection1).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.InvoiceSection2).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.BankDetails).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.LocalCustomsCode).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.DefaultQuestionnaireId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatUniqueTypeCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.VatMandatoryTypeCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.VatFormatTypeCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.VatUniqueCountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatMandatoryCountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VatFormatCountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.RegulatedAgentNumber).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.CustomerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CustomerTenantShareCustomsFile).IsRequired();
            this.Property(t => t.IsIncrementalBuildRunning).IsRequired();
            this.Property(t => t.CustomerTenantShareExportFile).IsRequired();
            this.Property(t => t.AllowAgentInCustomersLOV).IsRequired();
            this.Property(t => t.SCACCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.FMCNumber).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.StorageEncryptionKey).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.TemperatureUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.DefaultSLAId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EcommerceSupportEmail).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.CBSA).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.CAAT).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.CheckDigitControlAlgorithmCode).HasMaxLength(4).IsRequired().IsUnicode(false);
            this.Property(t => t.AllowCustomersInAgentsLOV).IsRequired();
            this.Property(t => t.VatUniquePartnerTypeCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.TransferQuotationsToUnifreightTrigger).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.ShipmentATADateIndicator).HasMaxLength(20).IsUnicode(false);

            this.ToTable("Tenants");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Company).HasColumnName("Company");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Website).HasColumnName("Website");
            this.Property(t => t.AddressId).HasColumnName("AddressId");
            this.Property(t => t.LocalAddressId).HasColumnName("LocalAddressId");
            this.Property(t => t.Format).HasColumnName("Format");
            this.Property(t => t.Language).HasColumnName("Language");
            this.Property(t => t.Direction).HasColumnName("Direction");
            this.Property(t => t.IATA).HasColumnName("IATA");
            this.Property(t => t.CASSCode).HasColumnName("CASSCode");
            this.Property(t => t.Signature).HasColumnName("Signature");
            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode");
            this.Property(t => t.VolumeUnitCode).HasColumnName("VolumeUnitCode");
            this.Property(t => t.DimensionsUnitCode).HasColumnName("DimensionsUnitCode");
            this.Property(t => t.ExportFreightPrepaidCollectId).HasColumnName("ExportFreightPrepaidCollectId");
            this.Property(t => t.ExportOtherPrepaidCollectId).HasColumnName("ExportOtherPrepaidCollectId");
            this.Property(t => t.ImportFreightPrepaidCollectId).HasColumnName("ImportFreightPrepaidCollectId");
            this.Property(t => t.ImportOtherPrepaidCollectId).HasColumnName("ImportOtherPrepaidCollectId");
            this.Property(t => t.FreightCurrencyId).HasColumnName("FreightCurrencyId");
            this.Property(t => t.VatNumber).HasColumnName("VatNumber");
            this.Property(t => t.OtherChargesCurrencyId).HasColumnName("OtherChargesCurrencyId");
            this.Property(t => t.TimeZoneOffset).HasColumnName("TimeZoneOffset");
            this.Property(t => t.DayLightOffset).HasColumnName("DayLightOffset");
            this.Property(t => t.DayLightStartDate).HasColumnName("DayLightStartDate");
            this.Property(t => t.DayLightEndDate).HasColumnName("DayLightEndDate");
            this.Property(t => t.QuoteSaleCurrencyId).HasColumnName("QuoteSaleCurrencyId");
            this.Property(t => t.PaymentTermId).HasColumnName("PaymentTermId");
            this.Property(t => t.ProfitCurrencyId).HasColumnName("ProfitCurrencyId");
            this.Property(t => t.AgentId).HasColumnName("AgentId");
            this.Property(t => t.PasswordPolicyCode).HasColumnName("PasswordPolicyCode");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomerTenantShareCustomsFile).HasColumnName("CustomerTenantShareCustomsFile");
            this.Property(t => t.IsIncrementalBuildRunning).HasColumnName("IsIncrementalBuildRunning");
            this.Property(t => t.CustomerTenantShareExportFile).HasColumnName("CustomerTenantShareExportFile");
            this.Property(t => t.IsNotesRightToLeftEnabled).HasColumnName("IsNotesRightToLeftEnabled");
            this.Property(t => t.IsInternalTicketByDefault).HasColumnName("IsInternalTicketByDefault");
            this.Property(t => t.IsFullTextSearchEnabled).HasColumnName("IsFullTextSearchEnabled");
            this.Property(t => t.ProrateMasterReceivables).HasColumnName("ProrateMasterReceivables");
            this.Property(t => t.SCACCode).HasColumnName("SCACCode");
            this.Property(t => t.FMCNumber).HasColumnName("FMCNumber");
            this.Property(t => t.TenantVATManagement).HasColumnName("TenantVATManagement");
            this.Property(t => t.StorageEncryptionKey).HasColumnName("StorageEncryptionKey");
            this.Property(t => t.TemperatureUnitCode).HasColumnName("TemperatureUnitCode");
            this.Property(t => t.DefaultSLAId).HasColumnName("DefaultSLAId");
            this.Property(t => t.NumberFormatCode).HasColumnName("NumberFormatCode");
            this.Property(t => t.EcommerceSupportEmail).HasColumnName("EcommerceSupportEmail");
            this.Property(t => t.CBSA).HasColumnName("CBSA");
            this.Property(t => t.CAAT).HasColumnName("CAAT");
            this.Property(t => t.CheckDigitControlAlgorithmCode).HasColumnName("CheckDigitControlAlgorithmCode");
            this.Property(t => t.ApplyVATForAllPartners).HasColumnName("ApplyVATForAllPartners");
            this.Property(t => t.HideFCLAllIn).HasColumnName("HideFCLAllIn");
            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate");
            this.Property(t => t.DisplayDocumentsAndEvents).HasColumnName("DisplayDocumentsAndEvents");
            this.Property(t => t.EmptyReturnClosingDays).HasColumnName("EmptyReturnClosingDays");
            this.Property(t => t.ShipmentATAClosingDays).HasColumnName("ShipmentATAClosingDays");
            this.Property(t => t.ShipmentATADateIndicator).HasColumnName("ShipmentATADateIndicator");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.MasterExportFreightPrepaidCollectId).HasColumnName("MasterExportFreightPCId");
                this.Property(t => t.MasterExportOtherPrepaidCollectId).HasColumnName("MasterExportOtherPCId");
                this.Property(t => t.MasterImportFreightPrepaidCollectId).HasColumnName("MasterImportFreightPCId");
                this.Property(t => t.MasterImportOtherPrepaidCollectId).HasColumnName("MasterImportOtherPCId");
                this.Property(t => t.VatMandatoryForPotentialCustomers).HasColumnName("VaTMandatoryPotentialCustomers");
                this.Property(t => t.AllowAgentInCustomersLOV).HasColumnName("AllowAgentInShipCustomersLOV");
                this.Property(t => t.IsCorrespondenceRightToLeftEnabled).HasColumnName("IsCorrespondenceRTLEnabled");
                this.Property(t => t.ExportQuotationsToIntegratedSystem).HasColumnName("ExportQuotationsToIntegrated");
                this.Property(t => t.TransferQuotationsToUnifreightTrigger).HasColumnName("TransferQuotationsToUNFTrigger");
                this.Property(t => t.IsQuotesRequestActivatedInShared).HasColumnName("IsQuotesRequestActivated");

            }
            //#else

            else
            {
                this.Property(t => t.MasterExportFreightPrepaidCollectId).HasColumnName("MasterExportFreightPrepaidCollectId");
                this.Property(t => t.MasterExportOtherPrepaidCollectId).HasColumnName("MasterExportOtherPrepaidCollectId");
                this.Property(t => t.MasterImportFreightPrepaidCollectId).HasColumnName("MasterImportFreightPrepaidCollectId");
                this.Property(t => t.MasterImportOtherPrepaidCollectId).HasColumnName("MasterImportOtherPrepaidCollectId");
                this.Property(t => t.VatMandatoryForPotentialCustomers).HasColumnName("VatMandatoryForPotentialCustomers");
                this.Property(t => t.AllowAgentInCustomersLOV).HasColumnName("AllowAgentInCustomersLOV");
                this.Property(t => t.IsCorrespondenceRightToLeftEnabled).HasColumnName("IsCorrespondenceRightToLeftEnabled");
                this.Property(t => t.ExportQuotationsToIntegratedSystem).HasColumnName("ExportQuotationsToIntegratedSystem");
                this.Property(t => t.TransferQuotationsToUnifreightTrigger).HasColumnName("TransferQuotationsToUnifreightTrigger");
                this.Property(t => t.IsQuotesRequestActivatedInShared).HasColumnName("IsQuotesRequestActivatedInShared");

            }

            //#endif

            this.Property(t => t.AllowCustomersInAgentsLOV).HasColumnName("AllowCustomersInAgentsLOV");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsDataBackupBuilt).HasColumnName("IsDataBackupBuilt");
            this.Property(t => t.ChargeableWeightUnitCode).HasColumnName("ChargeableWeightUnitCode");
            this.Property(t => t.WeightMeasurementUnitCode).HasColumnName("WeightMeasurementUnitCode");
            this.Property(t => t.DateTimeFormat).HasColumnName("DateTimeFormat");
            this.Property(t => t.InvoiceSection1).HasColumnName("InvoiceSection1");
            this.Property(t => t.InvoiceSection2).HasColumnName("InvoiceSection2");
            this.Property(t => t.BankDetails).HasColumnName("BankDetails");
            this.Property(t => t.IsSharedLogisticsActivated).HasColumnName("IsSharedLogisticsActivated");
            this.Property(t => t.IsMobileActivated).HasColumnName("IsMobileActivated");
            this.Property(t => t.SharedLogisticsMessageLink).HasColumnName("SharedLogisticsMessageLink");
            this.Property(t => t.IsWebAccessActivated).HasColumnName("IsWebAccessActivated");
            this.Property(t => t.IsCargoTrackWebAccessActivated).HasColumnName("IsCargoTrackWebAccessActivated");
            this.Property(t => t.IsDigitalPortalAccessActivated).HasColumnName("IsDigitalPortalAccessActivated");
            
            this.Property(t => t.LocalCustomsCode).HasColumnName("LocalCustomsCode");
            this.Property(t => t.IsHybrid).HasColumnName("IsHybrid");
            this.Property(t => t.VatUniqueTypeCode).HasColumnName("VatUniqueTypeCode");
            this.Property(t => t.VatMandatoryTypeCode).HasColumnName("VatMandatoryTypeCode");
            this.Property(t => t.VatFormatTypeCode).HasColumnName("VatFormatTypeCode");
            this.Property(t => t.VatUniqueCountryId).HasColumnName("VatUniqueCountryId");
            this.Property(t => t.VatMandatoryCountryId).HasColumnName("VatMandatoryCountryId");
            this.Property(t => t.VatFormatCountryId).HasColumnName("VatFormatCountryId");
            this.Property(t => t.IsCustomerTelRequired).HasColumnName("IsCustomerTelRequired");
            this.Property(t => t.IsCustomerFaxRequired).HasColumnName("IsCustomerFaxRequired");
            this.Property(t => t.IsPotentialTelRequired).HasColumnName("IsPotentialTelRequired");
            this.Property(t => t.IsPotentialFaxRequired).HasColumnName("IsPotentialFaxRequired");
            this.Property(t => t.IsNumeric).HasColumnName("IsNumeric");
            this.Property(t => t.VatSize).HasColumnName("VatSize");
            this.Property(t => t.IsPickDelAdrsRequired).HasColumnName("IsPickDelAdrsRequired");
            this.Property(t => t.IsCustomerAddress1Required).HasColumnName("IsCustomerAddress1Required");
            this.Property(t => t.HasPrimaryContact).HasColumnName("HasPrimaryContact");
            this.Property(t => t.DefaultQuestionnaireId).HasColumnName("DefaultQuestionnaireId");
            this.Property(t => t.IsQuoteSubjectEdited).HasColumnName("IsQuoteSubjectEdited");
            this.Property(t => t.AllowEAWBMoreThanTenPackages).HasColumnName("AllowEAWBMoreThanTenPackages");
            this.Property(t => t.RegulatedAgentNumber).HasColumnName("RegulatedAgentNumber");
            this.Property(t => t.RegulatedAgentRegimeActivated).HasColumnName("RegulatedAgentRegimeActivated");
            this.Property(t => t.TenantEmailSendingQuota).HasColumnName("TenantEmailSendingQuota");
            this.Property(t => t.VatUniquePartnerTypeCode).HasColumnName("VatUniquePartnerTypeCode");
            
            this.Property(t => t.SharedLogisMasterMessageLink).HasColumnName("SharedLogisMasterMessageLink");
            this.Property(t => t.ShowMultiUnitsOfMeasurements).HasColumnName("ShowMultiUnitsOfMeasurements");
            this.Property(t => t.UseNewTermsOfUse).HasColumnName("UseNewTermsOfUse");
            this.Property(t => t.ApproveUploadedDocuments).HasColumnName("ApproveUploadedDocuments");

            this.HasRequired(t => t.LogBoxTenantSetting).WithRequiredPrincipal(d => d.Tenant);
            this.HasOptional(t => t.Address).WithMany().HasForeignKey(d => d.AddressId);
            this.HasOptional(t => t.LocalAddress).WithMany().HasForeignKey(d => d.LocalAddressId);
            this.HasOptional(t => t.AgentCard).WithMany().HasForeignKey(d => d.AgentId);
            this.HasOptional(t => t.Currency).WithMany().HasForeignKey(d => d.CurrencyId);
            this.HasOptional(t => t.FreightCurrency).WithMany().HasForeignKey(d => d.FreightCurrencyId);
            this.HasOptional(t => t.OtherChargesCurrency).WithMany().HasForeignKey(d => d.OtherChargesCurrencyId);
            this.HasOptional(t => t.ProfitCurrency).WithMany().HasForeignKey(d => d.ProfitCurrencyId);
            this.HasOptional(t => t.QuoteSaleCurrency).WithMany().HasForeignKey(d => d.QuoteSaleCurrencyId);
            this.HasOptional(t => t.DimensionsUnit).WithMany().HasForeignKey(d => d.DimensionsUnitCode);
            this.HasOptional(t => t.PasswordPolicy).WithMany().HasForeignKey(d => d.PasswordPolicyCode);
            this.HasOptional(t => t.PaymentTerm).WithMany().HasForeignKey(d => d.PaymentTermId);
            this.HasRequired(t => t.ExportFreightPrepaidCollect).WithMany().HasForeignKey(d => d.ExportFreightPrepaidCollectId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.VolumeUnit).WithMany(t => t.Tenants).HasForeignKey(d => d.VolumeUnitCode);
            this.HasOptional(t => t.ChargeableWeightUnit).WithMany().HasForeignKey(d => d.WeightMeasurementUnitCode);
            this.HasOptional(t => t.GrossWeightUnit).WithMany().HasForeignKey(d => d.GrossWeightUnitCode);
            this.HasOptional(t => t.VatUniqueType).WithMany().HasForeignKey(d => d.VatUniqueTypeCode);
            this.HasOptional(t => t.VatMandatoryType).WithMany().HasForeignKey(d => d.VatMandatoryTypeCode);
            this.HasOptional(t => t.VatUniqueCountry).WithMany().HasForeignKey(d => d.VatUniqueCountryId);
            this.HasOptional(t => t.VatMandatoryCountry).WithMany().HasForeignKey(d => d.VatMandatoryCountryId);
            this.HasOptional(t => t.CustomerCard).WithMany().HasForeignKey(d => d.CustomerId);
            this.HasOptional(t => t.VatFormatType).WithMany().HasForeignKey(d => d.VatFormatTypeCode);
            this.HasOptional(t => t.VatFormatCountry).WithMany().HasForeignKey(d => d.VatFormatCountryId);
            this.HasOptional(t => t.TemperatureUnit).WithMany().HasForeignKey(d => d.TemperatureUnitCode);
            this.HasOptional(t => t.NumberFormat).WithMany().HasForeignKey(d => d.NumberFormatCode);
            this.HasOptional(t => t.WeightUnit).WithMany().HasForeignKey(d => d.ChargeableWeightUnitCode);
            this.HasOptional(t => t.VatUniquePartnerType).WithMany().HasForeignKey(d => d.VatUniquePartnerTypeCode);
        }
    }
}