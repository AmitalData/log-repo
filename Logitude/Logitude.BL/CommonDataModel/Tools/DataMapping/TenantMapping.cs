using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class TenantMapping
    {
        public static void MapEntity(TenantPM entityPM, Tenant poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Id = entityPM.Id;
            }

            poco.AddressId = entityPM.AddressId;
            poco.LocalAddressId = entityPM.LocalAddressId;
            poco.CurrencyId = entityPM.CurrencyId;
            poco.Direction = entityPM.Direction;
            poco.Email = entityPM.Email;
            poco.Format = entityPM.Format;
            poco.Company = entityPM.Company;
            poco.Language = entityPM.Language;
            poco.Website = entityPM.Website;
            poco.IATA = entityPM.IATA;
            poco.Signature = entityPM.Signature;
            poco.DimensionsUnitCode = entityPM.DimensionsUnitCode;
            poco.VolumeUnitCode = entityPM.VolumeUnitCode;
            poco.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            poco.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
            poco.FreightCurrencyId = entityPM.FreightCurrencyId;
            poco.ExportFreightPrepaidCollectId = entityPM.ExportFreightPrepaidCollectId;
            poco.ExportOtherPrepaidCollectId = entityPM.ExportOtherPrepaidCollectId;
            poco.ImportFreightPrepaidCollectId = entityPM.ImportFreightPrepaidCollectId;
            poco.ImportOtherPrepaidCollectId = entityPM.ImportOtherPrepaidCollectId;
            poco.VatNumber = entityPM.VatNumber;
            poco.OtherChargesCurrencyId = entityPM.OtherChargesCurrencyId;
            poco.TimeZoneOffset = entityPM.TimeZoneOffset;
            poco.DayLightStartDate = entityPM.DayLightStartDate;
            poco.DayLightEndDate = entityPM.DayLightEndDate;
            poco.DayLightOffset = entityPM.DayLightOffset;
            poco.QuoteSaleCurrencyId = entityPM.QuoteSaleCurrencyId;
            poco.PaymentTermId = entityPM.PaymentTermId;
            poco.ProfitCurrencyId = entityPM.ProfitCurrencyId;
            poco.AgentId = entityPM.AgentId;
            poco.PasswordPolicyCode = entityPM.PasswordPolicyCode;
            poco.MasterExpFreigPrepaidCollectId = entityPM.MasterExpFreigPrepaidCollectId;
            poco.MasterExpOtherPrepaidCollectId = entityPM.MasterExpOtherPrepaidCollectId;
            poco.MasterImpFreiPrepaidCollectId = entityPM.MasterImpFreiPrepaidCollectId;
            poco.MasterImpOtherPrepaidCollectId = entityPM.MasterImpOtherPrepaidCollectId;
            poco.IsDataBackupBuilt = entityPM.IsDataBackupBuilt;
            poco.WeightMeasurementUnitCode = entityPM.WeightMeasurementUnitCode;
            poco.DateTimeFormat = entityPM.DateTimeFormat;
            poco.InvoiceSection1 = entityPM.InvoiceSection1;
            poco.InvoiceSection2 = entityPM.InvoiceSection2;
            poco.BankDetails = entityPM.BankDetails;
            poco.IsSharedLogisticsActivated = entityPM.IsSharedLogisticsActivated;
            poco.SharedLogisticsMessageLink = entityPM.SharedLogisticsMessageLink;
            poco.CASSCode = entityPM.CASSCode;
            poco.LocalCustomsCode = entityPM.LocalCustomsCode;
            poco.IsHybrid = entityPM.IsHybrid;
            poco.VatUniqueTypeCode = entityPM.VatUniqueTypeCode;
            poco.VatMandatoryTypeCode = entityPM.VatMandatoryTypeCode;
            poco.VatUniqueCountryId = entityPM.VatUniqueCountryId;
            poco.VatMandatoryCountryId = entityPM.VatMandatoryCountryId;
            poco.VatMandatoryForPotentialCust = entityPM.VatMandatoryForPotentialCust;
            poco.IsCustomerTelRequired = entityPM.IsCustomerTelRequired;
            poco.IsCustomerFaxRequired = entityPM.IsCustomerFaxRequired;
            poco.IsPickDelAdrsRequired = entityPM.IsPickDelAdrsRequired;
            poco.IsCustomerAddress1Required = entityPM.IsCustomerAddress1Required;
            poco.HasPrimaryContact = entityPM.HasPrimaryContact;
            poco.DefaultQuestionnaireId = entityPM.DefaultQuestionnaireId;
            poco.IsQuoteSubjectEdited = entityPM.IsQuoteSubjectEdited;
            poco.AllowEAWBMoreThanTenPackages = entityPM.AllowEAWBMoreThanTenPackages;
            poco.IsMobileActivated = entityPM.IsMobileActivated;
            poco.RegulatedAgentNumber = entityPM.RegulatedAgentNumber;
            poco.RegulatedAgentRegimeActivated = entityPM.RegulatedAgentRegimeActivated;
            poco.CustomerId = entityPM.CustomerId;
            poco.CustomerTenantShareCustomsFile = entityPM.CustomerTenantShareCustomsFile;
  
            poco.CustomerTenantShareExportFile = entityPM.CustomerTenantShareExportFile;
            poco.AllowAgentInCustomersLOV = entityPM.AllowAgentInCustomersLOV;
            poco.AllowCustomersInAgentsLOV = entityPM.AllowCustomersInAgentsLOV;
            poco.IsPotentialTelRequired = entityPM.IsPotentialTelRequired;
            poco.IsPotentialFaxRequired = entityPM.IsPotentialFaxRequired;
            poco.VatFormatTypeCode = entityPM.VatFormatTypeCode;
            poco.VatFormatCountryId = entityPM.VatFormatCountryId;
            poco.IsNumeric = entityPM.IsNumeric;
            poco.VatSize = entityPM.VatSize;
      
     
            poco.IsWebAccessActivated = entityPM.IsWebAccessActivated;
            poco.IsCargoTrackWebAccessActivated = entityPM.IsCargoTrackWebAccessActivated;
            poco.IsDigitalPortalAccessActivated = entityPM.IsDigitalPortalAccessActivated;
            poco.IsCorrespondRightToLeftEnabled = entityPM.IsCorrespondRightToLeftEnabled;
            poco.IsNotesRightToLeftEnabled = entityPM.IsNotesRightToLeftEnabled;
            poco.AccountingActivationDate = entityPM.AccountingActivationDate;
            poco.AccountingActivated = entityPM.AccountingActivated;
            //poco.DropBoxAccessToken = entityPM.DropBoxAccessToken;
            poco.IsInternalTicketByDefault = entityPM.IsInternalTicketByDefault;
            poco.ProrateMasterReceivables = entityPM.ProrateMasterReceivables;
            poco.SCACCode = entityPM.SCACCode;
            poco.ExportQuotationsoIntegratedSys = entityPM.ExportQuotationsoIntegratedSys;
            poco.FMCNumber = entityPM.FMCNumber;
            poco.TenantVATManagement = entityPM.TenantVATManagement;
          
            poco.TemperatureUnitCode = entityPM.TemperatureUnitCode;
            poco.NumberFormatCode = entityPM.NumberFormatCode;
            poco.DefaultSLAId = entityPM.DefaultSLAId;
            poco.IsIncrementalBuildRunning = entityPM.IsIncrementalBuildRunning;
            poco.SharedLogisMasterMessageLink = entityPM.SharedLogisMasterMessageLink;
            poco.ShowMultiUnitsOfMeasurements = entityPM.ShowMultiUnitsOfMeasurements;
            poco.EcommerceSupportEmail = entityPM.EcommerceSupportEmail;
            poco.EcommerceTenant = entityPM.EcommerceTenant;

            poco.CBSA = entityPM.CBSA;
            poco.CAAT = entityPM.CAAT;
            poco.ApplyVATForAllPartners = entityPM.ApplyVATForAllPartners;
           
            poco.AirRatio = entityPM.AirRatio;
            poco.LCLRatio = entityPM.LCLRatio;
            poco.FCLRatio = entityPM.FCLRatio;
            poco.LTLRatio = entityPM.LTLRatio;
            poco.FTLRatio = entityPM.FTLRatio;

            if (entityPM.CheckDigitControlAlgorithmCode == null)
            {
                entityPM.CheckDigitControlAlgorithmCode = "NONE";
            }

            poco.CheckDigitControlAlgorithmCode = entityPM.CheckDigitControlAlgorithmCode;
            poco.DisplayDocumentsAndEvents = entityPM.DisplayDocumentsAndEvents;
            poco.VatUniquePartnerTypeCode = entityPM.VatUniquePartnerTypeCode;
            if (string.IsNullOrEmpty(entityPM.TransferQuotationsToUnfTrigger))// Rabaia Added this check to solve ergent signup problem
            {
                poco.TransferQuotationsToUnfTrigger = "Dont";
            }
            else
            {
                poco.TransferQuotationsToUnfTrigger = entityPM.TransferQuotationsToUnfTrigger;
            }

            poco.IsQuoteRequestActivateInShared = entityPM.IsQuoteRequestActivateInShared;
            poco.EmptyReturnClosingDays = entityPM.EmptyReturnClosingDays;
            poco.ShipmentATAClosingDays = entityPM.ShipmentATAClosingDays;
            poco.UseNewTermsOfUse = entityPM.UseNewTermsOfUse;
            poco.ShipmentATADateIndicator = entityPM.ShipmentATADateIndicator;
            poco.ApproveUploadedDocuments = entityPM.ApproveUploadedDocuments;
            // poco.StorageEncryptionKey = entityPM.StorageEncryptionKey;
            BuildSearchFields(entityPM, poco);
        }

        private static void BuildSearchFields(TenantPM entityPM, Tenant entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Company);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Direction);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Email);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Language);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.VatNumber);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
