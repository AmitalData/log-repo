using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.DataContracts;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TenantQuery
    {
        TenantRepository repository;
        public TenantQuery()
        {
            repository = new TenantRepository();
        }
        public TenantQuery(int tenant)
        {
            repository = new TenantRepository(tenant);
        }
        public TenantQuery(TenantRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TenantPM> GetTenantPMs()
        {
            IQueryable<TenantPM> tenants = (from a in repository.context.Tenants.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country").Include("LogBoxTenantSetting")
                                            select new TenantPM()
                                            {
                                                Id = a.Id,
                                                AddressId = a.AddressId,

                                                LocalAddressId = a.LocalAddressId,
                                                CompanyAddress = a.Address != null ? a.Address.Name : null,
                                                Company = a.Company,
                                                Signature = a.Signature,
                                                IATA = a.IATA,
                                                VatNumber = a.VatNumber,
                                                SearchFields = a.SearchFields,
                                                PaymentTermId = a.PaymentTermId,
                                                PaymentTermName = a.PaymentTerm != null ? a.PaymentTerm.EnglishName : null,
                                                AgentId = a.AgentId,
                                                AgentName = a.AgentCard != null ? a.AgentCard.EnglishName : null,
                                                CurrencyId = a.CurrencyId,
                                                AccountingCurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                AccountingCurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                ProfitCurrencyId = a.ProfitCurrencyId,
                                                ProfitCurrencyCode = a.ProfitCurrency != null ? a.ProfitCurrency.Code : null,
                                                FreightCurrencyId = a.FreightCurrencyId,
                                                FreightCurrencyCode = a.FreightCurrency != null ? a.FreightCurrency.Code : null,
                                                OtherChargesCurrencyId = a.OtherChargesCurrencyId,
                                                OtherChargesCurrencyCode = a.OtherChargesCurrency != null ? a.OtherChargesCurrency.Code : null,
                                                QuoteSaleCurrencyId = a.QuoteSaleCurrencyId,
                                                QuoteSaleCurrencyCode = a.QuoteSaleCurrency != null ? a.QuoteSaleCurrency.Code : null,
                                                VolumeUnitCode = a.VolumeUnitCode,
                                                DimensionsUnitCode = a.DimensionsUnitCode,
                                                GrossWeightUnitCode = a.GrossWeightUnitCode,
                                                ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                                WeightMeasurementUnitCode = a.WeightMeasurementUnitCode,
                                                ExportFreightPrepaidCollectId = a.ExportFreightPrepaidCollectId,
                                                ImportFreightPrepaidCollectId = a.ImportFreightPrepaidCollectId,
                                                ExportOtherPrepaidCollectId = a.ExportOtherPrepaidCollectId,
                                                ImportOtherPrepaidCollectId = a.ImportOtherPrepaidCollectId,
                                                MasterExportFreightPrepaidCollectId = a.MasterExportFreightPrepaidCollectId,
                                                MasterImportFreightPrepaidCollectId = a.MasterImportFreightPrepaidCollectId,
                                                MasterExportOtherPrepaidCollectId = a.MasterExportOtherPrepaidCollectId,
                                                MasterImportOtherPrepaidCollectId = a.MasterImportOtherPrepaidCollectId,
                                                TimeZoneOffset = a.TimeZoneOffset,
                                                Language = a.Language,
                                                Email = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.Email : "",
                                                Website = a.Website,
                                                Format = a.Format,
                                                Direction = a.Direction,
                                                DayLightOffset = a.DayLightOffset,
                                                DayLightStartDate = a.DayLightStartDate,
                                                DayLightEndDate = a.DayLightEndDate,
                                                PasswordPolicyCode = a.PasswordPolicyCode,
                                                PasswordStrength = a.PasswordPolicy != null ? a.PasswordPolicy.PasswordStrength : null,
                                                IsDataBackupBuilt = a.IsDataBackupBuilt,
                                                CountryCode = a.Address != null ? (a.Address.Country != null ? a.Address.Country.Code : null) : null,
                                                CountryName = a.Address != null ? (a.Address.Country != null ? a.Address.Country.EnglishName : null) : null,
                                                DateTimeFormat = a.DateTimeFormat,
                                                InvoiceSection1 = a.InvoiceSection1,
                                                InvoiceSection2 = a.InvoiceSection2,
                                                BankDetails = a.BankDetails,
                                                IsSharedLogisticsActivated = a.IsSharedLogisticsActivated,
                                                SharedLogisticsMessageLink = a.SharedLogisticsMessageLink,
                                                ProfitCurrencyRate = 1,
                                                CASSCode = a.CASSCode,
                                                IsHybrid = a.IsHybrid,
                                                LocalCustomsCode = a.LocalCustomsCode,
                                                VatUniqueTypeCode = a.VatUniqueTypeCode,
                                                VatMandatoryTypeCode = a.VatMandatoryTypeCode,
                                                VatUniqueCountryId = a.VatUniqueCountryId,
                                                VatMandatoryCountryId = a.VatMandatoryCountryId,
                                                IsCustomerTelRequired = a.IsCustomerTelRequired,
                                                IsCustomerFaxRequired = a.IsCustomerFaxRequired,
                                                IsPickDelAdrsRequired = a.IsPickDelAdrsRequired,
                                                VatMandatoryForPotentialCustomers = a.VatMandatoryForPotentialCustomers,
                                                IsCustomerAddress1Required = a.IsCustomerAddress1Required,
                                                HasPrimaryContact = a.HasPrimaryContact,
                                                DefaultQuestionnaireId = a.DefaultQuestionnaireId,
                                                IsQuoteSubjectEdited = a.IsQuoteSubjectEdited,
                                                AllowEAWBMoreThanTenPackages = a.AllowEAWBMoreThanTenPackages,
                                                IsMobileActivated = a.IsMobileActivated,
                                                RegulatedAgentNumber = a.RegulatedAgentNumber,
                                                RegulatedAgentRegimeActivated = a.RegulatedAgentRegimeActivated,

                                                CustomerId = a.CustomerId,
                                                CustomerName = a.CustomerCard != null ? a.CustomerCard.EnglishName : null,
                                                IsCustomerTenantShare = a.IsCustomerTenantShare,

                                                CustomerTenantShareExportFile = a.CustomerTenantShareExportFile,
                                                AllowAgentInCustomersLOV = a.AllowAgentInCustomersLOV,
                                                IsPotentialTelRequired = a.IsPotentialTelRequired,
                                                IsPotentialFaxRequired = a.IsPotentialFaxRequired,
                                                VatFormatTypeCode = a.VatFormatTypeCode,
                                                VatFormatCountryId = a.VatFormatCountryId,
                                                IsNumeric = a.IsNumeric,
                                                VatSize = a.VatSize,
                                                IsWebAccessActivated = a.IsWebAccessActivated,

                                                IsCorrespondenceRightToLeftEnabled = a.IsCorrespondenceRightToLeftEnabled,
                                                IsNotesRightToLeftEnabled = a.IsNotesRightToLeftEnabled,
                                                IsInternalTicketByDefault = a.IsInternalTicketByDefault,
                                                ProrateMasterReceivables = a.ProrateMasterReceivables,
                                                SCACCode = a.SCACCode,
                                                ExportQuotationsToIntegratedSystem = a.ExportQuotationsToIntegratedSystem,
                                                FMCNumber = a.FMCNumber,

                                                //DropBoxAccessToken = a.DropBoxAccessToken
                                                TenantVATManagement = a.TenantVATManagement,
                                                TemperatureUnitCode = a.TemperatureUnitCode,
                                                DefaultSLAId = a.DefaultSLAId,

                                                NumberFormatCode = a.NumberFormatCode,
                                                CAAT = a.CAAT,
                                                CBSA = a.CBSA,
                                                IsTestTenant = a.IsTestTenant,
                                                CheckDigitControlAlgorithmCode = a.CheckDigitControlAlgorithmCode,
                                                ApplyVATForAllPartners = a.ApplyVATForAllPartners,
                                                IsDocumentsArchive = a.LogBoxTenantSetting.IsDocumentsArchive,
                                                CustomerTenantShareImportFile = a.LogBoxTenantSetting.CustomerTenantShareImportFile,
                                                AutoArchiveOnInvoice = a.LogBoxTenantSetting.AutoArchiveOnInvoice,
                                                StockTypeCode = a.LogBoxTenantSetting.StockTypeCode,
                                                DocumentShareAsDefault = a.LogBoxTenantSetting.DocumentShareAsDefault,
                                                LogBoxAdminUserId = a.LogBoxTenantSetting.LogBoxAdminUserId,
                                                HideFCLAllIn = a.HideFCLAllIn,
                                                AllowCustomersInAgentsLOV = a.AllowCustomersInAgentsLOV,
                                            });

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalObjectContext = GlobalContext.GetContext();
                IQueryable<TenantManagement> tenantmanagements = globalObjectContext.TenantManagements;
                foreach (TenantPM t in tenants)
                {
                    t.PackageCode = tenantmanagements.Where(d => d.Id == t.Id).FirstOrDefault().PackageCode;
                    t.TemporalPackageCode = tenantmanagements.Where(d => d.Id == t.Id).FirstOrDefault().TemporalPackageCode;
                    t.TemporalStartDate = tenantmanagements.Where(d => d.Id == t.Id).FirstOrDefault().TemporalStartDate;
                    t.TemporalEndDate = tenantmanagements.Where(d => d.Id == t.Id).FirstOrDefault().TemporalEndDate;
                }
            }
            return tenants;
        }

        public List<TenantList> GetSomeTenant()
        {
            List<TenantList> tenantLists = (from a in repository.context.Tenants
                                            where a.Id > 1269
                                            select new TenantList()
                                            {
                                                Id = a.Id,
                                            }).ToList();


            return tenantLists;
        }

        public TenantPM GetSinglePM(int id)
        {
            string entityName = "TenantPM" + id;

            TenantPM entity;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    Tenant tt = (from a in repository.context.Tenants.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country").Include("LogBoxTenantSetting")
                                 where a.Id == id
                                 select a).FirstOrDefault();

                    TenantPM tenant = new TenantPM()
                    {
                        Id = tt.Id,
                        AddressId = tt.AddressId,
                        LocalAddressId = tt.LocalAddressId,
                        CompanyAddress = tt.Address != null ? tt.Address.Name : null,
                        Company = tt.Company,
                       
                        Signature = tt.Signature,
                        IATA = tt.IATA,
                        VatNumber = tt.VatNumber,
                        SearchFields = tt.SearchFields,
                        PaymentTermId = tt.PaymentTermId,
                        PaymentTermName = tt.PaymentTerm != null ? tt.PaymentTerm.EnglishName : null,
                        AgentId = tt.AgentId,
                        AgentName = tt.AgentCard != null ? tt.AgentCard.EnglishName : null,
                        CurrencyId = tt.CurrencyId,
                        AccountingCurrencyCode = tt.Currency != null ? tt.Currency.Code : null,
                        AccountingCurrencyName = tt.Currency != null ? tt.Currency.EnglishName : null,
                        ProfitCurrencyId = tt.ProfitCurrencyId,
                        ProfitCurrencyCode = tt.ProfitCurrency != null ? tt.ProfitCurrency.Code : null,
                        FreightCurrencyId = tt.FreightCurrencyId,
                        FreightCurrencyCode = tt.FreightCurrency != null ? tt.FreightCurrency.Code : null,
                        OtherChargesCurrencyId = tt.OtherChargesCurrencyId,
                        OtherChargesCurrencyCode = tt.OtherChargesCurrency != null ? tt.OtherChargesCurrency.Code : null,
                        QuoteSaleCurrencyId = tt.QuoteSaleCurrencyId,
                        QuoteSaleCurrencyCode = tt.QuoteSaleCurrency != null ? tt.QuoteSaleCurrency.Code : null,
                        VolumeUnitCode = tt.VolumeUnitCode,
                        DimensionsUnitCode = tt.DimensionsUnitCode,
                        GrossWeightUnitCode = tt.GrossWeightUnitCode,
                        ChargeableWeightUnitCode = tt.ChargeableWeightUnitCode,
                        WeightMeasurementUnitCode = tt.WeightMeasurementUnitCode,
                        ExportFreightPrepaidCollectId = tt.ExportFreightPrepaidCollectId,
                        ImportFreightPrepaidCollectId = tt.ImportFreightPrepaidCollectId,
                        ExportOtherPrepaidCollectId = tt.ExportOtherPrepaidCollectId,
                        ImportOtherPrepaidCollectId = tt.ImportOtherPrepaidCollectId,
                        MasterExportFreightPrepaidCollectId = tt.MasterExportFreightPrepaidCollectId,
                        MasterImportFreightPrepaidCollectId = tt.MasterImportFreightPrepaidCollectId,
                        MasterExportOtherPrepaidCollectId = tt.MasterExportOtherPrepaidCollectId,
                        MasterImportOtherPrepaidCollectId = tt.MasterImportOtherPrepaidCollectId,
                        TimeZoneOffset = tt.TimeZoneOffset,
                        Language = tt.Language,
                        Email = tt.CustomerCard != null && tt.CustomerCard.PrimaryContact != null ? tt.CustomerCard.PrimaryContact.Email : "",
                        Website = tt.Website,
                        Format = tt.Format,
                        Direction = tt.Direction,
                        DayLightOffset = tt.DayLightOffset,
                        DayLightStartDate = tt.DayLightStartDate,
                        DayLightEndDate = tt.DayLightEndDate,
                        PasswordPolicyCode = tt.PasswordPolicyCode,
                        PasswordStrength = tt.PasswordPolicy != null ? tt.PasswordPolicy.PasswordStrength : null,
                        IsDataBackupBuilt = tt.IsDataBackupBuilt,
                        CountryCode = tt.Address != null ? (tt.Address.Country != null ? tt.Address.Country.Code : null) : null,
                        CountryName = tt.Address != null ? (tt.Address.Country != null ? tt.Address.Country.EnglishName : null) : null,
                        DateTimeFormat = tt.DateTimeFormat,
                        InvoiceSection1 = tt.InvoiceSection1,
                        InvoiceSection2 = tt.InvoiceSection2,
                        BankDetails = tt.BankDetails,
                        IsSharedLogisticsActivated = tt.IsSharedLogisticsActivated,
                        SharedLogisticsMessageLink = tt.SharedLogisticsMessageLink,
                        ProfitCurrencyRate = 1,
                        CASSCode = tt.CASSCode,
                        IsHybrid = tt.IsHybrid,
                        LocalCustomsCode = tt.LocalCustomsCode,
                        VatUniqueTypeCode = tt.VatUniqueTypeCode,
                        VatMandatoryTypeCode = tt.VatMandatoryTypeCode,
                        VatUniqueCountryId = tt.VatUniqueCountryId,
                        VatMandatoryCountryId = tt.VatMandatoryCountryId,
                        IsCustomerTelRequired = tt.IsCustomerTelRequired,
                        IsCustomerFaxRequired = tt.IsCustomerFaxRequired,
                        IsPickDelAdrsRequired = tt.IsPickDelAdrsRequired,
                        VatMandatoryForPotentialCustomers = tt.VatMandatoryForPotentialCustomers,
                        IsCustomerAddress1Required = tt.IsCustomerAddress1Required,
                        HasPrimaryContact = tt.HasPrimaryContact,
                        DefaultQuestionnaireId = tt.DefaultQuestionnaireId,
                        IsQuoteSubjectEdited = tt.IsQuoteSubjectEdited,
                        AllowEAWBMoreThanTenPackages = tt.AllowEAWBMoreThanTenPackages,
                        IsMobileActivated = tt.IsMobileActivated,
                        RegulatedAgentNumber = tt.RegulatedAgentNumber,
                        RegulatedAgentRegimeActivated = tt.RegulatedAgentRegimeActivated,
                        
                        CustomerId = tt.CustomerId,
                        CustomerName = tt.CustomerCard != null ? tt.CustomerCard.EnglishName : null,
                        IsCustomerTenantShare = tt.IsCustomerTenantShare,
                      
                        CustomerTenantShareExportFile = tt.CustomerTenantShareExportFile,
                        AllowAgentInCustomersLOV = tt.AllowAgentInCustomersLOV,
                        AllowCustomersInAgentsLOV = tt.AllowCustomersInAgentsLOV,
                        IsPotentialTelRequired = tt.IsPotentialTelRequired,
                        IsPotentialFaxRequired = tt.IsPotentialFaxRequired,
                        VatFormatTypeCode = tt.VatFormatTypeCode,
                        VatFormatCountryId = tt.VatFormatCountryId,
                        IsNumeric = tt.IsNumeric,
                        VatSize = tt.VatSize,
                       
                        IsWebAccessActivated = tt.IsWebAccessActivated,
                        IsCorrespondenceRightToLeftEnabled = tt.IsCorrespondenceRightToLeftEnabled,
                        IsNotesRightToLeftEnabled = tt.IsNotesRightToLeftEnabled,
                        AccountingActivationDate = tt.AccountingActivationDate,
                        AccountingActivated = tt.AccountingActivated,
                        IsInternalTicketByDefault = tt.IsInternalTicketByDefault,
                        ProrateMasterReceivables = tt.ProrateMasterReceivables,
                        SCACCode = tt.SCACCode,
                        ExportQuotationsToIntegratedSystem = tt.ExportQuotationsToIntegratedSystem,
                        FMCNumber = tt.FMCNumber,
                      
                        //DropBoxAccessToken = tt.DropBoxAccessToken
                        TenantVATManagement = tt.TenantVATManagement,
                        TemperatureUnitCode = tt.TemperatureUnitCode,
                        DefaultSLAId = tt.DefaultSLAId,
                 
                        NumberFormatCode = tt.NumberFormatCode,
                        EcommerceSupportEmail = tt.EcommerceSupportEmail,
                        CAAT = tt.CAAT,
                        CBSA = tt.CBSA,
                        IsTestTenant = tt.IsTestTenant,
                        CheckDigitControlAlgorithmCode = tt.CheckDigitControlAlgorithmCode,
                        ApplyVATForAllPartners = tt.ApplyVATForAllPartners,
                        IsDocumentsArchive = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.IsDocumentsArchive : false,
                        CustomerTenantShareImportFile = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.CustomerTenantShareImportFile : false,
                        AutoArchiveOnInvoice = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.AutoArchiveOnInvoice : false,
                        StockTypeCode = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.StockTypeCode : null,
                        DocumentShareAsDefault = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.DocumentShareAsDefault : false,
                        LogBoxAdminUserId = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.LogBoxAdminUserId : null,
                        HideFCLAllIn = tt.HideFCLAllIn,
                    };

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalObjectContext = GlobalContext.GetContext();
                        tenant.PackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().PackageCode;
                        tenant.TemporalPackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalPackageCode;
                        tenant.TemporalStartDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalStartDate;
                        tenant.TemporalEndDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalEndDate;
                        //tenant.StockTypeCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().StockTypeCode;
                    }

                    if (tenant.CurrencyId != null)
                    {
                        CurrencyRepository repository = new CurrencyRepository(tenant.Id);
                        Currency cur = repository.GetSingleCurrencyById(tenant.CurrencyId, tenant.Id, true);
                        if (cur != null)
                        {
                            tenant.CurrencyCode = cur.Code;
                            tenant.CurrencySign = cur.Sign;
                        }
                    }

                    if (tenant.AddressId != null)
                    {
                        AddressQuery addressQuery = new AddressQuery(id);
                        AddressPM add = addressQuery.GetSingleAddressPM(tenant.AddressId, tenant.Id);
                        tenant.CountryCode = add.CountryCode;
                        tenant.CountryName = add.CountryEnglishName;
                    }

                    entity = tenant;
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                }

                else
                {
                    entity = (TenantPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                Tenant tt = (from a in repository.context.Tenants.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country").Include("LogBoxTenantSetting")
                             where a.Id == id
                             select a).FirstOrDefault();


                TenantPM tenant = new TenantPM()
                {
                    Id = tt.Id,
                    AddressId = tt.AddressId,
                    LocalAddressId = tt.LocalAddressId,
                    CompanyAddress = tt.Address != null ? tt.Address.Name : null,
                    Company = tt.Company,
                    Signature = tt.Signature,
                    IATA = tt.IATA,
                    VatNumber = tt.VatNumber,
                    SearchFields = tt.SearchFields,
                    PaymentTermId = tt.PaymentTermId,
                    PaymentTermName = tt.PaymentTerm != null ? tt.PaymentTerm.EnglishName : null,
                    AgentId = tt.AgentId,
                    AgentName = tt.AgentCard != null ? tt.AgentCard.EnglishName : null,
                    CurrencyId = tt.CurrencyId,
                    AccountingCurrencyCode = tt.Currency != null ? tt.Currency.Code : null,
                    AccountingCurrencyName = tt.Currency != null ? tt.Currency.EnglishName : null,
                    ProfitCurrencyId = tt.ProfitCurrencyId,
                    ProfitCurrencyCode = tt.ProfitCurrency != null ? tt.ProfitCurrency.Code : null,
                    FreightCurrencyId = tt.FreightCurrencyId,
                    FreightCurrencyCode = tt.FreightCurrency != null ? tt.FreightCurrency.Code : null,
                    OtherChargesCurrencyId = tt.OtherChargesCurrencyId,
                    OtherChargesCurrencyCode = tt.OtherChargesCurrency != null ? tt.OtherChargesCurrency.Code : null,
                    QuoteSaleCurrencyId = tt.QuoteSaleCurrencyId,
                    QuoteSaleCurrencyCode = tt.QuoteSaleCurrency != null ? tt.QuoteSaleCurrency.Code : null,
                    VolumeUnitCode = tt.VolumeUnitCode,
                    DimensionsUnitCode = tt.DimensionsUnitCode,
                    GrossWeightUnitCode = tt.GrossWeightUnitCode,
                    ChargeableWeightUnitCode = tt.ChargeableWeightUnitCode,
                    WeightMeasurementUnitCode = tt.WeightMeasurementUnitCode,
                    ExportFreightPrepaidCollectId = tt.ExportFreightPrepaidCollectId,
                    ImportFreightPrepaidCollectId = tt.ImportFreightPrepaidCollectId,
                    ExportOtherPrepaidCollectId = tt.ExportOtherPrepaidCollectId,
                    ImportOtherPrepaidCollectId = tt.ImportOtherPrepaidCollectId,
                    MasterExportFreightPrepaidCollectId = tt.MasterExportFreightPrepaidCollectId,
                    MasterImportFreightPrepaidCollectId = tt.MasterImportFreightPrepaidCollectId,
                    MasterExportOtherPrepaidCollectId = tt.MasterExportOtherPrepaidCollectId,
                    MasterImportOtherPrepaidCollectId = tt.MasterImportOtherPrepaidCollectId,
                    TimeZoneOffset = tt.TimeZoneOffset,
                    Language = tt.Language,
                    Email = tt.CustomerCard != null && tt.CustomerCard.PrimaryContact != null ? tt.CustomerCard.PrimaryContact.Email : "",
                    Website = tt.Website,
                    Format = tt.Format,
                    Direction = tt.Direction,
                    DayLightOffset = tt.DayLightOffset,
                    DayLightStartDate = tt.DayLightStartDate,
                    DayLightEndDate = tt.DayLightEndDate,
                    PasswordPolicyCode = tt.PasswordPolicyCode,
                    PasswordStrength = tt.PasswordPolicy != null ? tt.PasswordPolicy.PasswordStrength : null,
                    IsDataBackupBuilt = tt.IsDataBackupBuilt,
                    CountryCode = tt.Address != null ? (tt.Address.Country != null ? tt.Address.Country.Code : null) : null,
                    CountryName = tt.Address != null ? (tt.Address.Country != null ? tt.Address.Country.EnglishName : null) : null,
                    DateTimeFormat = tt.DateTimeFormat,
                    InvoiceSection1 = tt.InvoiceSection1,
                    InvoiceSection2 = tt.InvoiceSection2,
                    BankDetails = tt.BankDetails,
                    IsSharedLogisticsActivated = tt.IsSharedLogisticsActivated,
                    SharedLogisticsMessageLink = tt.SharedLogisticsMessageLink,
                    ProfitCurrencyRate = 1,
                    CASSCode = tt.CASSCode,
                    IsHybrid = tt.IsHybrid,
                    LocalCustomsCode = tt.LocalCustomsCode,
                    VatUniqueTypeCode = tt.VatUniqueTypeCode,
                    VatMandatoryTypeCode = tt.VatMandatoryTypeCode,
                    VatUniqueCountryId = tt.VatUniqueCountryId,
                    VatMandatoryCountryId = tt.VatMandatoryCountryId,
                    IsCustomerTelRequired = tt.IsCustomerTelRequired,
                    IsCustomerFaxRequired = tt.IsCustomerFaxRequired,
                    IsPickDelAdrsRequired = tt.IsPickDelAdrsRequired,
                    VatMandatoryForPotentialCustomers = tt.VatMandatoryForPotentialCustomers,
                    IsCustomerAddress1Required = tt.IsCustomerAddress1Required,
                    HasPrimaryContact = tt.HasPrimaryContact,
                    DefaultQuestionnaireId = tt.DefaultQuestionnaireId,
                    IsQuoteSubjectEdited = tt.IsQuoteSubjectEdited,
                    AllowEAWBMoreThanTenPackages = tt.AllowEAWBMoreThanTenPackages,
                    IsMobileActivated = tt.IsMobileActivated,
                    RegulatedAgentNumber = tt.RegulatedAgentNumber,
                    RegulatedAgentRegimeActivated = tt.RegulatedAgentRegimeActivated,
                   
                    CustomerId = tt.CustomerId,
                    CustomerName = tt.CustomerCard != null ? tt.CustomerCard.EnglishName : null,
                    IsCustomerTenantShare = tt.IsCustomerTenantShare,
                   
                    CustomerTenantShareExportFile = tt.CustomerTenantShareExportFile,
                    AllowAgentInCustomersLOV = tt.AllowAgentInCustomersLOV,
                    AllowCustomersInAgentsLOV = tt.AllowCustomersInAgentsLOV,
                    IsPotentialTelRequired = tt.IsPotentialTelRequired,
                    IsPotentialFaxRequired = tt.IsPotentialFaxRequired,
                    VatFormatTypeCode = tt.VatFormatTypeCode,
                    VatFormatCountryId = tt.VatFormatCountryId,
                    IsNumeric = tt.IsNumeric,
                    VatSize = tt.VatSize,
                    
                    IsWebAccessActivated = tt.IsWebAccessActivated,
                    IsCorrespondenceRightToLeftEnabled = tt.IsCorrespondenceRightToLeftEnabled,
                    IsNotesRightToLeftEnabled = tt.IsNotesRightToLeftEnabled,
                    AccountingActivationDate = tt.AccountingActivationDate,
                    AccountingActivated = tt.AccountingActivated,
                    IsInternalTicketByDefault = tt.IsInternalTicketByDefault,
                    ProrateMasterReceivables = tt.ProrateMasterReceivables,
                    SCACCode = tt.SCACCode,
                    ExportQuotationsToIntegratedSystem = tt.ExportQuotationsToIntegratedSystem,
                    //DropBoxAccessToken = tt.DropBoxAccessToken
                    FMCNumber = tt.FMCNumber,
                    TenantVATManagement = tt.TenantVATManagement,
               
                    TemperatureUnitCode = tt.TemperatureUnitCode,
                    DefaultSLAId = tt.DefaultSLAId,
                  
                    NumberFormatCode = tt.NumberFormatCode,
                    EcommerceSupportEmail = tt.EcommerceSupportEmail,
                    CAAT = tt.CAAT,
                    CBSA = tt.CBSA,
                    IsTestTenant = tt.IsTestTenant,
                    CheckDigitControlAlgorithmCode = tt.CheckDigitControlAlgorithmCode,
                    ApplyVATForAllPartners = tt.ApplyVATForAllPartners,
                    IsDocumentsArchive = tt.LogBoxTenantSetting!= null ? tt.LogBoxTenantSetting.IsDocumentsArchive : false,
                    CustomerTenantShareImportFile = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.CustomerTenantShareImportFile:false,
                    AutoArchiveOnInvoice = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.AutoArchiveOnInvoice : false,
                    StockTypeCode = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.StockTypeCode : null,
                    DocumentShareAsDefault = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.DocumentShareAsDefault : false,
                    LogBoxAdminUserId = tt.LogBoxTenantSetting != null ? tt.LogBoxTenantSetting.LogBoxAdminUserId : null,
                    HideFCLAllIn = tt.HideFCLAllIn,
                };

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalObjectContext = GlobalContext.GetContext();
                    tenant.PackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().PackageCode;
                    tenant.TemporalPackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalPackageCode;
                    tenant.TemporalStartDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalStartDate;
                    tenant.TemporalEndDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalEndDate;
                    //tenant.StockTypeCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().StockTypeCode;
                    scope.Complete();
                }

                if (tenant.CurrencyId != null)
                {
                    CurrencyRepository repository = new CurrencyRepository(tenant.Id);
                    Currency cur = repository.GetSingleCurrencyById(tenant.CurrencyId, tenant.Id, true);
                    tenant.CurrencyCode = cur.Code;
                }

                if (tenant.AddressId != null)
                {
                    AddressQuery addressQuery = new AddressQuery(id);
                    AddressPM add = addressQuery.GetSingleAddressPM(tenant.AddressId, tenant.Id);
                    tenant.CountryCode = add.CountryCode;
                    tenant.CountryName = add.CountryEnglishName;
                }

                entity = tenant;
            }
            return entity;
        }

        public static TenantPM GetSingleTenantPM(int id)
        {
            string entityName = "TenantPM" + id;

            TenantPM entity;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    ICommonDataContext context = CommonDataContext.GetContext(id);
                    TenantPM tenant = (from a in context.Tenants.Include("Address")
                                       where a.Id == id
                                       select new TenantPM()
                                       {
                                           AddressId = a.AddressId,
                                           LocalAddressId = a.LocalAddressId,
                                           Company = a.Company,
                                           CurrencyId = a.CurrencyId,
                                           Direction = a.Direction,
                                           Email = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.Email : "",
                                           Format = a.Format,
                                           Id = a.Id,
                                           Language = a.Language,
                                           Website = a.Website,
                                           IATA = a.IATA,
                                           Signature = a.Signature,
                                           DimensionsUnitCode = a.DimensionsUnitCode,
                                           VolumeUnitCode = a.VolumeUnitCode,
                                           GrossWeightUnitCode = a.GrossWeightUnitCode,
                                           ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                           ExportFreightPrepaidCollectId = a.ExportFreightPrepaidCollectId,
                                           ExportOtherPrepaidCollectId = a.ExportOtherPrepaidCollectId,
                                           ImportFreightPrepaidCollectId = a.ImportFreightPrepaidCollectId,
                                           ImportOtherPrepaidCollectId = a.ImportOtherPrepaidCollectId,
                                           FreightCurrencyId = a.FreightCurrencyId,
                                           VatNumber = a.VatNumber,
                                           OtherChargesCurrencyId = a.OtherChargesCurrencyId,
                                           TimeZoneOffset = a.TimeZoneOffset,
                                           DayLightStartDate = a.DayLightStartDate,
                                           DayLightEndDate = a.DayLightEndDate,
                                           DayLightOffset = a.DayLightOffset,
                                           QuoteSaleCurrencyId = a.QuoteSaleCurrencyId,
                                           PaymentTermId = a.PaymentTermId,
                                           ProfitCurrencyId = a.ProfitCurrencyId,
                                           AgentId = a.AgentId,
                                           PasswordPolicyCode = a.PasswordPolicyCode,
                                           MasterExportFreightPrepaidCollectId = a.MasterExportFreightPrepaidCollectId,
                                           MasterExportOtherPrepaidCollectId = a.MasterExportOtherPrepaidCollectId,
                                           MasterImportFreightPrepaidCollectId = a.MasterImportFreightPrepaidCollectId,
                                           MasterImportOtherPrepaidCollectId = a.MasterImportOtherPrepaidCollectId,
                                           SearchFields = a.SearchFields,
                                           IsDataBackupBuilt = a.IsDataBackupBuilt,
                                           WeightMeasurementUnitCode = a.WeightMeasurementUnitCode,
                                           DateTimeFormat = a.DateTimeFormat,
                                           InvoiceSection1 = a.InvoiceSection1,
                                           InvoiceSection2 = a.InvoiceSection2,
                                           BankDetails = a.BankDetails,
                                           IsSharedLogisticsActivated = a.IsSharedLogisticsActivated,
                                           SharedLogisticsMessageLink = a.SharedLogisticsMessageLink,
                                           ProfitCurrencyRate = 1,
                                           CASSCode = a.CASSCode,
                                           LocalCustomsCode = a.LocalCustomsCode,
                                           CountryCode = a.Address != null ? (a.Address.Country != null ? a.Address.Country.Code : null) : null,
                                           CountryName = a.Address != null ? (a.Address.Country != null ? a.Address.Country.EnglishName : null) : null,
                                           IsHybrid = a.IsHybrid,
                                           VatUniqueTypeCode = a.VatUniqueTypeCode,
                                           VatMandatoryTypeCode = a.VatMandatoryTypeCode,
                                           VatUniqueCountryId = a.VatUniqueCountryId,
                                           VatMandatoryCountryId = a.VatMandatoryCountryId,
                                           IsCustomerTelRequired = a.IsCustomerTelRequired,
                                           IsCustomerFaxRequired = a.IsCustomerFaxRequired,
                                           IsPickDelAdrsRequired = a.IsPickDelAdrsRequired,
                                           VatMandatoryForPotentialCustomers = a.VatMandatoryForPotentialCustomers,
                                           IsCustomerAddress1Required = a.IsCustomerAddress1Required,
                                           HasPrimaryContact = a.HasPrimaryContact,
                                           DefaultQuestionnaireId = a.DefaultQuestionnaireId,
                                           IsQuoteSubjectEdited = a.IsQuoteSubjectEdited,
                                           AllowEAWBMoreThanTenPackages = a.AllowEAWBMoreThanTenPackages,
                                           IsMobileActivated = a.IsMobileActivated,
                                           RegulatedAgentNumber = a.RegulatedAgentNumber,
                                           RegulatedAgentRegimeActivated = a.RegulatedAgentRegimeActivated,
                                         
                                           CustomerId = a.CustomerId,
                                           CustomerName = a.CustomerCard != null ? a.CustomerCard.EnglishName : null,
                                           IsCustomerTenantShare = a.IsCustomerTenantShare,
                                         
                                           CustomerTenantShareExportFile = a.CustomerTenantShareExportFile,
                                           AllowAgentInCustomersLOV = a.AllowAgentInCustomersLOV,
                                           AllowCustomersInAgentsLOV = a.AllowCustomersInAgentsLOV,
                                           IsPotentialTelRequired = a.IsPotentialTelRequired,
                                           IsPotentialFaxRequired = a.IsPotentialFaxRequired,
                                           VatFormatTypeCode = a.VatFormatTypeCode,
                                           VatFormatCountryId = a.VatFormatCountryId,
                                           IsNumeric = a.IsNumeric,
                                           VatSize = a.VatSize,
                                         
                                           IsWebAccessActivated = a.IsWebAccessActivated,
                                           IsCorrespondenceRightToLeftEnabled = a.IsCorrespondenceRightToLeftEnabled,
                                           IsNotesRightToLeftEnabled = a.IsNotesRightToLeftEnabled,
                                           IsInternalTicketByDefault = a.IsInternalTicketByDefault,
                                           ProrateMasterReceivables = a.ProrateMasterReceivables,
                                           SCACCode = a.SCACCode,
                                           ExportQuotationsToIntegratedSystem = a.ExportQuotationsToIntegratedSystem,
                                           FMCNumber = a.FMCNumber,
                                      
                                           //DropBoxAccessToken = a.DropBoxAccessToken
                                           TenantVATManagement = a.TenantVATManagement,
                                           TemperatureUnitCode = a.TemperatureUnitCode,
                                           DefaultSLAId = a.DefaultSLAId,
                                         
                                           NumberFormatCode = a.NumberFormatCode,
                                           EcommerceSupportEmail = a.EcommerceSupportEmail,
                                           CAAT = a.CAAT,
                                           CBSA = a.CBSA,
                                           IsTestTenant = a.IsTestTenant,
                                           CheckDigitControlAlgorithmCode = a.CheckDigitControlAlgorithmCode,
                                           ApplyVATForAllPartners = a.ApplyVATForAllPartners,
                                           HideFCLAllIn = a.HideFCLAllIn,
                                       }).FirstOrDefault();

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalObjectContext = GlobalContext.GetContext();
                        tenant.PackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().PackageCode;
                        tenant.TemporalPackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalPackageCode;
                        tenant.TemporalStartDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalStartDate;
                        tenant.TemporalEndDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalEndDate;
                        //tenant.StockTypeCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().StockTypeCode;
                    }

                    if (tenant.CurrencyId != null)
                    {
                        Currency cur = CurrencyRepository.GetSingleCurrency(tenant.CurrencyId, tenant.Id, true);
                        tenant.CurrencyCode = cur.Code;
                    }

                    if (tenant.AddressId != null)
                    {
                        AddressQuery addressQuery = new AddressQuery(id);
                        AddressPM add = addressQuery.GetSingleAddressPM(tenant.AddressId, tenant.Id);
                        tenant.CountryCode = add.CountryCode;
                        tenant.CountryName = add.CountryEnglishName;
                    }

                    entity = tenant;
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                }
                else
                {
                    entity = (TenantPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(id);
                TenantPM tenant = (from a in context.Tenants.Include("Address.Country")
                                   where a.Id == id
                                   select new TenantPM()
                                   {
                                       AddressId = a.AddressId,
                                       LocalAddressId = a.LocalAddressId,
                                       Company = a.Company,
                                       CurrencyId = a.CurrencyId,
                                       Direction = a.Direction,
                                       Email = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.Email : "",
                                       Format = a.Format,
                                       Id = a.Id,
                                       Language = a.Language,
                                       Website = a.Website,
                                       IATA = a.IATA,
                                       Signature = a.Signature,
                                       DimensionsUnitCode = a.DimensionsUnitCode,
                                       VolumeUnitCode = a.VolumeUnitCode,
                                       GrossWeightUnitCode = a.GrossWeightUnitCode,
                                       ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                       ExportFreightPrepaidCollectId = a.ExportFreightPrepaidCollectId,
                                       ExportOtherPrepaidCollectId = a.ExportOtherPrepaidCollectId,
                                       ImportFreightPrepaidCollectId = a.ImportFreightPrepaidCollectId,
                                       ImportOtherPrepaidCollectId = a.ImportOtherPrepaidCollectId,
                                       FreightCurrencyId = a.FreightCurrencyId,
                                       VatNumber = a.VatNumber,
                                       OtherChargesCurrencyId = a.OtherChargesCurrencyId,
                                       TimeZoneOffset = a.TimeZoneOffset,
                                       DayLightStartDate = a.DayLightStartDate,
                                       DayLightEndDate = a.DayLightEndDate,
                                       DayLightOffset = a.DayLightOffset,
                                       QuoteSaleCurrencyId = a.QuoteSaleCurrencyId,
                                       PaymentTermId = a.PaymentTermId,
                                       ProfitCurrencyId = a.ProfitCurrencyId,
                                       AgentId = a.AgentId,
                                       PasswordPolicyCode = a.PasswordPolicyCode,
                                       MasterExportFreightPrepaidCollectId = a.MasterExportFreightPrepaidCollectId,
                                       MasterExportOtherPrepaidCollectId = a.MasterExportOtherPrepaidCollectId,
                                       MasterImportFreightPrepaidCollectId = a.MasterImportFreightPrepaidCollectId,
                                       MasterImportOtherPrepaidCollectId = a.MasterImportOtherPrepaidCollectId,
                                       SearchFields = a.SearchFields,
                                       IsDataBackupBuilt = a.IsDataBackupBuilt,
                                       CountryCode = a.Address != null ? (a.Address.Country != null ? a.Address.Country.Code : null) : null,
                                       CountryName = a.Address != null ? (a.Address.Country != null ? a.Address.Country.EnglishName : null) : null,
                                       WeightMeasurementUnitCode = a.WeightMeasurementUnitCode,
                                       DateTimeFormat = a.DateTimeFormat,
                                       InvoiceSection1 = a.InvoiceSection1,
                                       InvoiceSection2 = a.InvoiceSection2,
                                       BankDetails = a.BankDetails,
                                       IsSharedLogisticsActivated = a.IsSharedLogisticsActivated,
                                       SharedLogisticsMessageLink = a.SharedLogisticsMessageLink,
                                       ProfitCurrencyRate = 1,
                                       CASSCode = a.CASSCode,
                                       LocalCustomsCode = a.LocalCustomsCode,
                                       IsHybrid = a.IsHybrid,
                                       VatUniqueTypeCode = a.VatUniqueTypeCode,
                                       VatMandatoryTypeCode = a.VatMandatoryTypeCode,
                                       VatUniqueCountryId = a.VatUniqueCountryId,
                                       VatMandatoryCountryId = a.VatMandatoryCountryId,
                                       IsCustomerTelRequired = a.IsCustomerTelRequired,
                                       IsCustomerFaxRequired = a.IsCustomerFaxRequired,
                                       IsPickDelAdrsRequired = a.IsPickDelAdrsRequired,
                                       VatMandatoryForPotentialCustomers = a.VatMandatoryForPotentialCustomers,
                                       IsCustomerAddress1Required = a.IsCustomerAddress1Required,
                                       HasPrimaryContact = a.HasPrimaryContact,
                                       DefaultQuestionnaireId = a.DefaultQuestionnaireId,
                                       IsQuoteSubjectEdited = a.IsQuoteSubjectEdited,
                                       AllowEAWBMoreThanTenPackages = a.AllowEAWBMoreThanTenPackages,
                                       IsMobileActivated = a.IsMobileActivated,
                                       RegulatedAgentNumber = a.RegulatedAgentNumber,
                                       RegulatedAgentRegimeActivated = a.RegulatedAgentRegimeActivated,
                                    
                                       CustomerId = a.CustomerId,
                                       CustomerName = a.CustomerCard != null ? a.CustomerCard.EnglishName : null,
                                       IsCustomerTenantShare = a.IsCustomerTenantShare,
                                    
                                       CustomerTenantShareExportFile = a.CustomerTenantShareExportFile,
                                       AllowAgentInCustomersLOV = a.AllowAgentInCustomersLOV,
                                       AllowCustomersInAgentsLOV = a.AllowCustomersInAgentsLOV,
                                       IsPotentialTelRequired = a.IsPotentialTelRequired,
                                       IsPotentialFaxRequired = a.IsPotentialFaxRequired,
                                       VatFormatTypeCode = a.VatFormatTypeCode,
                                       VatFormatCountryId = a.VatFormatCountryId,
                                       IsNumeric = a.IsNumeric,
                                       VatSize = a.VatSize,
                              
                                       IsWebAccessActivated = a.IsWebAccessActivated,
                                       IsCorrespondenceRightToLeftEnabled = a.IsCorrespondenceRightToLeftEnabled,
                                       IsNotesRightToLeftEnabled = a.IsNotesRightToLeftEnabled,
                                       IsInternalTicketByDefault = a.IsInternalTicketByDefault,
                                       ProrateMasterReceivables = a.ProrateMasterReceivables,
                                       SCACCode = a.SCACCode,
                                       ExportQuotationsToIntegratedSystem = a.ExportQuotationsToIntegratedSystem,
                                       //DropBoxAccessToken = a.DropBoxAccessToken
                                       FMCNumber = a.FMCNumber,
                                       TenantVATManagement = a.TenantVATManagement,
                                     
                                       TemperatureUnitCode = a.TemperatureUnitCode,
                                       DefaultSLAId = a.DefaultSLAId,
                                   
                                       NumberFormatCode = a.NumberFormatCode,
                                       EcommerceSupportEmail = a.EcommerceSupportEmail,
                                       CAAT = a.CAAT,
                                       CBSA = a.CBSA,
                                       IsTestTenant = a.IsTestTenant,
                                       CheckDigitControlAlgorithmCode = a.CheckDigitControlAlgorithmCode,
                                       ApplyVATForAllPartners = a.ApplyVATForAllPartners,
                                       HideFCLAllIn = a.HideFCLAllIn,
                                   }).FirstOrDefault();

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalObjectContext = GlobalContext.GetContext();
                    tenant.PackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().PackageCode;
                    tenant.TemporalPackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalPackageCode;
                    tenant.TemporalStartDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalStartDate;
                    tenant.TemporalEndDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalEndDate;
                    // tenant.StockTypeCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().StockTypeCode;
                }
                if (tenant.CurrencyId != null)
                {
                    Currency cur = CurrencyRepository.GetSingleCurrency(tenant.CurrencyId, tenant.Id, true);
                    tenant.CurrencyCode = cur.Code;
                }

                if (tenant.AddressId != null)
                {
                    AddressQuery addressQuery = new AddressQuery(id);
                    AddressPM add = addressQuery.GetSingleAddressPM(tenant.AddressId, tenant.Id);
                    tenant.CountryCode = add.CountryCode;
                    tenant.CountryName = add.CountryEnglishName;
                }

                entity = tenant;
            }
            return entity;
        }

        public static TenantPM GetSingleTenantPM(int id, bool fromcache)
        {
            string entityName = "TenantPM" + id;
            TenantPM entity;

            if (fromcache)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(id);
                        TenantPM tenant = (from a in context.Tenants.Include("Address")
                                           where a.Id == id
                                           select new TenantPM()
                                           {
                                               AddressId = a.AddressId,
                                               LocalAddressId = a.LocalAddressId,
                                               Company = a.Company,
                                               CurrencyId = a.CurrencyId,
                                               Direction = a.Direction,
                                               Email = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.Email : "",
                                               Format = a.Format,
                                               Id = a.Id,
                                               Language = a.Language,
                                               Website = a.Website,
                                               IATA = a.IATA,
                                               Signature = a.Signature,
                                               DimensionsUnitCode = a.DimensionsUnitCode,
                                               VolumeUnitCode = a.VolumeUnitCode,
                                               GrossWeightUnitCode = a.GrossWeightUnitCode,
                                               ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                               ExportFreightPrepaidCollectId = a.ExportFreightPrepaidCollectId,
                                               ExportOtherPrepaidCollectId = a.ExportOtherPrepaidCollectId,
                                               ImportFreightPrepaidCollectId = a.ImportFreightPrepaidCollectId,
                                               ImportOtherPrepaidCollectId = a.ImportOtherPrepaidCollectId,
                                               FreightCurrencyId = a.FreightCurrencyId,
                                               VatNumber = a.VatNumber,
                                               OtherChargesCurrencyId = a.OtherChargesCurrencyId,
                                               TimeZoneOffset = a.TimeZoneOffset,
                                               DayLightStartDate = a.DayLightStartDate,
                                               DayLightEndDate = a.DayLightEndDate,
                                               DayLightOffset = a.DayLightOffset,
                                               QuoteSaleCurrencyId = a.QuoteSaleCurrencyId,
                                               PaymentTermId = a.PaymentTermId,
                                               ProfitCurrencyId = a.ProfitCurrencyId,
                                               AgentId = a.AgentId,
                                               PasswordPolicyCode = a.PasswordPolicyCode,
                                               MasterExportFreightPrepaidCollectId = a.MasterExportFreightPrepaidCollectId,
                                               MasterExportOtherPrepaidCollectId = a.MasterExportOtherPrepaidCollectId,
                                               MasterImportFreightPrepaidCollectId = a.MasterImportFreightPrepaidCollectId,
                                               MasterImportOtherPrepaidCollectId = a.MasterImportOtherPrepaidCollectId,
                                               SearchFields = a.SearchFields,
                                               IsDataBackupBuilt = a.IsDataBackupBuilt,
                                               WeightMeasurementUnitCode = a.WeightMeasurementUnitCode,
                                               DateTimeFormat = a.DateTimeFormat,
                                               InvoiceSection1 = a.InvoiceSection1,
                                               InvoiceSection2 = a.InvoiceSection2,
                                               BankDetails = a.BankDetails,
                                               IsSharedLogisticsActivated = a.IsSharedLogisticsActivated,
                                               SharedLogisticsMessageLink = a.SharedLogisticsMessageLink,
                                               ProfitCurrencyRate = 1,
                                               CASSCode = a.CASSCode,
                                               IsHybrid = a.IsHybrid,
                                               LocalCustomsCode = a.LocalCustomsCode,
                                               CountryCode = a.Address != null ? (a.Address.Country != null ? a.Address.Country.Code : null) : null,
                                               CountryName = a.Address != null ? (a.Address.Country != null ? a.Address.Country.EnglishName : null) : null,
                                               VatUniqueTypeCode = a.VatUniqueTypeCode,
                                               VatMandatoryTypeCode = a.VatMandatoryTypeCode,
                                               VatUniqueCountryId = a.VatUniqueCountryId,
                                               VatMandatoryCountryId = a.VatMandatoryCountryId,
                                               IsCustomerTelRequired = a.IsCustomerTelRequired,
                                               IsCustomerFaxRequired = a.IsCustomerFaxRequired,
                                               IsPickDelAdrsRequired = a.IsPickDelAdrsRequired,
                                               VatMandatoryForPotentialCustomers = a.VatMandatoryForPotentialCustomers,
                                               IsCustomerAddress1Required = a.IsCustomerAddress1Required,
                                               HasPrimaryContact = a.HasPrimaryContact,
                                               DefaultQuestionnaireId = a.DefaultQuestionnaireId,
                                               IsQuoteSubjectEdited = a.IsQuoteSubjectEdited,
                                               AllowEAWBMoreThanTenPackages = a.AllowEAWBMoreThanTenPackages,
                                               IsMobileActivated = a.IsMobileActivated,
                                               RegulatedAgentNumber = a.RegulatedAgentNumber,
                                               RegulatedAgentRegimeActivated = a.RegulatedAgentRegimeActivated,
                                            
                                               CustomerId = a.CustomerId,
                                               CustomerName = a.CustomerCard != null ? a.CustomerCard.EnglishName : null,
                                               IsCustomerTenantShare = a.IsCustomerTenantShare,
                                            
                                               CustomerTenantShareExportFile = a.CustomerTenantShareExportFile,
                                               AllowAgentInCustomersLOV = a.AllowAgentInCustomersLOV,
                                               AllowCustomersInAgentsLOV = a.AllowCustomersInAgentsLOV,
                                               IsPotentialTelRequired = a.IsPotentialTelRequired,
                                               IsPotentialFaxRequired = a.IsPotentialFaxRequired,
                                               VatFormatTypeCode = a.VatFormatTypeCode,
                                               VatFormatCountryId = a.VatFormatCountryId,
                                               IsNumeric = a.IsNumeric,
                                               VatSize = a.VatSize,
                                             
                                               IsWebAccessActivated = a.IsWebAccessActivated,
                                               IsCorrespondenceRightToLeftEnabled = a.IsCorrespondenceRightToLeftEnabled,
                                               IsNotesRightToLeftEnabled = a.IsNotesRightToLeftEnabled,
                                               AccountingActivated = a.AccountingActivated,
                                               AccountingActivationDate = a.AccountingActivationDate,
                                               IsInternalTicketByDefault = a.IsInternalTicketByDefault,
                                               ProrateMasterReceivables = a.ProrateMasterReceivables,
                                               ExportQuotationsToIntegratedSystem = a.ExportQuotationsToIntegratedSystem,
                                               SCACCode = a.SCACCode,
                                               //DropBoxAccessToken = a.DropBoxAccessToken
                                               FMCNumber = a.FMCNumber,
                                               TenantVATManagement = a.TenantVATManagement,
                                         
                                               TemperatureUnitCode = a.TemperatureUnitCode,
                                               DefaultSLAId = a.DefaultSLAId,
                                        
                                               NumberFormatCode = a.NumberFormatCode,
                                               EcommerceSupportEmail = a.EcommerceSupportEmail,
                                               CAAT = a.CAAT,
                                               CBSA = a.CBSA,
                                               IsTestTenant = a.IsTestTenant,
                                               CheckDigitControlAlgorithmCode = a.CheckDigitControlAlgorithmCode,
                                               ApplyVATForAllPartners = a.ApplyVATForAllPartners,
                                               HideFCLAllIn = a.HideFCLAllIn,
                                           }).FirstOrDefault();

                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IGlobalContext globalObjectContext = GlobalContext.GetContext();
                            tenant.PackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().PackageCode;
                            tenant.TemporalPackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalPackageCode;
                            tenant.TemporalStartDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalStartDate;
                            tenant.TemporalEndDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalEndDate;
                            tenant.PrivateLabelId = globalObjectContext.GlobalTenants.Where(d => d.Id == tenant.Id).FirstOrDefault().PrivateLabelId;
                            //tenant.StockTypeCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().StockTypeCode;
                        }

                        if (tenant.CurrencyId != null)
                        {
                            Currency cur = CurrencyRepository.GetSingleCurrency(tenant.CurrencyId, tenant.Id, true);
                            tenant.CurrencyCode = cur.Code;
                        }

                        if (tenant.AddressId != null)
                        {
                            AddressQuery addressQuery = new AddressQuery(id);
                            AddressPM add = addressQuery.GetSingleAddressPM(tenant.AddressId, tenant.Id);
                            tenant.CountryCode = add.CountryCode;
                            tenant.CountryName = add.CountryEnglishName;
                        }

                        entity = tenant;
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        entity = (TenantPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    ICommonDataContext context = CommonDataContext.GetContext(id);
                    TenantPM tenant = (from a in context.Tenants.Include("Address")
                                       where a.Id == id
                                       select new TenantPM()
                                       {
                                           AddressId = a.AddressId,
                                           LocalAddressId = a.LocalAddressId,
                                           Company = a.Company,
                                           CurrencyId = a.CurrencyId,
                                           Direction = a.Direction,
                                           Email = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.Email : "",
                                           Format = a.Format,
                                           Id = a.Id,
                                           Language = a.Language,
                                           Website = a.Website,
                                           IATA = a.IATA,
                                           Signature = a.Signature,
                                           DimensionsUnitCode = a.DimensionsUnitCode,
                                           VolumeUnitCode = a.VolumeUnitCode,
                                           GrossWeightUnitCode = a.GrossWeightUnitCode,
                                           ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                           ExportFreightPrepaidCollectId = a.ExportFreightPrepaidCollectId,
                                           ExportOtherPrepaidCollectId = a.ExportOtherPrepaidCollectId,
                                           ImportFreightPrepaidCollectId = a.ImportFreightPrepaidCollectId,
                                           ImportOtherPrepaidCollectId = a.ImportOtherPrepaidCollectId,
                                           FreightCurrencyId = a.FreightCurrencyId,
                                           VatNumber = a.VatNumber,
                                           OtherChargesCurrencyId = a.OtherChargesCurrencyId,
                                           TimeZoneOffset = a.TimeZoneOffset,
                                           DayLightStartDate = a.DayLightStartDate,
                                           DayLightEndDate = a.DayLightEndDate,
                                           DayLightOffset = a.DayLightOffset,
                                           QuoteSaleCurrencyId = a.QuoteSaleCurrencyId,
                                           PaymentTermId = a.PaymentTermId,
                                           ProfitCurrencyId = a.ProfitCurrencyId,
                                           AgentId = a.AgentId,
                                           PasswordPolicyCode = a.PasswordPolicyCode,
                                           MasterExportFreightPrepaidCollectId = a.MasterExportFreightPrepaidCollectId,
                                           MasterExportOtherPrepaidCollectId = a.MasterExportOtherPrepaidCollectId,
                                           MasterImportFreightPrepaidCollectId = a.MasterImportFreightPrepaidCollectId,
                                           MasterImportOtherPrepaidCollectId = a.MasterImportOtherPrepaidCollectId,
                                           SearchFields = a.SearchFields,
                                           IsDataBackupBuilt = a.IsDataBackupBuilt,
                                           WeightMeasurementUnitCode = a.WeightMeasurementUnitCode,
                                           DateTimeFormat = a.DateTimeFormat,
                                           InvoiceSection1 = a.InvoiceSection1,
                                           InvoiceSection2 = a.InvoiceSection2,
                                           BankDetails = a.BankDetails,
                                           IsSharedLogisticsActivated = a.IsSharedLogisticsActivated,
                                           SharedLogisticsMessageLink = a.SharedLogisticsMessageLink,
                                           ProfitCurrencyRate = 1,
                                           CASSCode = a.CASSCode,
                                           LocalCustomsCode = a.LocalCustomsCode,
                                           CountryCode = a.Address != null ? (a.Address.Country != null ? a.Address.Country.Code : null) : null,
                                           CountryName = a.Address != null ? (a.Address.Country != null ? a.Address.Country.EnglishName : null) : null,
                                           IsHybrid = a.IsHybrid,
                                           VatUniqueTypeCode = a.VatUniqueTypeCode,
                                           VatMandatoryTypeCode = a.VatMandatoryTypeCode,
                                           VatUniqueCountryId = a.VatUniqueCountryId,
                                           VatMandatoryCountryId = a.VatMandatoryCountryId,
                                           IsCustomerTelRequired = a.IsCustomerTelRequired,
                                           IsCustomerFaxRequired = a.IsCustomerFaxRequired,
                                           IsPickDelAdrsRequired = a.IsPickDelAdrsRequired,
                                           VatMandatoryForPotentialCustomers = a.VatMandatoryForPotentialCustomers,
                                           IsCustomerAddress1Required = a.IsCustomerAddress1Required,
                                           HasPrimaryContact = a.HasPrimaryContact,
                                           DefaultQuestionnaireId = a.DefaultQuestionnaireId,
                                           IsQuoteSubjectEdited = a.IsQuoteSubjectEdited,
                                           AllowEAWBMoreThanTenPackages = a.AllowEAWBMoreThanTenPackages,
                                           IsMobileActivated = a.IsMobileActivated,
                                           RegulatedAgentNumber = a.RegulatedAgentNumber,
                                           RegulatedAgentRegimeActivated = a.RegulatedAgentRegimeActivated,
                                        
                                           CustomerId = a.CustomerId,
                                           CustomerName = a.CustomerCard != null ? a.CustomerCard.EnglishName : null,
                                           IsCustomerTenantShare = a.IsCustomerTenantShare,
                                         
                                           CustomerTenantShareExportFile = a.CustomerTenantShareExportFile,
                                           AllowAgentInCustomersLOV = a.AllowAgentInCustomersLOV,
                                           AllowCustomersInAgentsLOV = a.AllowCustomersInAgentsLOV,
                                           IsPotentialTelRequired = a.IsPotentialTelRequired,
                                           IsPotentialFaxRequired = a.IsPotentialFaxRequired,
                                           VatFormatTypeCode = a.VatFormatTypeCode,
                                           VatFormatCountryId = a.VatFormatCountryId,
                                           IsNumeric = a.IsNumeric,
                                           VatSize = a.VatSize,
                                         
                                           IsWebAccessActivated = a.IsWebAccessActivated,
                                           IsCorrespondenceRightToLeftEnabled = a.IsCorrespondenceRightToLeftEnabled,
                                           IsNotesRightToLeftEnabled = a.IsNotesRightToLeftEnabled,
                                           AccountingActivated = a.AccountingActivated,
                                           AccountingActivationDate = a.AccountingActivationDate,
                                           IsInternalTicketByDefault = a.IsInternalTicketByDefault,
                                           ProrateMasterReceivables = a.ProrateMasterReceivables,
                                           SCACCode = a.SCACCode,
                                           ExportQuotationsToIntegratedSystem = a.ExportQuotationsToIntegratedSystem,
                                           //DropBoxAccessToken = a.DropBoxAccessToken
                                           FMCNumber = a.FMCNumber,
                                           TenantVATManagement = a.TenantVATManagement,
                                  
                                           TemperatureUnitCode = a.TemperatureUnitCode,
                                           DefaultSLAId = a.DefaultSLAId,
                                        
                                        
                                           NumberFormatCode = a.NumberFormatCode,
                                           EcommerceSupportEmail = a.EcommerceSupportEmail,
                                           CAAT = a.CAAT,
                                           CBSA = a.CBSA,
                                           IsTestTenant = a.IsTestTenant,
                                           CheckDigitControlAlgorithmCode = a.CheckDigitControlAlgorithmCode,
                                           ApplyVATForAllPartners = a.ApplyVATForAllPartners,
                                           HideFCLAllIn = a.HideFCLAllIn,

                                       }).FirstOrDefault();

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalObjectContext = GlobalContext.GetContext();
                        tenant.PackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().PackageCode;
                        tenant.TemporalPackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalPackageCode;
                        tenant.TemporalStartDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalStartDate;
                        tenant.TemporalEndDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalEndDate;
                        tenant.PrivateLabelId = globalObjectContext.GlobalTenants.Where(d => d.Id == tenant.Id).FirstOrDefault().PrivateLabelId;
                        //tenant.StockTypeCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().StockTypeCode;
                    }

                    if (tenant.CurrencyId != null)
                    {
                        Currency cur = CurrencyRepository.GetSingleCurrency(tenant.CurrencyId, tenant.Id, true);
                        tenant.CurrencyCode = cur.Code;
                    }

                    if (tenant.AddressId != null)
                    {
                        AddressQuery addressQuery = new AddressQuery(id);
                        AddressPM add = addressQuery.GetSingleAddressPM(tenant.AddressId, tenant.Id);
                        tenant.CountryCode = add.CountryCode;
                        tenant.CountryName = add.CountryEnglishName;
                    }

                    entity = tenant;
                }
            }
            else
            {
                ICommonDataContext context = CommonDataContext.GetContext(id);
                TenantPM tenant = (from a in context.Tenants.Include("Address")
                                   where a.Id == id
                                   select new TenantPM()
                                   {
                                       AddressId = a.AddressId,
                                       LocalAddressId = a.LocalAddressId,
                                       Company = a.Company,
                                       CurrencyId = a.CurrencyId,
                                       Direction = a.Direction,
                                       Email = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.Email : "",
                                       Format = a.Format,
                                       Id = a.Id,
                                       Language = a.Language,
                                       Website = a.Website,
                                       IATA = a.IATA,
                                       Signature = a.Signature,
                                       DimensionsUnitCode = a.DimensionsUnitCode,
                                       VolumeUnitCode = a.VolumeUnitCode,
                                       GrossWeightUnitCode = a.GrossWeightUnitCode,
                                       ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                       ExportFreightPrepaidCollectId = a.ExportFreightPrepaidCollectId,
                                       ExportOtherPrepaidCollectId = a.ExportOtherPrepaidCollectId,
                                       ImportFreightPrepaidCollectId = a.ImportFreightPrepaidCollectId,
                                       ImportOtherPrepaidCollectId = a.ImportOtherPrepaidCollectId,
                                       FreightCurrencyId = a.FreightCurrencyId,
                                       VatNumber = a.VatNumber,
                                       OtherChargesCurrencyId = a.OtherChargesCurrencyId,
                                       TimeZoneOffset = a.TimeZoneOffset,
                                       DayLightStartDate = a.DayLightStartDate,
                                       DayLightEndDate = a.DayLightEndDate,
                                       DayLightOffset = a.DayLightOffset,
                                       QuoteSaleCurrencyId = a.QuoteSaleCurrencyId,
                                       PaymentTermId = a.PaymentTermId,
                                       ProfitCurrencyId = a.ProfitCurrencyId,
                                       AgentId = a.AgentId,
                                       PasswordPolicyCode = a.PasswordPolicyCode,
                                       MasterExportFreightPrepaidCollectId = a.MasterExportFreightPrepaidCollectId,
                                       MasterExportOtherPrepaidCollectId = a.MasterExportOtherPrepaidCollectId,
                                       MasterImportFreightPrepaidCollectId = a.MasterImportFreightPrepaidCollectId,
                                       MasterImportOtherPrepaidCollectId = a.MasterImportOtherPrepaidCollectId,
                                       SearchFields = a.SearchFields,
                                       IsDataBackupBuilt = a.IsDataBackupBuilt,
                                       WeightMeasurementUnitCode = a.WeightMeasurementUnitCode,
                                       DateTimeFormat = a.DateTimeFormat,
                                       InvoiceSection1 = a.InvoiceSection1,
                                       InvoiceSection2 = a.InvoiceSection2,
                                       BankDetails = a.BankDetails,
                                       IsSharedLogisticsActivated = a.IsSharedLogisticsActivated,
                                       SharedLogisticsMessageLink = a.SharedLogisticsMessageLink,
                                       ProfitCurrencyRate = 1,
                                       CASSCode = a.CASSCode,
                                       LocalCustomsCode = a.LocalCustomsCode,
                                       CountryCode = a.Address != null ? (a.Address.Country != null ? a.Address.Country.Code : null) : null,
                                       CountryName = a.Address != null ? (a.Address.Country != null ? a.Address.Country.EnglishName : null) : null,
                                       IsHybrid = a.IsHybrid,
                                       VatUniqueTypeCode = a.VatUniqueTypeCode,
                                       VatMandatoryTypeCode = a.VatMandatoryTypeCode,
                                       VatUniqueCountryId = a.VatUniqueCountryId,
                                       VatMandatoryCountryId = a.VatMandatoryCountryId,
                                       IsCustomerTelRequired = a.IsCustomerTelRequired,
                                       IsCustomerFaxRequired = a.IsCustomerFaxRequired,
                                       IsPickDelAdrsRequired = a.IsPickDelAdrsRequired,
                                       VatMandatoryForPotentialCustomers = a.VatMandatoryForPotentialCustomers,
                                       IsCustomerAddress1Required = a.IsCustomerAddress1Required,
                                       HasPrimaryContact = a.HasPrimaryContact,
                                       DefaultQuestionnaireId = a.DefaultQuestionnaireId,
                                       IsQuoteSubjectEdited = a.IsQuoteSubjectEdited,
                                       AllowEAWBMoreThanTenPackages = a.AllowEAWBMoreThanTenPackages,
                                       IsMobileActivated = a.IsMobileActivated,
                                       RegulatedAgentNumber = a.RegulatedAgentNumber,
                                       RegulatedAgentRegimeActivated = a.RegulatedAgentRegimeActivated,
                                  
                                       CustomerId = a.CustomerId,
                                       CustomerName = a.CustomerCard != null ? a.CustomerCard.EnglishName : null,
                                       IsCustomerTenantShare = a.IsCustomerTenantShare,
                                    
                                       CustomerTenantShareExportFile = a.CustomerTenantShareExportFile,
                                       AllowAgentInCustomersLOV = a.AllowAgentInCustomersLOV,
                                       AllowCustomersInAgentsLOV = a.AllowCustomersInAgentsLOV,
                                       IsPotentialTelRequired = a.IsPotentialTelRequired,
                                       IsPotentialFaxRequired = a.IsPotentialFaxRequired,
                                       VatFormatTypeCode = a.VatFormatTypeCode,
                                       VatFormatCountryId = a.VatFormatCountryId,
                                       IsNumeric = a.IsNumeric,
                                       VatSize = a.VatSize,
                                     
                                       IsWebAccessActivated = a.IsWebAccessActivated,
                                       IsCorrespondenceRightToLeftEnabled = a.IsCorrespondenceRightToLeftEnabled,
                                       IsNotesRightToLeftEnabled = a.IsNotesRightToLeftEnabled,
                                       AccountingActivated = a.AccountingActivated,
                                       AccountingActivationDate = a.AccountingActivationDate,
                                       IsInternalTicketByDefault = a.IsInternalTicketByDefault,
                                       ProrateMasterReceivables = a.ProrateMasterReceivables,
                                       SCACCode = a.SCACCode,
                                       ExportQuotationsToIntegratedSystem = a.ExportQuotationsToIntegratedSystem,
                                       //DropBoxAccessToken = a.DropBoxAccessToken
                                       FMCNumber = a.FMCNumber,
                                       TenantVATManagement = a.TenantVATManagement,
                                    
                                       TemperatureUnitCode = a.TemperatureUnitCode,
                                       DefaultSLAId = a.DefaultSLAId,
                                    
                                       NumberFormatCode = a.NumberFormatCode,
                                       EcommerceSupportEmail = a.EcommerceSupportEmail,
                                       CAAT = a.CAAT,
                                       CBSA = a.CBSA,
                                       IsTestTenant = a.IsTestTenant,
                                       CheckDigitControlAlgorithmCode = a.CheckDigitControlAlgorithmCode,
                                       ApplyVATForAllPartners = a.ApplyVATForAllPartners,
                                       HideFCLAllIn = a.HideFCLAllIn,

                                   }).FirstOrDefault();
                if (tenant != null)
                {
                    if (tenant.AddressId != null)
                    {
                        AddressRepository addressrep = new AddressRepository(context);
                        Address address = addressrep.GetSingleAddress(tenant.AddressId, tenant.Id);
                        if (address != null)
                        {
                            tenant.CountryName = address.Country.EnglishName;
                            tenant.CompanyAddress = address.Name;
                        }
                    }

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalObjectContext = GlobalContext.GetContext();
                        tenant.PackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().PackageCode;
                        tenant.TemporalPackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalPackageCode;
                        tenant.TemporalStartDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalStartDate;
                        tenant.TemporalEndDate = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().TemporalEndDate;
                        tenant.PrivateLabelId = globalObjectContext.GlobalTenants.Where(d => d.Id == tenant.Id).FirstOrDefault().PrivateLabelId;
                        //tenant.StockTypeCode = globalObjectContext.TenantManagements.Where(d => d.Id == tenant.Id).FirstOrDefault().StockTypeCode;
                    }

                    if (tenant.CurrencyId != null)
                    {
                        Currency cur = CurrencyRepository.GetSingleCurrency(tenant.CurrencyId, tenant.Id, true);
                        if (cur == null)
                        {
                            cur = CurrencyRepository.GetSingleCurrency(tenant.CurrencyId, 0, true);
                        }
                        tenant.CurrencyCode = cur.Code;
                    }

                    if (tenant.AddressId != null)
                    {
                        AddressQuery addressQuery = new AddressQuery(id);
                        AddressPM add = addressQuery.GetSingleAddressPM(tenant.AddressId, tenant.Id);
                        tenant.CountryCode = add.CountryCode;
                        tenant.CountryName = add.CountryEnglishName;
                    }


                }
                entity = tenant;
            }
            return entity;
        }

        public TenantPM GetTenantFromDB(int tenant)
        {
            TenantPM entityPM = new TenantPM();

            Tenant myPOCO = (from a in repository.context.Tenants.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country").Include("LogBoxTenantSetting")
                             where a.Id == tenant
                             select a).FirstOrDefault();

            if (myPOCO != null)
            {
                entityPM = new TenantPM()
                {
                    Id = myPOCO.Id,
                    AddressId = myPOCO.AddressId,
                    LocalAddressId = myPOCO.LocalAddressId,
                    CompanyAddress = myPOCO.Address != null ? myPOCO.Address.Name : null,
                    Company = myPOCO.Company,
                    Signature = myPOCO.Signature,
                    IATA = myPOCO.IATA,
                    VatNumber = myPOCO.VatNumber,
                    SearchFields = myPOCO.SearchFields,
                    PaymentTermId = myPOCO.PaymentTermId,
                    PaymentTermName = myPOCO.PaymentTerm != null ? myPOCO.PaymentTerm.EnglishName : null,
                    AgentId = myPOCO.AgentId,
                    AgentName = myPOCO.AgentCard != null ? myPOCO.AgentCard.EnglishName : null,
                    CurrencyId = myPOCO.CurrencyId,
                    AccountingCurrencyCode = myPOCO.Currency != null ? myPOCO.Currency.Code : null,
                    AccountingCurrencyName = myPOCO.Currency != null ? myPOCO.Currency.EnglishName : null,
                    ProfitCurrencyId = myPOCO.ProfitCurrencyId,
                    ProfitCurrencyCode = myPOCO.ProfitCurrency != null ? myPOCO.ProfitCurrency.Code : null,
                    FreightCurrencyId = myPOCO.FreightCurrencyId,
                    FreightCurrencyCode = myPOCO.FreightCurrency != null ? myPOCO.FreightCurrency.Code : null,
                    OtherChargesCurrencyId = myPOCO.OtherChargesCurrencyId,
                    OtherChargesCurrencyCode = myPOCO.OtherChargesCurrency != null ? myPOCO.OtherChargesCurrency.Code : null,
                    QuoteSaleCurrencyId = myPOCO.QuoteSaleCurrencyId,
                    QuoteSaleCurrencyCode = myPOCO.QuoteSaleCurrency != null ? myPOCO.QuoteSaleCurrency.Code : null,
                    VolumeUnitCode = myPOCO.VolumeUnitCode,
                    DimensionsUnitCode = myPOCO.DimensionsUnitCode,
                    GrossWeightUnitCode = myPOCO.GrossWeightUnitCode,
                    ChargeableWeightUnitCode = myPOCO.ChargeableWeightUnitCode,
                    WeightMeasurementUnitCode = myPOCO.WeightMeasurementUnitCode,
                    ExportFreightPrepaidCollectId = myPOCO.ExportFreightPrepaidCollectId,
                    ImportFreightPrepaidCollectId = myPOCO.ImportFreightPrepaidCollectId,
                    ExportOtherPrepaidCollectId = myPOCO.ExportOtherPrepaidCollectId,
                    ImportOtherPrepaidCollectId = myPOCO.ImportOtherPrepaidCollectId,
                    MasterExportFreightPrepaidCollectId = myPOCO.MasterExportFreightPrepaidCollectId,
                    MasterImportFreightPrepaidCollectId = myPOCO.MasterImportFreightPrepaidCollectId,
                    MasterExportOtherPrepaidCollectId = myPOCO.MasterExportOtherPrepaidCollectId,
                    MasterImportOtherPrepaidCollectId = myPOCO.MasterImportOtherPrepaidCollectId,
                    TimeZoneOffset = myPOCO.TimeZoneOffset,
                    Language = myPOCO.Language,
                    Email = myPOCO.CustomerCard != null && myPOCO.CustomerCard.PrimaryContact != null ? myPOCO.CustomerCard.PrimaryContact.Email : "",
                    Website = myPOCO.Website,
                    Format = myPOCO.Format,
                    Direction = myPOCO.Direction,
                    DayLightOffset = myPOCO.DayLightOffset,
                    DayLightStartDate = myPOCO.DayLightStartDate,
                    DayLightEndDate = myPOCO.DayLightEndDate,
                    PasswordPolicyCode = myPOCO.PasswordPolicyCode,
                    PasswordStrength = myPOCO.PasswordPolicy != null ? myPOCO.PasswordPolicy.PasswordStrength : null,
                    IsDataBackupBuilt = myPOCO.IsDataBackupBuilt,
                    CountryCode = myPOCO.Address != null ? (myPOCO.Address.Country != null ? myPOCO.Address.Country.Code : null) : null,
                    CountryName = myPOCO.Address != null ? (myPOCO.Address.Country != null ? myPOCO.Address.Country.EnglishName : null) : null,
                    DateTimeFormat = myPOCO.DateTimeFormat,
                    InvoiceSection1 = myPOCO.InvoiceSection1,
                    InvoiceSection2 = myPOCO.InvoiceSection2,
                    BankDetails = myPOCO.BankDetails,
                    IsSharedLogisticsActivated = myPOCO.IsSharedLogisticsActivated,
                    SharedLogisticsMessageLink = myPOCO.SharedLogisticsMessageLink,
                    ProfitCurrencyRate = 1,
                    CASSCode = myPOCO.CASSCode,
                    IsHybrid = myPOCO.IsHybrid,
                    LocalCustomsCode = myPOCO.LocalCustomsCode,
                    VatUniqueTypeCode = myPOCO.VatUniqueTypeCode,
                    VatMandatoryTypeCode = myPOCO.VatMandatoryTypeCode,
                    VatUniqueCountryId = myPOCO.VatUniqueCountryId,
                    VatMandatoryCountryId = myPOCO.VatMandatoryCountryId,
                    IsCustomerTelRequired = myPOCO.IsCustomerTelRequired,
                    IsCustomerFaxRequired = myPOCO.IsCustomerFaxRequired,
                    IsPickDelAdrsRequired = myPOCO.IsPickDelAdrsRequired,
                    VatMandatoryForPotentialCustomers = myPOCO.VatMandatoryForPotentialCustomers,
                    IsCustomerAddress1Required = myPOCO.IsCustomerAddress1Required,
                    HasPrimaryContact = myPOCO.HasPrimaryContact,
                    DefaultQuestionnaireId = myPOCO.DefaultQuestionnaireId,
                    IsQuoteSubjectEdited = myPOCO.IsQuoteSubjectEdited,
                    AllowEAWBMoreThanTenPackages = myPOCO.AllowEAWBMoreThanTenPackages,
                    IsMobileActivated = myPOCO.IsMobileActivated,
                    RegulatedAgentNumber = myPOCO.RegulatedAgentNumber,
                    RegulatedAgentRegimeActivated = myPOCO.RegulatedAgentRegimeActivated,

                    CustomerId = myPOCO.CustomerId,
                    CustomerName = myPOCO.CustomerCard != null ? myPOCO.CustomerCard.EnglishName : null,
                    IsCustomerTenantShare = myPOCO.IsCustomerTenantShare,

                    CustomerTenantShareExportFile = myPOCO.CustomerTenantShareExportFile,
                    AllowAgentInCustomersLOV = myPOCO.AllowAgentInCustomersLOV,
                    AllowCustomersInAgentsLOV = myPOCO.AllowCustomersInAgentsLOV,
                    IsPotentialTelRequired = myPOCO.IsPotentialTelRequired,
                    IsPotentialFaxRequired = myPOCO.IsPotentialFaxRequired,
                    VatFormatTypeCode = myPOCO.VatFormatTypeCode,
                    VatFormatCountryId = myPOCO.VatFormatCountryId,
                    IsNumeric = myPOCO.IsNumeric,
                    VatSize = myPOCO.VatSize,

                    IsWebAccessActivated = myPOCO.IsWebAccessActivated,
                    IsCorrespondenceRightToLeftEnabled = myPOCO.IsCorrespondenceRightToLeftEnabled,
                    IsNotesRightToLeftEnabled = myPOCO.IsNotesRightToLeftEnabled,
                    AccountingActivationDate = myPOCO.AccountingActivationDate,
                    AccountingActivated = myPOCO.AccountingActivated,
                    IsInternalTicketByDefault = myPOCO.IsInternalTicketByDefault,
                    ProrateMasterReceivables = myPOCO.ProrateMasterReceivables,
                    SCACCode = myPOCO.SCACCode,
                    ExportQuotationsToIntegratedSystem = myPOCO.ExportQuotationsToIntegratedSystem,
                    FMCNumber = myPOCO.FMCNumber,

                    TenantVATManagement = myPOCO.TenantVATManagement,
                    LayoutDirection = myPOCO.LayoutDirection,
                    TemperatureUnitCode = myPOCO.TemperatureUnitCode,
                    DefaultSLAId = myPOCO.DefaultSLAId,

                    NumberFormatCode = myPOCO.NumberFormatCode,
                    EcommerceSupportEmail = myPOCO.EcommerceSupportEmail,
                    CAAT = myPOCO.CAAT,
                    CBSA = myPOCO.CBSA,
                    IsTestTenant = myPOCO.IsTestTenant,
                    CheckDigitControlAlgorithmCode = myPOCO.CheckDigitControlAlgorithmCode,
                    ApplyVATForAllPartners = myPOCO.ApplyVATForAllPartners,
                    IsDocumentsArchive = myPOCO.LogBoxTenantSetting.IsDocumentsArchive,
                    CustomerTenantShareImportFile = myPOCO.LogBoxTenantSetting.CustomerTenantShareImportFile,
                    AutoArchiveOnInvoice = myPOCO.LogBoxTenantSetting.AutoArchiveOnInvoice,
                    StockTypeCode = myPOCO.LogBoxTenantSetting != null ? myPOCO.LogBoxTenantSetting.StockTypeCode : null,
                    DocumentShareAsDefault = myPOCO.LogBoxTenantSetting.DocumentShareAsDefault,
                    LogBoxAdminUserId = myPOCO.LogBoxTenantSetting != null ? myPOCO.LogBoxTenantSetting.LogBoxAdminUserId : null,
                    HideFCLAllIn = myPOCO.HideFCLAllIn,
                };

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalObjectContext = GlobalContext.GetContext();
                    entityPM.PackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == entityPM.Id).FirstOrDefault().PackageCode;
                    entityPM.TemporalPackageCode = globalObjectContext.TenantManagements.Where(d => d.Id == entityPM.Id).FirstOrDefault().TemporalPackageCode;
                    entityPM.TemporalStartDate = globalObjectContext.TenantManagements.Where(d => d.Id == entityPM.Id).FirstOrDefault().TemporalStartDate;
                    entityPM.TemporalEndDate = globalObjectContext.TenantManagements.Where(d => d.Id == entityPM.Id).FirstOrDefault().TemporalEndDate;
                }

                if (entityPM.CurrencyId != null)
                {
                    Currency cur = CurrencyRepository.GetSingleCurrency(entityPM.CurrencyId, entityPM.Id, true);
                    if (cur != null)
                    {
                        entityPM.CurrencyCode = cur.Code;
                        entityPM.CurrencySign = cur.Sign;
                    }
                }

                if (entityPM.AddressId != null)
                {
                    AddressQuery addressQuery = new AddressQuery(entityPM.Id);
                    AddressPM add = addressQuery.GetSingleAddressPM(entityPM.AddressId, entityPM.Id);
                    if (add != null)
                    {
                        entityPM.CountryCode = add.CountryCode;
                        entityPM.CountryName = add.CountryEnglishName;
                    }
                }
            }

            return entityPM;
        }

        public IQueryable<TenantList> GetIQueryableEntityList(IQueryable<Tenant> iQueryable)
        {
            IQueryable<TenantList> result = from a in iQueryable.Include("Address").Include("PaymentTerm").Include("OtherChargesCurrency").Include("QuoteSaleCurrency").Include("AgentCard").Include("Currency").Include("ProfitCurrency").Include("FreightCurrency").Include("PasswordPolicy").Include("Address.Country")
                                            select new TenantList()
                                            {
                                                Id = a.Id,
                                                AddressId = a.AddressId,
                                                CompanyAddress = a.Address != null ? a.Address.Name : null,
                                                Company = a.Company,
                                                Signature = a.Signature,
                                                IATA = a.IATA,
                                                VatNumber = a.VatNumber,
                                                SearchFields = a.SearchFields,
                                                PaymentTermId = a.PaymentTermId,
                                                PaymentTermName = a.PaymentTerm != null ? a.PaymentTerm.EnglishName : null,
                                                AgentId = a.AgentId,
                                                AgentName = a.AgentCard != null ? a.AgentCard.EnglishName : null,
                                                CurrencyId = a.CurrencyId,
                                                AccountingCurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                ProfitCurrencyId = a.ProfitCurrencyId,
                                                ProfitCurrencyCode = a.ProfitCurrency != null ? a.ProfitCurrency.Code : null,
                                                FreightCurrencyId = a.FreightCurrencyId,
                                                FreightCurrencyCode = a.FreightCurrency != null ? a.FreightCurrency.Code : null,
                                                OtherChargesCurrencyId = a.OtherChargesCurrencyId,
                                                OtherChargesCurrencyCode = a.OtherChargesCurrency != null ? a.OtherChargesCurrency.Code : null,
                                                QuoteSaleCurrencyId = a.QuoteSaleCurrencyId,
                                                QuoteSaleCurrencyCode = a.QuoteSaleCurrency != null ? a.QuoteSaleCurrency.Code : null,
                                                VolumeUnitCode = a.VolumeUnitCode,
                                                DimensionsUnitCode = a.DimensionsUnitCode,
                                                GrossWeightUnitCode = a.GrossWeightUnitCode,
                                                ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                                WeightMeasurementUnitCode = a.WeightMeasurementUnitCode,
                                                ExportFreightPrepaidCollectId = a.ExportFreightPrepaidCollectId,
                                                ImportFreightPrepaidCollectId = a.ImportFreightPrepaidCollectId,
                                                ExportOtherPrepaidCollectId = a.ExportOtherPrepaidCollectId,
                                                ImportOtherPrepaidCollectId = a.ImportOtherPrepaidCollectId,
                                                MasterExportFreightPrepaidCollectId = a.MasterExportFreightPrepaidCollectId,
                                                MasterImportFreightPrepaidCollectId = a.MasterImportFreightPrepaidCollectId,
                                                MasterExportOtherPrepaidCollectId = a.MasterExportOtherPrepaidCollectId,
                                                MasterImportOtherPrepaidCollectId = a.MasterImportOtherPrepaidCollectId,
                                                TimeZoneOffset = a.TimeZoneOffset,
                                                Language = a.Language,
                                                Email = a.Email,
                                                Website = a.Website,
                                                Format = a.Format,
                                                Direction = a.Direction,
                                                DayLightOffset = a.DayLightOffset,
                                                DayLightStartDate = a.DayLightStartDate,
                                                DayLightEndDate = a.DayLightEndDate,
                                                PasswordPolicyCode = a.PasswordPolicyCode,
                                                PasswordStrength = a.PasswordPolicy != null ? a.PasswordPolicy.PasswordStrength : null,
                                                IsDataBackupBuilt = a.IsDataBackupBuilt,
                                                CountryCode = a.Address != null ? (a.Address.Country != null ? a.Address.Country.Code : null) : null,
                                                CountryName = a.Address != null ? (a.Address.Country != null ? a.Address.Country.EnglishName : null) : null,
                                                InvoiceSection1 = a.InvoiceSection1,
                                                InvoiceSection2 = a.InvoiceSection2,
                                                BankDetails = a.BankDetails,
                                                IsSharedLogisticsActivated = a.IsSharedLogisticsActivated,
                                                SharedLogisticsMessageLink = a.SharedLogisticsMessageLink,
                                                CASSCode = a.CASSCode,
                                                LocalCustomsCode = a.LocalCustomsCode,
                                                IsHybrid = a.IsHybrid,
                                                VatUniqueTypeCode = a.VatUniqueTypeCode,
                                                VatMandatoryTypeCode = a.VatMandatoryTypeCode,
                                                VatUniqueCountryId = a.VatUniqueCountryId,
                                                VatMandatoryCountryId = a.VatMandatoryCountryId,
                                                VatMandatoryForPotentialCustomers = a.VatMandatoryForPotentialCustomers,
                                                DefaultQuestionnaireId = a.DefaultQuestionnaireId,
                                                IsQuoteSubjectEdited = a.IsQuoteSubjectEdited,
                                                AllowEAWBMoreThanTenPackages = a.AllowEAWBMoreThanTenPackages,
                                                IsMobileActivated = a.IsMobileActivated,
                                                RegulatedAgentNumber = a.RegulatedAgentNumber,
                                                RegulatedAgentRegimeActivated = a.RegulatedAgentRegimeActivated,
                                    
                                                CustomerId = a.CustomerId,
                                                CustomerName = a.CustomerCard != null ? a.CustomerCard.EnglishName : null,
                                                IsCustomerTenantShare = a.IsCustomerTenantShare,
                                              
                                                CustomerTenantShareExportFile = a.CustomerTenantShareExportFile,
                                                AllowAgentInCustomersLOV = a.AllowAgentInCustomersLOV,
                                                AllowCustomersInAgentsLOV = a.AllowCustomersInAgentsLOV,
                                                IsWebAccessActivated = a.IsWebAccessActivated,
                                                IsCorrespondenceRightToLeftEnabled = a.IsCorrespondenceRightToLeftEnabled,
                                                IsNotesRightToLeftEnabled = a.IsNotesRightToLeftEnabled,
                                                IsInternalTicketByDefault = a.IsInternalTicketByDefault,
                                                ProrateMasterReceivables = a.ProrateMasterReceivables,
                                                SCACCode = a.SCACCode,
                                                ExportQuotationsToIntegratedSystem = a.ExportQuotationsToIntegratedSystem,
                                                //DropBoxAccessToken = a.DropBoxAccessToken
                                                TenantVATManagement = a.TenantVATManagement,
                                                TemperatureUnitCode = a.TemperatureUnitCode,
                                                DefaultSLAId = a.DefaultSLAId,
                                                NumberFormatCode = a.NumberFormatCode,
                                                CAAT = a.CAAT,
                                                CBSA = a.CBSA,
                                                IsTestTenant = a.IsTestTenant,
                                                CheckDigitControlAlgorithmCode = a.CheckDigitControlAlgorithmCode,
                                                HideFCLAllIn = a.HideFCLAllIn,
                                            };
            return result;
        }

        public TenantPM GetSinglePMByCustomerId(int CustomerId)
        {
            TenantPM entity = (from a in repository.context.Tenants.Include("CustomerCard.Contact").Include("CustomerCard")
                               where a.Id == CustomerId
                               select new TenantPM()
                               {
                                   Id = a.Id,
                                   Company = a.Company,
                                   Email = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.Email : "",
                                   VatNumber = a.VatNumber,//CustomerCard.VatNumber,
                                   CustomerId = a.CustomerId,
                                   CustomerName = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.EnglishName : null,
                                   CustomerMobile = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.Mobile : null,
                                   CustomerPhone = a.CustomerCard != null && a.CustomerCard.PrimaryContact != null ? a.CustomerCard.PrimaryContact.BusinessPhone : "",
                                 
                               }).FirstOrDefault();
            return entity;
        }

        public List<TenantList> GetTenantLists()
        {
            List<TenantList> tenantLists = (from a in repository.context.Tenants
                                            select new TenantList()
                                            {
                                                Id = a.Id,

                                            }).ToList();


            return tenantLists;

        }

        public IQueryable<TenantList> GetAllTenantLists()
        {
            IQueryable<TenantList> tenantLists = (from a in repository.context.Tenants
                                                  select new TenantList()
                                                  {
                                                      Id = a.Id,
                                                      StorageEncryptionKey = a.StorageEncryptionKey,

                                                  });


            return tenantLists;

        }
        public string GetCompanyNameById(int id)
        {

            return (from a in repository.context.Tenants.Where(d => d.Id == id) select a.Company).FirstOrDefault();

        }


        public List<UsersByTenantItem> GetUsersByTenantLists(string distributorCode, string packageCode, string addOnPackageCode, bool includeInactiveTenants, bool includeInactiveUsers, int tenant)
        {

            List<UsersByTenantItem> result = new List<UsersByTenantItem>();

            #region Tenant
            List<TenantList> tenants = (from a in repository.context.Tenants
                                        select new TenantList()
                                        {
                                            Id = a.Id,
                                            CustomerName = a.Company,
                                            VatNumber = a.VatNumber,
                                        }).ToList();
            #endregion

            #region Tenant Management  &  Package & TenantAddOnPM
            PackageRepository packageRepository = new PackageRepository(tenant);
            List<Package> packageLists = packageRepository.GetPackages().ToList();
            List<TenantManagement> tenantmanagements = new List<TenantManagement>();
            List<TenantManagementLicensePM> tenantManagementLicensePMLists = new List<TenantManagementLicensePM>();
            List<TenantAddOnPM> tenantAddOnPMPMLists = new List<TenantAddOnPM>();
            List<int> tenantIds = tenants.GroupBy(d => d.Id).Select(d => d.First().Id).ToList();


            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {

                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                IQueryable<TenantManagement> tenantmanagementList = tenantManagementRepository.GetTenantManagementsByIds(tenantIds);
                if (!string.IsNullOrEmpty(distributorCode)) tenantmanagementList = tenantmanagementList.Where(d => d.DistributorCode == distributorCode);
                if (!includeInactiveUsers) tenantmanagementList = tenantmanagementList.Where(d => d.GlobalTenant != null && d.GlobalTenant.IsActive);
                if (!string.IsNullOrEmpty(packageCode)) tenantmanagementList = tenantmanagementList.Where(d => d.PackageCode == packageCode || d.IsMultiPackage);
                tenantmanagements = tenantmanagementList.ToList();

                List<int> tenantManagementIds = tenantmanagements.GroupBy(d => d.Id).Select(d => d.First().Id).ToList();
                List<int> tenantManagementMultiPackageIds = tenantmanagements.Where(d => d.IsMultiPackage).GroupBy(d => d.Id).Select(d => d.First().Id).ToList();
                if (tenantManagementIds.Count > 0)
                {
                    TenantAddOnQuery tenantAddOnQuery = new TenantAddOnQuery(tenant);
                    tenantAddOnPMPMLists = tenantAddOnQuery.GetTenantAddOnPMByListTenantids(tenantManagementIds).ToList();
                }

                if (tenantManagementMultiPackageIds.Count > 0)
                {
                    TenantManagementLicenseQuery tenantManagementLicenseQuery = new TenantManagementLicenseQuery(tenant);
                    tenantManagementLicensePMLists = tenantManagementLicenseQuery.GetTenantManagementLicenseByListids(tenantManagementMultiPackageIds).ToList();
                }

            }

            #endregion

            List<TenantData> tenantDataLists = new List<TenantData>();

            foreach (TenantManagement tenantManagement in tenantmanagements)
            {
                TenantData tenantData = new TenantData();
                TenantList tenantList = tenants.Where(d => d.Id == tenantManagement.Id).FirstOrDefault();
                if (tenantList != null) tenantData.VATNumber = tenantList.VatNumber;

                tenantData.TenantName = tenantManagement.Name;
                tenantData.Tenant = tenantManagement.Id;
                tenantData.DistributorCode = tenantManagement.DistributorCode;
                tenantData.PackageCodeLists = new List<string>();
                tenantData.AddInsPackageCodeLists = new List<string>();
                if (tenantManagement.GlobalTenant != null) tenantData.ActiveTenant = tenantManagement.GlobalTenant != null && tenantManagement.GlobalTenant.IsActive ? "Active" : "Not Active";



                if (tenantManagement.IsMultiPackage)
                {
                    List<TenantManagementLicensePM> multiPackages = tenantManagementLicensePMLists.Where(d => d.Tenant == tenantManagement.Id).ToList();
                    if (multiPackages.Count > 0)
                    {
                        foreach (TenantManagementLicensePM multiPackage in multiPackages)
                        {
                            Package package = packageLists.Where(d => d.Code == multiPackage.PackageCode).FirstOrDefault();
                            if (package != null)
                            {
                                tenantData.PackageName += (!string.IsNullOrEmpty(tenantData.PackageName) ? (" , " + package.Name) : package.Name);
                                tenantData.PackageCodeLists.Add(package.Code);
                            }

                        }
                    }
                }

                else if (!string.IsNullOrEmpty(tenantManagement.PackageCode))
                {
                    Package package = packageLists.Where(d => d.Code == tenantManagement.PackageCode).FirstOrDefault();
                    if (package != null)
                    {
                        tenantData.PackageName = package.Name;
                        tenantData.PackageCodeLists.Add(package.Code);


                    }
                }


                List<TenantAddOnPM> tenantAddOnPMPackages = tenantAddOnPMPMLists.Where(d => d.Tenant == tenantManagement.Id).ToList();
                if (tenantAddOnPMPackages.Count > 0)
                {
                    foreach (TenantAddOnPM tenantAddOnPMPackage in tenantAddOnPMPackages)
                    {
                        Package package = packageLists.Where(d => d.Code == tenantAddOnPMPackage.PackageCode).FirstOrDefault();
                        if (package != null)
                        {
                            tenantData.AddIns += (!string.IsNullOrEmpty(tenantData.AddIns) ? (" , " + package.Name) : package.Name);
                            tenantData.AddInsPackageCodeLists.Add(package.Code);
                        }
                    }
                }

                tenantDataLists.Add(tenantData);
            }

            if (!string.IsNullOrEmpty(packageCode)) tenantDataLists = tenantDataLists.Where(d => d.PackageCodeLists.Contains(packageCode)).ToList();
            if (!string.IsNullOrEmpty(addOnPackageCode)) tenantDataLists = tenantDataLists.Where(d => d.AddInsPackageCodeLists.Contains(addOnPackageCode)).ToList();

            tenantIds = tenantDataLists.GroupBy(d => d.Tenant).Select(d => d.First().Tenant).ToList();

            ContactQuery contactQuery = new ContactQuery(tenant);

            List<ContactList> contactLists = contactQuery.GetContactUserListsByTenants(tenantIds, includeInactiveUsers);

            foreach (TenantData tenantitem in tenantDataLists)
            {
                List<ContactList> contacts = contactLists.Where(d => d.Tenant == tenantitem.Tenant).ToList();

                foreach (ContactList contactList in contacts)
                {
                    UsersByTenantItem item = new UsersByTenantItem();
                    item.Tenant = tenantitem.Tenant;
                    item.TenantName = tenantitem.TenantName;
                    item.VATNumber = tenantitem.VATNumber;
                    item.Package = tenantitem.PackageName;
                    item.AddIns = tenantitem.AddIns;
                    item.ActiveTenant = tenantitem.ActiveTenant;
                    item.TotalActiveUsers = contacts.Where(d => !d.InActive).Count();
                    item.UserEmail = contactList.Email;
                    item.UserName = contactList.EnglishName;
                    item.ActiveUser = !contactList.InActive ? "Active" : "Not Active";
                    item.LastLoginDate = contactList.LastLoginDate;

                    result.Add(item);
                }
            }

            return result;


        }

        public string GetTenantVatNumber(int tenant)
        {
            string cacheKey = "TenantVatNumber" + tenant;
            string vatNumber;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(cacheKey) == null)
                {
                    TenantRepository tenantRepository = new TenantRepository(tenant);
                    vatNumber = tenantRepository.GetTenantVatNumberOnly(tenant);
                    CacheManager.CacheWrapper.Insert(cacheKey, vatNumber, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    vatNumber = (string)CacheManager.CacheWrapper.Get(cacheKey);
                }
            }
            else
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                vatNumber = tenantRepository.GetTenantVatNumberOnly(tenant);
            }
            return vatNumber;
        }

    }
}