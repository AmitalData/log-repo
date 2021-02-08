using System;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityLists;
using System.Collections.Generic;
using System.Transactions;
using Logitude.BL.GlobalModel.EntityDws;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Text.RegularExpressions;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class TenantManagementQuery
    {
        private TenantManagementRepository repository;
        public TenantManagementQuery()
        {
            repository = new TenantManagementRepository();
        }
        public TenantManagementQuery(int tenant)
        {
            repository = new TenantManagementRepository();
        }
        public TenantManagementQuery(TenantManagementRepository repository)
        {
            this.repository = repository;
        }
        public TenantManagementPM GetSinglePMByDomain(string domain)
        {

            domain = TrimDomainByRegex(domain);
            TenantManagementPM TenantManagement = (from a in repository.context.TenantManagements
                                                   where a.EnableBranding && a.CustomerURL == domain && a.Id !=0 && a.GlobalTenant.IsActive
                                                   select new TenantManagementPM()
                                                   {
                                                       Id = a.Id,
                                                       MainColor = a.MainColor, 
                                                       SecondaryColor = a.SecondaryColor, 
                                                       BackgroundId = a.BackgroundId,
                                                       ShipmentHeaderImageId = a.ShipmentHeaderImageId,
                                                       ComapnylogoId = a.ComapnylogoId,
                                                       BrowserIconId = a.BrowserIconId,
                                                       CustomerURL = a.CustomerURL,

                                                   }).FirstOrDefault();


            return TenantManagement;
        }

        public int GetTenantSinglePMByDomain(string domain)
        {

            domain = TrimDomainByRegex(domain);
            int Tenant = (from a in repository.context.TenantManagements
                                                   where a.CustomerURL == domain  && a.Id !=0 && a.GlobalTenant.IsActive
                                                   select a.Id
                                                  ).FirstOrDefault();

            return Tenant;
        }

        public bool CheckIsdomainAlreadyExist(TenantManagementPM tenantManagement)
        {

            string domain = TrimDomainByRegex(tenantManagement.CustomerURL);
            bool IsExist = (from a in repository.context.TenantManagements
                          where a.CustomerURL == domain  && a.Id!= tenantManagement.Id
                            select a.Id).Any();

            return IsExist;
        }


        private string TrimDomainByRegex(string domain)
        {
            domain = domain.EndsWith("/") ? domain.Substring(0, domain.Length - 1) : domain;
            domain = Regex.Replace(domain, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase);

            return domain;
        }
        public TenantManagementPM GetSinglePM(int id)
        {
            string entityName = "TenantManagementPM" + id;

            TenantManagementPM entity = null;

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    TenantManagementPM tenant = (from a in repository.context.TenantManagements.Include("GlobalTenant").Include("BluesnapContract")
                                                 where a.Id == id
                                                 select new TenantManagementPM()
                                                 {
                                                     Id = a.Id,
                                                     Name = a.Name,
                                                     PackageCode = a.PackageCode,
                                                     PackageName = a.PackageName,
                                                     IsTrial = a.IsTrial,
                                                     TrialStartDate = a.TrialStartDate,
                                                     TrialEndDate = a.TrialEndDate,
                                                     FirstPaymentDate = a.FirstPaymentDate,
                                                     PaidUntilDate = a.PaidUntilDate,
                                                     NumberOfUsers = a.NumberOfUsers,
                                                     SearchFields = a.SearchFields,
                                                     TTY = a.TTY,
                                                     FreeUsers = a.FreeUsers,
                                                     IsRecurring = a.IsRecurring,
                                                     RecurringPeriodCode = a.RecurringPeriodCode,
                                                     CreateDate = a.CreateDate,
                                                     UpdateDate = a.UpdateDate,
                                                     PaymentFailure = a.PaymentFailure,
                                                     SuspendDate = a.SuspendDate,
                                                     InternalNotes = a.InternalNotes,
                                                     BluesnapAccount = a.BluesnapAccount,
                                                     MainContract = a.MainContract,
                                                     TemporalPackageCode = a.TemporalPackageCode,
                                                     TemporalStartDate = a.TemporalStartDate,
                                                     TemporalEndDate = a.TemporalEndDate,
                                                     LicensePrice = a.LicensePrice,
                                                     PaymentChannelCode = a.PaymentChannelCode,
                                                     PaymentMethodCode = a.PaymentMethodCode,
                                                     Notes = a.Notes,
                                                     APInvoiceLastDate = a.APInvoiceLastDate,
                                                     APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                                                     APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                                                     ARInvoiceLastDate = a.ARInvoiceLastDate,
                                                     ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                                                     ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                                                     CustomerLastDate = a.CustomerLastDate,
                                                     CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                                                     CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                                                     LastFHLSentDate = a.LastFHLSentDate,
                                                     LastFWBSentDate = a.LastFWBSentDate,
                                                     QuoteLastDate = a.QuoteLastDate,
                                                     QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                                                     QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                                                     ShipmentLastDate = a.ShipmentLastDate,
                                                     ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                                                     ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                                                     StatisticsUpdateDate = a.StatisticsUpdateDate,
                                                     CountryName = a.CountryName,
                                                     LastLoginDateTime = a.LastLoginDateTime,
                                                     PaymentCurrencyCode = a.PaymentCurrencyCode,
                                                     IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                                                     ManageLicencesPerUser = a.ManageLicencesPerUser,
                                                     DistributorCode = a.DistributorCode,
                                                     IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                                                     IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                                                     IsCargonautEnabled = a.IsCargonautEnabled,
                                                     LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                                                     LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                                                     IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                                                     BluesnapContractId = a.BluesnapContractId,
                                                     BluesnapCRMContractId = a.BluesnapCRMContractId,
                                                     BluesnapEAWBContractId = a.BluesnapEAWBContractId,
                                                     BluesnapEAWBSContractId = a.BluesnapEAWBSContractId,
                                                     BluesnapOneTimeContract = a.BluesnapOneTimeContract,
                                                     BluesnapContractQTY = a.BluesnapContractQTY,
                                                     BluesnapCRMContractQTY = a.BluesnapCRMContractQTY,
                                                     BluesnapEAWBContractQTY = a.BluesnapEAWBContractQTY,
                                                     BluesnapEAWBSContractQTY = a.BluesnapEAWBSContractQTY,
                                                     BluesnapOneTimeContractQTY = a.BluesnapOneTimeContractQTY,
                                                     BluesnapInttraStockContractQTY = a.BluesnapInttraStockContractQTY,
                                                     BluesnapInttraStockContractId = a.BluesnapInttraStockContractId,
                                                     //BluesnapContractId = a.BluesnapContract == null ? null : a.BluesnapContract.ContractId,
                                                     AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                                                     PIMA = a.PIMA,
                                                     IsEAWBOnlyDemo = a.IsEAWBOnlyDemo,
                                                     IsRestrictedByAirline = a.IsRestrictedByAirline,
                                                     LastFFRSentDate = a.LastFFRSentDate,
                                                     ManagesRegisteredAgent = a.ManagesRegisteredAgent,
                                                     ActivityLastDate = a.ActivityLastDate,
                                                     ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                                                     ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                                                     OpportunityLastDate = a.OpportunityLastDate,
                                                     OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                                                     OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                                                     FSULastReceivedDate = a.FSULastReceivedDate,
                                                     FSALastReceivedDate = a.FSALastReceivedDate,
                                                     FSRLastSentDate = a.FSRLastSentDate,
                                                     BillingByLogitude = a.BillingByLogitude,
                                                     ResellerCommission = a.ResellerCommission,
                                                     TenantTypeCode = a.TenantTypeCode,
                                                     TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                                                     SignupRequestRecipients = a.SignupRequestRecipients,
                                                     LoginPageNotes = a.LoginPageNotes,
                                                     SupportActivated = a.SupportActivated,
                                                     SupportEmail = a.SupportEmail,
                                                     IsMultiPackage = a.IsMultiPackage,
                                                     Technology = a.Technology,
                                                     MobileLastDate = a.MobileLastDate,
                                                     MobileTotalLastWeek = a.MobileTotalLastWeek,
                                                     MobileTotalLastMonth = a.MobileTotalLastMonth,
                                                     ShardLogisticLastDate = a.ShardLogisticLastDate,
                                                     ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                                                     ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                                                     RequestedAirlines = a.RequestedAirlines,
                                                     RegisteredAirlines = a.RegisteredAirlines,
                                                     PendingAirlines = a.PendingAirlines,
                                                     EnableBranding = a.EnableBranding,
                                                     ContactEmail = a.ContactEmail,
                                                     CustomerURL = a.CustomerURL,
                                                     HideSharedlogistics = a.HideSharedlogistics,
                                                     PrivateLabelId = a.GlobalTenant != null ? a.GlobalTenant.PrivateLabelId : "",
                                                     SilverlightEndDate = a.SilverlightEndDate,
                                                     IsParentTenant = a.IsParentTenant,
                                                     ParentTenantId = a.ParentTenantId,
                                                     AgentSharedLogisticsStatisticsLastDate = a.AgentSharedLogisticsStatisticsLastDate,
                                                     AgentSharedLogisticsStatisticsLastWeek = a.AgentSharedLogisticsStatisticsLastWeek,
                                                     AgentSharedLogisticsStatisticsLastMonth = a.AgentSharedLogisticsStatisticsLastMonth,
                                                     ChangeHeaderColor = a.ChangeHeaderColor,
                                                     StockTypeCode = a.StockTypeCode,
                                                     IsINTTRAStockPrepaid = a.IsINTTRAStockPrepaid,
                                                     PackageCodeSearchField = a.PackageCodeSearchField,
                                                     IsINTTRAOnlyDemo = a.IsINTTRAOnlyDemo,
                                                     MainAdditionalPackageApplied = a.MainAdditionalPackageApplied,
                                                     TotalPrice = a.TotalPrice,
                                                     SupportDomain = a.SupportDomain,
                                                     TotalNumberOfUsers = a.TotalNumberOfUsers,
                                                     TotalFreeUsers = a.TotalFreeUsers,
                                                     AveragePrice = a.AveragePrice,
                                                     TotalPaymentamount = a.TotalPaymentamount,
                                                     MainColor = a.MainColor,// != null && a.MainColor.Length > 7) ? "#" + a.MainColor.Substring(3, 6) : null,
                                                     SecondaryColor = a.SecondaryColor,// != null && a.SecondaryColor.Length > 7) ? "#" + a.SecondaryColor.Substring(3, 6) : null,

                                                     BackgroundId = a.BackgroundId,
                                                     ShipmentHeaderImageId = a.ShipmentHeaderImageId,
                                                     ComapnylogoId = a.ComapnylogoId,
                                                     BrowserIconId = a.BrowserIconId,
                                                     NoPaymentForChildTenants = a.NoPaymentForChildTenants,
                                                 }).FirstOrDefault();
                    if (tenant != null)
                    {
                        TenantAddOnQuery tenantAddOnQuery = new TenantAddOnQuery(tenant.Id);
                        tenant.AddOns = tenantAddOnQuery.GetTenantAddOnPMs(tenant.Id).ToList();

                        this.GetPackagesData(tenant);

                        TenantRepository tenantRep = new TenantRepository(tenant.Id);
                        Tenant ten = tenantRep.GetSingleTenant(tenant.Id);
                        LogBoxTenantSettingRepository tenantsettingRep = new LogBoxTenantSettingRepository(tenant.Id);
                        LogBoxTenantSetting tens = tenantsettingRep.GetSingleLBTenant(tenant.Id);
                        if (ten != null && tens != null)
                        {
                            //tenant.CountryName = ten.Address != null ? (ten.Address.Country != null ? ten.Address.Country.EnglishName : null) : null;
                            tenant.TimeZone = "(UTC) + " + ten.TimeZoneOffset;
                            tenant.DocumentShareAsDefault = tens.DocumentShareAsDefault;
                            tenant.AutoArchiveOnInvoice = tens.AutoArchiveOnInvoice;
                            tenant.IsTestTenant = ten.IsTestTenant;
                        }

                        GlobalTenantRepository globalTenRep = new GlobalTenantRepository();
                        GlobalTenant globalten = globalTenRep.GetGlobalTenantsByTenant(tenant.Id);
                        if (globalten != null)
                        {
                            tenant.GlobalDBId = globalten.GlobalDBId;
                            tenant.IsActive = globalten.IsActive;
                        }

                        //daysLeft
                        tenant.TrailDaysLeft = ComputeDaysLeft(tenant.TrialEndDate);
                        tenant.PaidDaysLeft = ComputeDaysLeft(tenant.PaidUntilDate);
                        tenant.SuspendDaysLeft = ComputeDaysLeft(tenant.SuspendDate);

                        entity = tenant;
                    }
                }

                else
                {
                    entity = (TenantManagementPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                TenantManagementPM tenant1 = (from a in repository.context.TenantManagements.Include("GlobalTenant").Include("BluesnapContract")
                                              where a.Id == id
                                              select new TenantManagementPM()
                                              {
                                                  Id = a.Id,
                                                  Name = a.Name,
                                                  PackageCode = a.PackageCode,
                                                  PackageName = a.PackageName,
                                                  IsTrial = a.IsTrial,
                                                  TrialStartDate = a.TrialStartDate,
                                                  TrialEndDate = a.TrialEndDate,
                                                  FirstPaymentDate = a.FirstPaymentDate,
                                                  PaidUntilDate = a.PaidUntilDate,
                                                  NumberOfUsers = a.NumberOfUsers,
                                                  SearchFields = a.SearchFields,
                                                  TTY = a.TTY,
                                                  FreeUsers = a.FreeUsers,
                                                  IsRecurring = a.IsRecurring,
                                                  RecurringPeriodCode = a.RecurringPeriodCode,
                                                  CreateDate = a.CreateDate,
                                                  UpdateDate = a.UpdateDate,
                                                  PaymentFailure = a.PaymentFailure,
                                                  SuspendDate = a.SuspendDate,
                                                  InternalNotes = a.InternalNotes,
                                                  BluesnapAccount = a.BluesnapAccount,
                                                  MainContract = a.MainContract,
                                                  TemporalPackageCode = a.TemporalPackageCode,
                                                  TemporalStartDate = a.TemporalStartDate,
                                                  TemporalEndDate = a.TemporalEndDate,
                                                  LicensePrice = a.LicensePrice,
                                                  PaymentChannelCode = a.PaymentChannelCode,
                                                  PaymentMethodCode = a.PaymentMethodCode,
                                                  Notes = a.Notes,
                                                  APInvoiceLastDate = a.APInvoiceLastDate,
                                                  APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                                                  APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                                                  ARInvoiceLastDate = a.ARInvoiceLastDate,
                                                  ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                                                  ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                                                  CustomerLastDate = a.CustomerLastDate,
                                                  CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                                                  CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                                                  LastFHLSentDate = a.LastFHLSentDate,
                                                  LastFWBSentDate = a.LastFWBSentDate,
                                                  QuoteLastDate = a.QuoteLastDate,
                                                  QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                                                  QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                                                  ShipmentLastDate = a.ShipmentLastDate,
                                                  ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                                                  ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                                                  StatisticsUpdateDate = a.StatisticsUpdateDate,
                                                  CountryName = a.CountryName,
                                                  LastLoginDateTime = a.LastLoginDateTime,
                                                  PaymentCurrencyCode = a.PaymentCurrencyCode,
                                                  IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                                                  DistributorCode = a.DistributorCode,
                                                  IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                                                  IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                                                  IsCargonautEnabled = a.IsCargonautEnabled,
                                                  LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                                                  LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                                                  IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                                                  BluesnapContractId = a.BluesnapContractId,
                                                  BluesnapCRMContractId = a.BluesnapCRMContractId,
                                                  BluesnapEAWBContractId = a.BluesnapEAWBContractId,
                                                  BluesnapEAWBSContractId = a.BluesnapEAWBSContractId,
                                                  BluesnapOneTimeContract = a.BluesnapOneTimeContract,
                                                  BluesnapContractQTY = a.BluesnapContractQTY,
                                                  BluesnapCRMContractQTY = a.BluesnapCRMContractQTY,
                                                  BluesnapEAWBContractQTY = a.BluesnapEAWBContractQTY,
                                                  BluesnapEAWBSContractQTY = a.BluesnapEAWBSContractQTY,
                                                  BluesnapOneTimeContractQTY = a.BluesnapOneTimeContractQTY,
                                                  BluesnapInttraStockContractQTY = a.BluesnapInttraStockContractQTY,
                                                  BluesnapInttraStockContractId = a.BluesnapInttraStockContractId,
                                                  //     BluesnapContractId = a.BluesnapContract == null ? null : a.BluesnapContract.ContractId,
                                                  AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                                                  PIMA = a.PIMA,
                                                  IsEAWBOnlyDemo = a.IsEAWBOnlyDemo,
                                                  IsRestrictedByAirline = a.IsRestrictedByAirline,
                                                  LastFFRSentDate = a.LastFFRSentDate,
                                                  ManagesRegisteredAgent = a.ManagesRegisteredAgent,
                                                  ActivityLastDate = a.ActivityLastDate,
                                                  ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                                                  ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                                                  OpportunityLastDate = a.OpportunityLastDate,
                                                  OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                                                  OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                                                  FSULastReceivedDate = a.FSULastReceivedDate,
                                                  FSALastReceivedDate = a.FSALastReceivedDate,
                                                  FSRLastSentDate = a.FSRLastSentDate,
                                                  BillingByLogitude = a.BillingByLogitude,
                                                  ResellerCommission = a.ResellerCommission,
                                                  TenantTypeCode = a.TenantTypeCode,
                                                  TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                                                  SignupRequestRecipients = a.SignupRequestRecipients,
                                                  LoginPageNotes = a.LoginPageNotes,
                                                  SupportActivated = a.SupportActivated,
                                                  SupportEmail = a.SupportEmail,
                                                  IsMultiPackage = a.IsMultiPackage,
                                                  Technology = a.Technology,
                                                  MobileLastDate = a.MobileLastDate,
                                                  MobileTotalLastWeek = a.MobileTotalLastWeek,
                                                  MobileTotalLastMonth = a.MobileTotalLastMonth,
                                                  ShardLogisticLastDate = a.ShardLogisticLastDate,
                                                  ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                                                  ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                                                  RequestedAirlines = a.RequestedAirlines,
                                                  RegisteredAirlines = a.RegisteredAirlines,
                                                  PendingAirlines = a.PendingAirlines,
                                                  EnableBranding = a.EnableBranding,
                                                  ContactEmail = a.ContactEmail,
                                                  CustomerURL = a.CustomerURL,
                                                  HideSharedlogistics = a.HideSharedlogistics,
                                                  PrivateLabelId = a.GlobalTenant != null ? a.GlobalTenant.PrivateLabelId : "",
                                                  SilverlightEndDate = a.SilverlightEndDate,
                                                  IsParentTenant = a.IsParentTenant,
                                                  ParentTenantId = a.ParentTenantId,
                                                  AgentSharedLogisticsStatisticsLastDate = a.AgentSharedLogisticsStatisticsLastDate,
                                                  AgentSharedLogisticsStatisticsLastWeek = a.AgentSharedLogisticsStatisticsLastWeek,
                                                  AgentSharedLogisticsStatisticsLastMonth = a.AgentSharedLogisticsStatisticsLastMonth,
                                                  ChangeHeaderColor = a.ChangeHeaderColor,
                                                  StockTypeCode = a.StockTypeCode,
                                                  PackageCodeSearchField = a.PackageCodeSearchField,
                                                  IsINTTRAStockPrepaid = a.IsINTTRAStockPrepaid,
                                                  IsINTTRAOnlyDemo = a.IsINTTRAOnlyDemo,
                                                  MainAdditionalPackageApplied = a.MainAdditionalPackageApplied,
                                                  TotalPrice = a.TotalPrice,
                                                  SupportDomain = a.SupportDomain,
                                                  TotalNumberOfUsers = a.TotalNumberOfUsers,
                                                  TotalFreeUsers = a.TotalFreeUsers,
                                                  AveragePrice = a.AveragePrice,
                                                  TotalPaymentamount = a.TotalPaymentamount,
                                                  MainColor = a.MainColor ,//!= null && a.MainColor.Length > 7) ? "#" + a.MainColor.Substring(3, 6) : null,
                                                  SecondaryColor = a.SecondaryColor,// != null && a.SecondaryColor.Length > 7) ? "#" + a.SecondaryColor.Substring(3, 6) : null,

                                                  BackgroundId = a.BackgroundId,
                                                  ComapnylogoId = a.ComapnylogoId,
                                                  BrowserIconId = a.BrowserIconId,
                                                  ShipmentHeaderImageId = a.ShipmentHeaderImageId,
                                                  NoPaymentForChildTenants = a.NoPaymentForChildTenants,
                                              }).FirstOrDefault();

                if (tenant1 != null)
                {
                    TenantAddOnQuery tenantAddOnQuery = new TenantAddOnQuery(tenant1.Id);
                    tenant1.AddOns = tenantAddOnQuery.GetTenantAddOnPMs(tenant1.Id).ToList();

                    this.GetPackagesData(tenant1);

                    TenantRepository tenantRep = new TenantRepository(tenant1.Id);
                    Tenant ten = tenantRep.GetSingleTenant(tenant1.Id);

                    LogBoxTenantSettingRepository tenantsettingRep = new LogBoxTenantSettingRepository(tenant1.Id);
                    LogBoxTenantSetting tens = tenantsettingRep.GetSingleLBTenant(tenant1.Id);
                    if (ten != null)
                    {
                        //tenant1.CountryName = ten.Address.Country.EnglishName;
                        tenant1.TimeZone = "(UTC) + " + ten.TimeZoneOffset;
                        tenant1.DocumentShareAsDefault = tens.DocumentShareAsDefault;
                        tenant1.AutoArchiveOnInvoice = tens.AutoArchiveOnInvoice;
                        tenant1.IsTestTenant = ten.IsTestTenant;
                    }

                    GlobalTenantRepository globalTenRep = new GlobalTenantRepository();
                    GlobalTenant globalten = globalTenRep.GetGlobalTenantsByTenant(tenant1.Id);
                    if (globalten != null)
                    {
                        tenant1.GlobalDBId = globalten.GlobalDBId;
                        tenant1.IsActive = globalten.IsActive;
                    }

                    //daysLeft
                    tenant1.TrailDaysLeft = ComputeDaysLeft(tenant1.TrialEndDate);
                    tenant1.PaidDaysLeft = ComputeDaysLeft(tenant1.PaidUntilDate);
                    tenant1.SuspendDaysLeft = ComputeDaysLeft(tenant1.SuspendDate);

                    entity = tenant1;
                }
            }

            return entity;
        }
        public IQueryable<TenantManagementPM> GetTenantManagementPMs()
        {
            return (from a in repository.context.TenantManagements.Include("GlobalTenant").Include("BluesnapContract")
                    select new TenantManagementPM()
                    {
                        Id = a.Id,
                        Name = a.Name,
                        PackageCode = a.PackageCode,
                        PackageName = a.PackageName,
                        IsTrial = a.IsTrial,
                        TrialStartDate = a.TrialStartDate,
                        TrialEndDate = a.TrialEndDate,
                        FirstPaymentDate = a.FirstPaymentDate,
                        PaidUntilDate = a.PaidUntilDate,
                        NumberOfUsers = a.NumberOfUsers,
                        SearchFields = a.SearchFields,
                        IsActive = a.GlobalTenant.IsActive,
                        TTY = a.TTY,
                        FreeUsers = a.FreeUsers,
                        IsRecurring = a.IsRecurring,
                        RecurringPeriodCode = a.RecurringPeriodCode,
                        CreateDate = a.CreateDate,
                        UpdateDate = a.UpdateDate,
                        PaymentFailure = a.PaymentFailure,
                        SuspendDate = a.SuspendDate,
                        InternalNotes = a.InternalNotes,
                        BluesnapAccount = a.BluesnapAccount,
                        MainContract = a.MainContract,
                        TemporalPackageCode = a.TemporalPackageCode,
                        TemporalStartDate = a.TemporalStartDate,
                        TemporalEndDate = a.TemporalEndDate,
                        LicensePrice = a.LicensePrice,
                        PaymentChannelCode = a.PaymentChannelCode,
                        PaymentMethodCode = a.PaymentMethodCode,
                        Notes = a.Notes,
                        APInvoiceLastDate = a.APInvoiceLastDate,
                        APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                        APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                        ARInvoiceLastDate = a.ARInvoiceLastDate,
                        ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                        ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                        CustomerLastDate = a.CustomerLastDate,
                        CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                        CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                        LastFHLSentDate = a.LastFHLSentDate,
                        LastFWBSentDate = a.LastFWBSentDate,
                        QuoteLastDate = a.QuoteLastDate,
                        QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                        QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                        ShipmentLastDate = a.ShipmentLastDate,
                        ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                        ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                        StatisticsUpdateDate = a.StatisticsUpdateDate,
                        CountryName = a.CountryName,
                        LastLoginDateTime = a.LastLoginDateTime,
                        PaymentCurrencyCode = a.PaymentCurrencyCode,
                        IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                        ManageLicencesPerUser = a.ManageLicencesPerUser,
                        DistributorCode = a.DistributorCode,
                        IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                        IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                        IsCargonautEnabled = a.IsCargonautEnabled,
                        LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                        LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                        IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                        BluesnapContractId = a.BluesnapContractId,
                        BluesnapCRMContractId = a.BluesnapCRMContractId,
                        BluesnapEAWBContractId = a.BluesnapEAWBContractId,
                        BluesnapEAWBSContractId = a.BluesnapEAWBSContractId,
                        BluesnapOneTimeContract = a.BluesnapOneTimeContract,
                        BluesnapContractQTY = a.BluesnapContractQTY,
                        BluesnapCRMContractQTY = a.BluesnapCRMContractQTY,
                        BluesnapEAWBContractQTY = a.BluesnapEAWBContractQTY,
                        BluesnapEAWBSContractQTY = a.BluesnapEAWBSContractQTY,
                        BluesnapOneTimeContractQTY = a.BluesnapOneTimeContractQTY,
                        BluesnapInttraStockContractQTY = a.BluesnapInttraStockContractQTY,
                        BluesnapInttraStockContractId = a.BluesnapInttraStockContractId,
                        //BluesnapContractId = a.BluesnapContract == null ? null : a.BluesnapContract.ContractId,
                        AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                        PIMA = a.PIMA,
                        IsEAWBOnlyDemo = a.IsEAWBOnlyDemo,
                        IsRestrictedByAirline = a.IsRestrictedByAirline,
                        LastFFRSentDate = a.LastFFRSentDate,
                        ManagesRegisteredAgent = a.ManagesRegisteredAgent,
                        ActivityLastDate = a.ActivityLastDate,
                        ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                        ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                        OpportunityLastDate = a.OpportunityLastDate,
                        OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                        OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                        FSULastReceivedDate = a.FSULastReceivedDate,
                        FSALastReceivedDate = a.FSALastReceivedDate,
                        FSRLastSentDate = a.FSRLastSentDate,
                        BillingByLogitude = a.BillingByLogitude,
                        ResellerCommission = a.ResellerCommission,
                        TenantTypeCode = a.TenantTypeCode,
                        TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                        SignupRequestRecipients = a.SignupRequestRecipients,
                        LoginPageNotes = a.LoginPageNotes,
                        SupportActivated = a.SupportActivated,
                        SupportEmail = a.SupportEmail,
                        IsMultiPackage = a.IsMultiPackage,
                        Technology = a.Technology,
                        MobileLastDate = a.MobileLastDate,
                        MobileTotalLastWeek = a.MobileTotalLastWeek,
                        MobileTotalLastMonth = a.MobileTotalLastMonth,
                        ShardLogisticLastDate = a.ShardLogisticLastDate,
                        ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                        ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                        RequestedAirlines = a.RequestedAirlines,
                        RegisteredAirlines = a.RegisteredAirlines,
                        PendingAirlines = a.PendingAirlines,
                        EnableBranding = a.EnableBranding,
                        ContactEmail = a.ContactEmail,
                        CustomerURL = a.CustomerURL,
                        HideSharedlogistics = a.HideSharedlogistics,
                        PrivateLabelId = a.GlobalTenant != null ? a.GlobalTenant.PrivateLabelId : "",
                        SilverlightEndDate = a.SilverlightEndDate,
                        IsParentTenant = a.IsParentTenant,
                        ParentTenantId = a.ParentTenantId,
                        AgentSharedLogisticsStatisticsLastDate = a.AgentSharedLogisticsStatisticsLastDate,
                        AgentSharedLogisticsStatisticsLastWeek = a.AgentSharedLogisticsStatisticsLastWeek,
                        AgentSharedLogisticsStatisticsLastMonth = a.AgentSharedLogisticsStatisticsLastMonth,
                        PackageCodeSearchField = a.PackageCodeSearchField,
                        ChangeHeaderColor = a.ChangeHeaderColor,
                        StockTypeCode = a.StockTypeCode,
                        IsINTTRAStockPrepaid = a.IsINTTRAStockPrepaid,
                        IsINTTRAOnlyDemo = a.IsINTTRAOnlyDemo,
                        MainAdditionalPackageApplied = a.MainAdditionalPackageApplied,
                        TotalPrice = a.TotalPrice,
                        SupportDomain = a.SupportDomain,
                        TotalNumberOfUsers = a.TotalNumberOfUsers,
                        TotalFreeUsers = a.TotalFreeUsers,
                        AveragePrice = a.AveragePrice,
                        TotalPaymentamount = a.TotalPaymentamount,
                        NoPaymentForChildTenants = a.NoPaymentForChildTenants,
                    });
        }
        public TenantManagementList MapSingleList(TenantManagement entity)
        {
            TenantManagementList myResult = null;

            if (entity != null)
            {
                myResult = new TenantManagementList()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    PackageCode = entity.PackageCode,
                    PackageName = entity.PackageName,
                    IsTrial = entity.IsTrial,
                    TrialStartDate = entity.TrialStartDate,
                    TrialEndDate = entity.TrialEndDate,
                    FirstPaymentDate = entity.FirstPaymentDate,
                    PaidUntilDate = entity.PaidUntilDate,
                    NumberOfUsers = entity.NumberOfUsers,
                    SearchFields = entity.SearchFields,
                    IsActive = entity.GlobalTenant.IsActive,
                    FreeUsers = entity.FreeUsers,
                    IsRecurring = entity.IsRecurring,
                    RecurringPeriodCode = entity.RecurringPeriodCode,
                    CreateDate = entity.CreateDate,
                    UpdateDate = entity.UpdateDate,
                    PaymentFailure = entity.PaymentFailure,
                    SuspendDate = entity.SuspendDate,
                    InternalNotes = entity.InternalNotes,
                    BluesnapAccount = entity.BluesnapAccount,
                    MainContract = entity.MainContract,
                    TemporalPackageCode = entity.TemporalPackageCode,
                    TemporalStartDate = entity.TemporalStartDate,
                    TemporalEndDate = entity.TemporalEndDate,
                    LicensePrice = entity.LicensePrice,
                    PaymentChannelCode = entity.PaymentChannelCode,
                    PaymentMethodCode = entity.PaymentMethodCode,
                    Notes = entity.Notes,
                    TTY = entity.TTY,
                    APInvoiceLastDate = entity.APInvoiceLastDate,
                    APInvoiceTotalLastMonth = entity.APInvoiceTotalLastMonth,
                    APInvoiceTotalLastWeek = entity.APInvoiceTotalLastWeek,
                    ARInvoiceLastDate = entity.ARInvoiceLastDate,
                    ARInvoiceTotalLastMonth = entity.ARInvoiceTotalLastMonth,
                    ARInvoiceTotalLastWeek = entity.ARInvoiceTotalLastWeek,
                    CustomerLastDate = entity.CustomerLastDate,
                    CustomerTotalLastMonth = entity.CustomerTotalLastMonth,
                    CustomerTotalLastWeek = entity.CustomerTotalLastWeek,
                    LastFHLSentDate = entity.LastFHLSentDate,
                    LastFWBSentDate = entity.LastFWBSentDate,
                    QuoteLastDate = entity.QuoteLastDate,
                    QuoteTotalLastMonth = entity.QuoteTotalLastMonth,
                    QuoteTotalLastWeek = entity.QuoteTotalLastWeek,
                    ShipmentLastDate = entity.ShipmentLastDate,
                    ShipmentTotalLastMonth = entity.ShipmentTotalLastMonth,
                    ShipmentTotalLastWeek = entity.ShipmentTotalLastWeek,
                    StatisticsUpdateDate = entity.StatisticsUpdateDate,
                    CountryName = entity.CountryName,
                    LastLoginDateTime = entity.LastLoginDateTime,
                    PaymentCurrencyCode = entity.PaymentCurrencyCode,
                    IsAWBStockPrepaid = entity.IsAWBStockPrepaid,
                    ManageLicencesPerUser = entity.ManageLicencesPerUser,
                    DistributorCode = entity.DistributorCode,
                    IsDistributorSupportEnabled = entity.IsDistributorSupportEnabled,
                    IsSystemSupportEnabled = entity.IsSystemSupportEnabled,
                    IsCargonautEnabled = entity.IsCargonautEnabled,
                    LastFHLCargonautSentDate = entity.LastFHLCargonautSentDate,
                    LastFWBCargonautSentDate = entity.LastFWBCargonautSentDate,
                    IsDEXXConnectionEnabled = entity.IsDEXXConnectionEnabled,
                    BluesnapContractId = entity.BluesnapContractId,
                    BluesnapCRMContractId = entity.BluesnapCRMContractId,
                    BluesnapEAWBContractId = entity.BluesnapEAWBContractId,
                    BluesnapEAWBSContractId = entity.BluesnapEAWBSContractId,
                    BluesnapOneTimeContract = entity.BluesnapOneTimeContract,
                    BluesnapContractQTY = entity.BluesnapContractQTY,
                    BluesnapCRMContractQTY = entity.BluesnapCRMContractQTY,
                    BluesnapEAWBContractQTY = entity.BluesnapEAWBContractQTY,
                    BluesnapEAWBSContractQTY = entity.BluesnapEAWBSContractQTY,
                    BluesnapOneTimeContractQTY = entity.BluesnapOneTimeContractQTY,
                    BluesnapInttraStockContractQTY = entity.BluesnapInttraStockContractQTY,
                    BluesnapInttraStockContractId = entity.BluesnapInttraStockContractId,
                    AWBMessagesCCSTypeCode = entity.AWBMessagesCCSTypeCode,
                    PIMA = entity.PIMA,
                    LastFFRSentDate = entity.LastFFRSentDate,
                    ActivityLastDate = entity.ActivityLastDate,
                    ActivityTotalLastWeek = entity.ActivityTotalLastWeek,
                    ActivityTotalLastMonth = entity.ActivityTotalLastMonth,
                    OpportunityLastDate = entity.OpportunityLastDate,
                    OpportunityTotalLastWeek = entity.OpportunityTotalLastWeek,
                    OpportunityTotalLastMonth = entity.OpportunityTotalLastMonth,
                    FSULastReceivedDate = entity.FSULastReceivedDate,
                    FSALastReceivedDate = entity.FSALastReceivedDate,
                    FSRLastSentDate = entity.FSRLastSentDate,
                    BillingByLogitude = entity.BillingByLogitude,
                    ResellerCommission = entity.ResellerCommission,
                    TenantTypeCode = entity.TenantTypeCode,
                    TenantConnectedToAirlineCode = entity.TenantConnectedToAirlineCode,
                    SupportEmail = entity.SupportEmail,
                    SupportActivated = entity.SupportActivated,
                    IsMultiPackage = entity.IsMultiPackage,
                    Technology = entity.Technology,
                    MobileLastDate = entity.MobileLastDate,
                    MobileTotalLastWeek = entity.MobileTotalLastWeek,
                    MobileTotalLastMonth = entity.MobileTotalLastMonth,
                    ShardLogisticLastDate = entity.ShardLogisticLastDate,
                    ShardLogisticTotalLastWeek = entity.ShardLogisticTotalLastWeek,
                    ShardLogisticTotalLastMonth = entity.ShardLogisticTotalLastMonth,
                    RequestedAirlines = entity.RequestedAirlines,
                    RegisteredAirlines = entity.RegisteredAirlines,
                    PendingAirlines = entity.PendingAirlines,
                    EnableBranding = entity.EnableBranding,
                    ContactEmail = entity.ContactEmail,
                    CustomerURL = entity.CustomerURL,
                    HideSharedlogistics = entity.HideSharedlogistics,
                    SilverlightEndDate = entity.SilverlightEndDate,
                    IsParentTenant = entity.IsParentTenant,
                    ParentTenantId = entity.ParentTenantId,
                    AgentSharedLogisticsStatisticsLastDate = entity.AgentSharedLogisticsStatisticsLastDate,
                    AgentSharedLogisticsStatisticsLastWeek = entity.AgentSharedLogisticsStatisticsLastWeek,
                    AgentSharedLogisticsStatisticsLastMonth = entity.AgentSharedLogisticsStatisticsLastMonth,
                    ChangeHeaderColor = entity.ChangeHeaderColor,
                    IsINTTRAStockPrepaid = entity.IsINTTRAStockPrepaid,
                    PackageCodeSearchField = entity.PackageCodeSearchField,
                    IsINTTRAOnlyDemo = entity.IsINTTRAOnlyDemo,
                    MainAdditionalPackageApplied = entity.MainAdditionalPackageApplied,
                    TotalPrice = entity.TotalPrice,
                    NoPaymentForChildTenants = entity.NoPaymentForChildTenants,
                };
            }

            return myResult;
        }

        public IQueryable<TenantManagementList> GetIQueryableEntityList(IQueryable<TenantManagement> iQueryable)
        {
            return from a in iQueryable
                   select new TenantManagementList()
                   {
                       Id = a.Id,
                       Name = a.Name,
                       PackageCode = a.PackageCode,
                       PackageName = a.PackageName,
                       IsTrial = a.IsTrial,
                       TrialStartDate = a.TrialStartDate,
                       TrialEndDate = a.TrialEndDate,
                       FirstPaymentDate = a.FirstPaymentDate,
                       PaidUntilDate = a.PaidUntilDate,
                       NumberOfUsers = a.NumberOfUsers,
                       IsActive = a.GlobalTenant.IsActive,
                       GlobalDBId = a.GlobalTenant.GlobalDBId,
                       SearchFields = a.SearchFields,
                       FreeUsers = a.FreeUsers,
                       IsRecurring = a.IsRecurring,
                       RecurringPeriodCode = a.RecurringPeriodCode,
                       CreateDate = a.CreateDate,
                       UpdateDate = a.UpdateDate,
                       PaymentFailure = a.PaymentFailure,
                       SuspendDate = a.SuspendDate,
                       InternalNotes = a.InternalNotes,
                       BluesnapAccount = a.BluesnapAccount,
                       MainContract = a.MainContract,
                       TemporalPackageCode = a.TemporalPackageCode,
                       TemporalStartDate = a.TemporalStartDate,
                       TemporalEndDate = a.TemporalEndDate,
                       LicensePrice = a.LicensePrice,
                       PaymentChannelCode = a.PaymentChannelCode,
                       PaymentMethodCode = a.PaymentMethodCode,
                       Notes = a.Notes,
                       TTY = a.TTY,
                       APInvoiceLastDate = a.APInvoiceLastDate,
                       APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                       APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                       ARInvoiceLastDate = a.ARInvoiceLastDate,
                       ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                       ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                       CustomerLastDate = a.CustomerLastDate,
                       CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                       CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                       LastFHLSentDate = a.LastFHLSentDate,
                       LastFWBSentDate = a.LastFWBSentDate,
                       QuoteLastDate = a.QuoteLastDate,
                       QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                       QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                       ShipmentLastDate = a.ShipmentLastDate,
                       ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                       ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                       StatisticsUpdateDate = a.StatisticsUpdateDate,
                       CountryName = a.CountryName,
                       LastLoginDateTime = a.LastLoginDateTime,
                       PaymentCurrencyCode = a.PaymentCurrencyCode,
                       IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                       ManageLicencesPerUser = a.ManageLicencesPerUser,
                       DistributorCode = a.DistributorCode,
                       IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                       IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                       IsCargonautEnabled = a.IsCargonautEnabled,
                       LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                       LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                       IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                       BluesnapContractId = a.BluesnapContractId,
                       BluesnapCRMContractId = a.BluesnapCRMContractId,
                       BluesnapEAWBContractId = a.BluesnapEAWBContractId,
                       BluesnapEAWBSContractId = a.BluesnapEAWBSContractId,
                       BluesnapOneTimeContract = a.BluesnapOneTimeContract,
                       BluesnapContractQTY = a.BluesnapContractQTY,
                       BluesnapCRMContractQTY = a.BluesnapCRMContractQTY,
                       BluesnapEAWBContractQTY = a.BluesnapEAWBContractQTY,
                       BluesnapEAWBSContractQTY = a.BluesnapEAWBSContractQTY,
                       BluesnapOneTimeContractQTY = a.BluesnapOneTimeContractQTY,
                       BluesnapInttraStockContractQTY = a.BluesnapInttraStockContractQTY,
                       BluesnapInttraStockContractId = a.BluesnapInttraStockContractId,
                       AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                       PIMA = a.PIMA,
                       LastFFRSentDate = a.LastFFRSentDate,
                       ActivityLastDate = a.ActivityLastDate,
                       ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                       ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                       OpportunityLastDate = a.OpportunityLastDate,
                       OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                       OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                       FSULastReceivedDate = a.FSULastReceivedDate,
                       FSALastReceivedDate = a.FSALastReceivedDate,
                       FSRLastSentDate = a.FSRLastSentDate,
                       BillingByLogitude = a.BillingByLogitude,
                       ResellerCommission = a.ResellerCommission,
                       TenantTypeCode = a.TenantTypeCode,
                       TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                       SupportActivated = a.SupportActivated,
                       SupportEmail = a.SupportEmail,
                       IsMultiPackage = a.IsMultiPackage,
                       MobileLastDate = a.MobileLastDate,
                       MobileTotalLastWeek = a.MobileTotalLastWeek,
                       MobileTotalLastMonth = a.MobileTotalLastMonth,
                       ShardLogisticLastDate = a.ShardLogisticLastDate,
                       ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                       ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                       EnableBranding = a.EnableBranding,
                       ContactEmail = a.ContactEmail,
                       CustomerURL = a.CustomerURL,
                       HideSharedlogistics = a.HideSharedlogistics,
                       SilverlightEndDate = a.SilverlightEndDate,
                       Technology = a.Technology,
                       IsParentTenant = a.IsParentTenant,
                       ParentTenantId = a.ParentTenantId,
                       AgentSharedLogisticsStatisticsLastDate = a.AgentSharedLogisticsStatisticsLastDate,
                       AgentSharedLogisticsStatisticsLastWeek = a.AgentSharedLogisticsStatisticsLastWeek,
                       AgentSharedLogisticsStatisticsLastMonth = a.AgentSharedLogisticsStatisticsLastMonth,
                       ChangeHeaderColor = a.ChangeHeaderColor,
                       IsINTTRAStockPrepaid = a.IsINTTRAStockPrepaid,
                       PackageCodeSearchField = a.PackageCodeSearchField,
                       IsINTTRAOnlyDemo = a.IsINTTRAOnlyDemo,
                       MainAdditionalPackageApplied = a.MainAdditionalPackageApplied,
                       TotalPrice = a.TotalPrice,
                       NoPaymentForChildTenants = a.NoPaymentForChildTenants,
                   };
        }

        public int ComputeDaysLeft(DateTime? date)
        {
            DateTime? startDate = DateTime.Now.Date;
            int days = 0;

            if (startDate != null && date != null)
            {
                TimeSpan? day = (date - startDate);

                if (day.Value.Days == 0)
                {
                    days = 0;
                }
                else if (day.Value.Days < 0)
                {
                    days = day.Value.Days;
                }
                else
                {
                    days = day.Value.Days;
                }
            }
            return days;
        }

        private void GetPackagesData(TenantManagementPM entityPM)
        {
            TenantManagementLicenseQuery licenseQuery = new TenantManagementLicenseQuery(entityPM.Id);
            entityPM.TenantManagementLicenses = licenseQuery.GetTenantManagementLicensePMs(entityPM.Id).ToList();

            if (entityPM.MainAdditionalPackageApplied)
            {
                if (entityPM.PackagesCodes_PK == null)
                {
                    entityPM.PackagesCodes_PK = new List<string>();
                }

                if (entityPM.PackagesCodes_BS == null)
                {
                    entityPM.PackagesCodes_BS = new List<string>();
                }

                if (entityPM.PackageCode != null)
                {
                    PackageRepository pckgRep = new PackageRepository(entityPM.Id);
                    Package pckg = pckgRep.GetSinglePackage(entityPM.PackageCode);
                    
                    if (pckg.FeaturePackageTypeCode == "BS")
                    {
                        entityPM.PackagesCodes_BS.Add(pckg.Code);
                    }

                    else
                    {
                        entityPM.PackagesCodes_PK.Add(pckg.Code);

                        PackageConnectedPackageRepository connectedPackageRepository = new PackageConnectedPackageRepository(entityPM.Id);
                        List<string> myCodes = (from a in connectedPackageRepository.context.PackageConnectedPackages
                                                where pckg.Code == a.PackageCode
                                                group a by a.ConnectedPackageCode into g
                                                select g.Key).ToList();

                        entityPM.PackagesCodes_BS = myCodes;
                    }
                }

                if(entityPM.IsMultiPackage)
                {
                    List<string> licensesCodes = entityPM.TenantManagementLicenses.Select(s => s.PackageCode).ToList();
                    entityPM.PackagesCodes_PK.AddRange(licensesCodes);

                    PackageConnectedPackageRepository connectedPackageRepository = new PackageConnectedPackageRepository(entityPM.Id);
                    entityPM.PackagesCodes_BS.AddRange((from a in connectedPackageRepository.context.PackageConnectedPackages
                                                 where licensesCodes.Contains(a.PackageCode)
                                                 group a by a.ConnectedPackageCode into g
                                                 select g.Key).ToList());
                }
            }

            else
            {
                if (entityPM.IsMultiPackage)
                {
                    List<string> licensesCodes = entityPM.TenantManagementLicenses.Select(s => s.PackageCode).ToList();
                    entityPM.PackagesCodes_PK = licensesCodes;

                    PackageConnectedPackageRepository connectedPackageRepository = new PackageConnectedPackageRepository(entityPM.Id);
                    entityPM.PackagesCodes_BS = (from a in connectedPackageRepository.context.PackageConnectedPackages
                                                 where licensesCodes.Contains(a.PackageCode)
                                                 group a by a.ConnectedPackageCode into g
                                                 select g.Key).ToList();

                }

                if (entityPM.PackageCode != null)
                {
                    PackageRepository pckgRep = new PackageRepository(entityPM.Id);
                    Package pckg = pckgRep.GetSinglePackage(entityPM.PackageCode);

                    if (!entityPM.IsMultiPackage)
                    {
                        if (entityPM.PackagesCodes_PK == null)
                        {
                            entityPM.PackagesCodes_PK = new List<string>();
                        }

                        if (entityPM.PackagesCodes_BS == null)
                        {
                            entityPM.PackagesCodes_BS = new List<string>();
                        }

                        if (pckg.FeaturePackageTypeCode == "BS")
                        {
                            entityPM.PackagesCodes_BS.Add(pckg.Code);
                        }

                        else
                        {
                            entityPM.PackagesCodes_PK.Add(pckg.Code);

                            PackageConnectedPackageRepository connectedPackageRepository = new PackageConnectedPackageRepository(entityPM.Id);
                            List<string> myCodes = (from a in connectedPackageRepository.context.PackageConnectedPackages
                                                    where pckg.Code == a.PackageCode
                                                    group a by a.ConnectedPackageCode into g
                                                    select g.Key).ToList();

                            entityPM.PackagesCodes_BS = myCodes;
                        }
                    }
                }

                if (entityPM.TemporalPackageCode != null)
                {
                    PackageRepository pckgRep = new PackageRepository(entityPM.Id);
                    Package pckg = pckgRep.GetSinglePackage(entityPM.TemporalPackageCode);
                    entityPM.TemporalPackageName = pckg.Name;
                }
            }
        }

        public TenantManagementPM GetTenantManagementPM(int id)
        {
            TenantManagementPM tenant = (from a in repository.context.TenantManagements
                                         where a.Id == id
                                         select new TenantManagementPM()
                                         {
                                             Id = a.Id,
                                             Name = a.Name,
                                             PackageCode = a.PackageCode,
                                             PackageName = a.PackageName,
                                             IsTrial = a.IsTrial,
                                             TrialStartDate = a.TrialStartDate,
                                             TrialEndDate = a.TrialEndDate,
                                             FirstPaymentDate = a.FirstPaymentDate,
                                             PaidUntilDate = a.PaidUntilDate,
                                             NumberOfUsers = a.NumberOfUsers,
                                             SearchFields = a.SearchFields,
                                             TTY = a.TTY,
                                             FreeUsers = a.FreeUsers,
                                             IsRecurring = a.IsRecurring,
                                             RecurringPeriodCode = a.RecurringPeriodCode,
                                             CreateDate = a.CreateDate,
                                             UpdateDate = a.UpdateDate,
                                             PaymentFailure = a.PaymentFailure,
                                             SuspendDate = a.SuspendDate,
                                             InternalNotes = a.InternalNotes,
                                             BluesnapAccount = a.BluesnapAccount,
                                             MainContract = a.MainContract,
                                             TemporalPackageCode = a.TemporalPackageCode,
                                             TemporalStartDate = a.TemporalStartDate,
                                             TemporalEndDate = a.TemporalEndDate,
                                             LicensePrice = a.LicensePrice,
                                             PaymentChannelCode = a.PaymentChannelCode,
                                             PaymentMethodCode = a.PaymentMethodCode,
                                             Notes = a.Notes,
                                             APInvoiceLastDate = a.APInvoiceLastDate,
                                             APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                                             APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                                             ARInvoiceLastDate = a.ARInvoiceLastDate,
                                             ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                                             ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                                             CustomerLastDate = a.CustomerLastDate,
                                             CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                                             CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                                             LastFHLSentDate = a.LastFHLSentDate,
                                             LastFWBSentDate = a.LastFWBSentDate,
                                             QuoteLastDate = a.QuoteLastDate,
                                             QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                                             QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                                             ShipmentLastDate = a.ShipmentLastDate,
                                             ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                                             ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                                             StatisticsUpdateDate = a.StatisticsUpdateDate,
                                             CountryName = a.CountryName,
                                             LastLoginDateTime = a.LastLoginDateTime,
                                             PaymentCurrencyCode = a.PaymentCurrencyCode,
                                             IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                                             ManageLicencesPerUser = a.ManageLicencesPerUser,
                                             DistributorCode = a.DistributorCode,
                                             IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                                             IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                                             IsCargonautEnabled = a.IsCargonautEnabled,
                                             LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                                             LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                                             IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                                             AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                                             PIMA = a.PIMA,
                                             IsEAWBOnlyDemo = a.IsEAWBOnlyDemo,
                                             IsRestrictedByAirline = a.IsRestrictedByAirline,
                                             LastFFRSentDate = a.LastFFRSentDate,
                                             ManagesRegisteredAgent = a.ManagesRegisteredAgent,
                                             ActivityLastDate = a.ActivityLastDate,
                                             ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                                             ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                                             OpportunityLastDate = a.OpportunityLastDate,
                                             OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                                             OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                                             FSULastReceivedDate = a.FSULastReceivedDate,
                                             FSALastReceivedDate = a.FSALastReceivedDate,
                                             FSRLastSentDate = a.FSRLastSentDate,
                                             BillingByLogitude = a.BillingByLogitude,
                                             ResellerCommission = a.ResellerCommission,
                                             TenantTypeCode = a.TenantTypeCode,
                                             TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                                             SignupRequestRecipients = a.SignupRequestRecipients,
                                             LoginPageNotes = a.LoginPageNotes,
                                             SupportActivated = a.SupportActivated,
                                             SupportEmail = a.SupportEmail,
                                             IsMultiPackage = a.IsMultiPackage,
                                             Technology = a.Technology,
                                             MobileLastDate = a.MobileLastDate,
                                             MobileTotalLastWeek = a.MobileTotalLastWeek,
                                             MobileTotalLastMonth = a.MobileTotalLastMonth,
                                             ShardLogisticLastDate = a.ShardLogisticLastDate,
                                             ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                                             ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                                             RequestedAirlines = a.RequestedAirlines,
                                             RegisteredAirlines = a.RegisteredAirlines,
                                             PendingAirlines = a.PendingAirlines,
                                             EnableBranding = a.EnableBranding,
                                             ContactEmail = a.ContactEmail,
                                             CustomerURL = a.CustomerURL,
                                             HideSharedlogistics = a.HideSharedlogistics,
                                             SilverlightEndDate = a.SilverlightEndDate,
                                             IsParentTenant = a.IsParentTenant,
                                             ParentTenantId = a.ParentTenantId,
                                             AgentSharedLogisticsStatisticsLastDate = a.AgentSharedLogisticsStatisticsLastDate,
                                             AgentSharedLogisticsStatisticsLastWeek = a.AgentSharedLogisticsStatisticsLastWeek,
                                             AgentSharedLogisticsStatisticsLastMonth = a.AgentSharedLogisticsStatisticsLastMonth,
                                             ChangeHeaderColor = a.ChangeHeaderColor,
                                             IsINTTRAStockPrepaid = a.IsINTTRAStockPrepaid,
                                             PackageCodeSearchField = a.PackageCodeSearchField,
                                             IsINTTRAOnlyDemo = a.IsINTTRAOnlyDemo,
                                             MainAdditionalPackageApplied = a.MainAdditionalPackageApplied,
                                             TotalPrice = a.TotalPrice,
                                             SupportDomain = a.SupportDomain,
                                             TotalNumberOfUsers = a.TotalNumberOfUsers,
                                             TotalFreeUsers = a.TotalFreeUsers,
                                             AveragePrice = a.AveragePrice,
                                             TotalPaymentamount = a.TotalPaymentamount,
                                             NoPaymentForChildTenants = a.NoPaymentForChildTenants,
                                         }).FirstOrDefault();

            return tenant;
        }

        public List<TenantManagementDW> GetTenantManagementDWs(int tenant, int skip, int take)
        {
            List<TenantManagementDW> result = (from a in repository.context.TenantManagements.Include("PaymentChannel").Include("PaymentCurrency").Include("RecurringPeriod")
                                               select new TenantManagementDW()
                                               {
                                                   TenantNumber = a.Id,
                                                   FreeUsers = a.TotalFreeUsers,
                                                   IsRecurring = a.IsRecurring,
                                                   LicensePrice = a.AveragePrice,
                                                   Notes = a.Notes,
                                                   NumberOfUsers = a.TotalNumberOfUsers == null ? 0 : a.TotalNumberOfUsers.Value,
                                                   PaidUntilDate = a.PaidUntilDate,
                                                   PaymentChannel = a.PaymentChannel != null ? a.PaymentChannel.Name : "",
                                                   PaymentCurrency = a.PaymentCurrency != null ? a.PaymentCurrency.Name : "",
                                                   RecurringPeriod = a.RecurringPeriod != null ? a.RecurringPeriod.Name : "",
                                                   ResellerCommission = a.ResellerCommission,
                                                   IsMultiPackage = a.IsMultiPackage,
                                                   PackageCode = a.PackageCode,
                                                   MainPackage = a.PackageName,
                                                   CRMYN = a.PackageCode == "LOGI" ? "Y" : "N",
                                                   EAWBYN = a.IsAWBStockPrepaid || a.PackageCode == "EAWB" || a.PackageCode == "BUBK" ? "Y" : "N",
                                                   MainPackageNumberOfUsers = a.MainAdditionalPackageApplied ? (a.NumberOfUsers == null ? 0 : a.NumberOfUsers.Value) : (!a.IsMultiPackage ? (a.NumberOfUsers == null ? 0 : a.NumberOfUsers.Value) : 0),
                                                   CRMNumberOfUsers = !a.IsMultiPackage && a.PackageCode == "LOGI" ? (a.NumberOfUsers == null ? 0 : a.NumberOfUsers.Value) : 0,
                                                   EAWBNumberOfUsers = !a.IsMultiPackage && (a.PackageCode == "EAWB" || a.PackageCode == "BUBK") ? (a.NumberOfUsers == null ? 0 : a.NumberOfUsers.Value) : 0,
                                               }).OrderBy(d => d.TenantNumber).Skip(skip).Take(take).ToList();

            List<int> tenantManagementIds = new List<int>();
            foreach (TenantManagementDW item in result.Where(d => d.IsMultiPackage == true).ToList())
            {
                if (!tenantManagementIds.Contains(item.TenantNumber)) tenantManagementIds.Add(item.TenantNumber);
            }


            List<TenantManagementLicensePM> tenantManagementLicensePMLists = new List<TenantManagementLicensePM>();
            if (tenantManagementIds.Count > 0)
            {
                TenantManagementLicenseQuery tenantManagementLicenseQuery = new TenantManagementLicenseQuery(tenant);
                tenantManagementLicensePMLists = tenantManagementLicenseQuery.GetTenantManagementLicenseByListids(tenantManagementIds).ToList();

            }
            PackageRepository packageRepository = new PackageRepository(tenant);
            List<Package> packageLists = packageRepository.GetPackages().ToList();

            foreach (TenantManagementDW item in result)
            {
                if (!string.IsNullOrEmpty(item.PackageCode))
                {
                    Package package = packageLists.Where(d => d.Code == item.PackageCode).FirstOrDefault();
                    if (package != null)
                    {
                        item.MainPackage = package.Name;
                    }
                }

                if (item.IsMultiPackage)
                {
                    List<TenantManagementLicensePM> multiPackage = tenantManagementLicensePMLists.Where(d => d.Tenant == item.TenantNumber).ToList();
                    if (multiPackage.Count > 0)
                    {
                        #region CRMYN
                        if (item.CRMYN == "N")
                        {
                            item.CRMYN = CheckIfMultiPackageHasThisCode(multiPackage, "LOGI") ? "Y" : "N";
                        }
                        #endregion

                        #region EAWBYN
                        if (item.EAWBYN == "N")
                        {
                            item.EAWBYN = CheckIfMultiPackageHasThisCode(multiPackage, "EAWB", "BUBK") ? "Y" : "N";
                        }
                        #endregion

                        #region Main Package Number of User
                        TenantManagementLicensePM package = multiPackage.Where(d => d.PackageCode != "EAWB" || d.PackageCode != "LOGI").OrderByDescending(d => d.NumberOfUsers).FirstOrDefault();
                        if (package != null) item.MainPackageNumberOfUsers = package.NumberOfUsers != null ? (int)package.NumberOfUsers : 0;

                        #endregion

                        #region CRM  Number of User
                        TenantManagementLicensePM crmPackage = multiPackage.Where(d => d.PackageCode == "LOGI").FirstOrDefault();
                        if (crmPackage != null) item.CRMNumberOfUsers = crmPackage.NumberOfUsers != null ? (int)crmPackage.NumberOfUsers : 0;
                        else item.CRMNumberOfUsers = 0;


                        #endregion

                        #region E-AWB  Number of User
                        TenantManagementLicensePM eAWBBackage = multiPackage.Where(d => d.PackageCode == "EAWB" || d.PackageCode == "BUBK").FirstOrDefault();
                        if (eAWBBackage != null) item.EAWBNumberOfUsers = eAWBBackage.NumberOfUsers != null ? (int)eAWBBackage.NumberOfUsers : 0;
                        else item.EAWBNumberOfUsers = 0;


                        #endregion
                    }
                }
            }


            return result;
        }

        private bool CheckIfMultiPackageHasThisCode(List<TenantManagementLicensePM> multiPackage, string packageCode, string packageCode2 = null)
        {
            TenantManagementLicensePM tenantManagementLicensePM = null;
            if (multiPackage != null)
                tenantManagementLicensePM = multiPackage.Where(d => d.PackageCode == packageCode).FirstOrDefault();
            if (tenantManagementLicensePM == null && !string.IsNullOrEmpty(packageCode2))
            {
                tenantManagementLicensePM = multiPackage.Where(d => d.PackageCode == packageCode2).FirstOrDefault();
            }

            return tenantManagementLicensePM != null ? true : false;
        }

        public string GetSystemDomain(int id)
        {
            string workEnvironment = Simplog.Server.Infrastructure.LogitudeSettings.WorkEnvironment;
            string deploymentStage = Simplog.Server.Infrastructure.LogitudeSettings.DeploymentStage;
            string fromEmail = "no-reply@" + (workEnvironment == "cloud" 
                ? "amital.co.il" : deploymentStage != null && (deploymentStage.ToLower() == "logboxwe1" || deploymentStage.ToLower() == "test2")
                ? GetLogboxDomain(id) : "LogitudeWorld.com");

            return fromEmail;
        }

        private string GetLogboxDomain(int tenantId)
        {
            string privateLabelId = (from a in repository.context.GlobalTenants
                                     where a.Id == tenantId
                                     select a.PrivateLabelId).FirstOrDefault();


            if (!string.IsNullOrEmpty(privateLabelId))
            {
                string privateLabelUrl = (from a in repository.context.TenantManagmentPrivateLabels
                                          where a.Id == privateLabelId
                                          select a.PrivateLabelUrl).FirstOrDefault();

                if (!string.IsNullOrEmpty(privateLabelUrl))
                {
                    privateLabelUrl = privateLabelUrl.Replace("system.", "");
                    return privateLabelUrl;
                }

            }
            return "logbox.co.il";
        }
    }
}