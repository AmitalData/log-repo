using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class TenantManagementMapping
    {
        public static void MapEntity(TenantManagementPM entityPM, TenantManagement entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.BluesnapContractQTY = entityPM.NumberOfUsers == null ? 0 : entityPM.NumberOfUsers.Value;
                entityPOCO.BluesnapCRMContractQTY = 1;
                entityPOCO.BluesnapEAWBContractQTY = 1;
                entityPOCO.BluesnapEAWBSContractQTY = 1;
                entityPOCO.BluesnapOneTimeContractQTY = 1;
                entityPOCO.BluesnapInttraStockContractQTY = 1;
            }

            else
            {
                entityPOCO.BluesnapContractQTY = entityPM.BluesnapContractQTY;
                entityPOCO.BluesnapCRMContractQTY = entityPM.BluesnapCRMContractQTY;
                entityPOCO.BluesnapEAWBContractQTY = entityPM.BluesnapEAWBContractQTY;
                entityPOCO.BluesnapEAWBSContractQTY = entityPM.BluesnapEAWBSContractQTY;
                entityPOCO.BluesnapOneTimeContractQTY = entityPM.BluesnapOneTimeContractQTY;
                entityPOCO.BluesnapInttraStockContractQTY = entityPM.BluesnapInttraStockContractQTY;
            }

            entityPOCO.Name = entityPM.Name;
            entityPOCO.PackageCode = entityPM.PackageCode;
            entityPOCO.IsTrial = entityPM.IsTrial;
            entityPOCO.TrialStartDate = entityPM.TrialStartDate;
            entityPOCO.TrialEndDate = entityPM.TrialEndDate;
            entityPOCO.FirstPaymentDate = entityPM.FirstPaymentDate;
            entityPOCO.PaidUntilDate = entityPM.PaidUntilDate;
            entityPOCO.NumberOfUsers = entityPM.NumberOfUsers;
            entityPOCO.TTY = entityPM.TTY;
            entityPOCO.SearchFields = entityPM.Id + "," + entityPM.Name + "," + entityPM.NumberOfUsers;
            entityPOCO.FreeUsers = entityPM.FreeUsers;
            entityPOCO.IsRecurring = entityPM.IsRecurring;
            entityPOCO.RecurringPeriodCode = entityPM.RecurringPeriodCode;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.PaymentFailure = entityPM.PaymentFailure;
            entityPOCO.SuspendDate = entityPM.SuspendDate;
            entityPOCO.InternalNotes = entityPM.InternalNotes;
            entityPOCO.BluesnapAccount = entityPM.BluesnapAccount;
            entityPOCO.MainContract = entityPM.MainContract;
            entityPOCO.TemporalPackageCode = entityPM.TemporalPackageCode;
            entityPOCO.TemporalStartDate = entityPM.TemporalStartDate;
            entityPOCO.TemporalEndDate = entityPM.TemporalEndDate;
            entityPOCO.PaymentMethodCode = entityPM.PaymentMethodCode;
            entityPOCO.PaymentChannelCode = entityPM.PaymentChannelCode;
            entityPOCO.LicensePrice = entityPM.LicensePrice;
            entityPOCO.Notes = entityPM.Notes;
            entityPOCO.PaymentCurrencyCode = entityPM.PaymentCurrencyCode;
            entityPOCO.IsAWBStockPrepaid = entityPM.IsAWBStockPrepaid;
            entityPOCO.ManageLicencesPerUser = entityPM.ManageLicencesPerUser;
            entityPOCO.DistributorCode = entityPM.DistributorCode;
            entityPOCO.IsDistributorSupportEnabled = entityPM.IsDistributorSupportEnabled;
            entityPOCO.IsSystemSupportEnabled = entityPM.IsSystemSupportEnabled;
            entityPOCO.IsCargonautEnabled = entityPM.IsCargonautEnabled;
            entityPOCO.IsDEXXConnectionEnabled = entityPM.IsDEXXConnectionEnabled;
            entityPOCO.BluesnapContractId = entityPM.BluesnapContractId;
            entityPOCO.BluesnapCRMContractId = entityPM.BluesnapCRMContractId;
            entityPOCO.BluesnapEAWBContractId = entityPM.BluesnapEAWBContractId;
            entityPOCO.BluesnapEAWBSContractId = entityPM.BluesnapEAWBSContractId;
            entityPOCO.BluesnapOneTimeContract = entityPM.BluesnapOneTimeContract;
            entityPOCO.BluesnapInttraStockContractId = entityPM.BluesnapInttraStockContractId;

            entityPOCO.GlobalTenant.TTY = entityPM.TTY;
            entityPOCO.GlobalTenant.IsActive = entityPM.IsActive;
            entityPOCO.GlobalTenant.CompanyName = entityPM.Name;
            entityPOCO.AWBMessagesCCSTypeCode = entityPM.AWBMessagesCCSTypeCode;
            entityPOCO.PIMA = entityPM.PIMA;
            entityPOCO.IsEAWBOnlyDemo = entityPM.IsEAWBOnlyDemo;
            entityPOCO.IsRestrictedByAirline = entityPM.IsRestrictedByAirline;
            entityPOCO.LastFFRSentDate = entityPM.LastFFRSentDate;
            entityPOCO.ManagesRegisteredAgent = entityPM.ManagesRegisteredAgent;
            entityPOCO.ActivityLastDate = entityPM.ActivityLastDate;
            entityPOCO.ActivityTotalLastWeek = entityPM.ActivityTotalLastWeek;
            entityPOCO.ActivityTotalLastMonth = entityPM.ActivityTotalLastMonth;
            entityPOCO.OpportunityLastDate = entityPM.OpportunityLastDate;
            entityPOCO.OpportunityTotalLastWeek = entityPM.OpportunityTotalLastWeek;
            entityPOCO.OpportunityTotalLastMonth = entityPM.OpportunityTotalLastMonth;
            entityPOCO.FSULastReceivedDate = entityPM.FSULastReceivedDate;
            entityPOCO.FSALastReceivedDate = entityPM.FSALastReceivedDate;
            entityPOCO.FSRLastSentDate = entityPM.FSRLastSentDate;
            entityPOCO.BillingByLogitude = entityPM.BillingByLogitude;
            entityPOCO.ResellerCommission = entityPM.ResellerCommission;
            entityPOCO.TenantTypeCode = entityPM.TenantTypeCode;
            entityPOCO.TenantConnectedToAirlineCode = entityPM.TenantConnectedToAirlineCode;
            entityPOCO.SignupRequestRecipients = entityPM.SignupRequestRecipients;
            entityPOCO.LoginPageNotes = entityPM.LoginPageNotes;
            entityPOCO.SupportActivated = entityPM.SupportActivated;
            entityPOCO.SupportEmail = entityPM.SupportEmail;
            entityPOCO.IsMultiPackage = entityPM.IsMultiPackage;
            entityPOCO.Technology = entityPM.Technology;
            entityPOCO.MobileLastDate = entityPM.MobileLastDate;
            entityPOCO.MobileTotalLastWeek = entityPM.MobileTotalLastWeek;
            entityPOCO.MobileTotalLastMonth = entityPM.MobileTotalLastMonth;
            entityPOCO.ShardLogisticLastDate = entityPM.ShardLogisticLastDate;
            entityPOCO.ShardLogisticTotalLastWeek = entityPM.ShardLogisticTotalLastWeek;
            entityPOCO.ShardLogisticTotalLastMonth = entityPM.ShardLogisticTotalLastMonth;
            entityPOCO.RequestedAirlines = entityPM.RequestedAirlines;
            entityPOCO.RegisteredAirlines = entityPM.RegisteredAirlines;
            entityPOCO.PendingAirlines = entityPM.PendingAirlines;
            entityPOCO.EnableBranding = entityPM.EnableBranding;
            entityPOCO.EnableExportToExcel = entityPM.EnableExportToExcel;
            entityPOCO.ActivatePrivateSite = entityPM.ActivatePrivateSite;
            entityPOCO.ContactEmail = entityPM.ContactEmail;
            entityPOCO.CustomerURL = entityPM.CustomerURL;
            entityPOCO.ActivatedforDeclarationApprove = entityPM.ActivatedforDeclarationApprove;
            entityPOCO.ShowMoneyOrder = entityPM.ShowMoneyOrder;
            entityPOCO.DeclarationMessage = entityPM.DeclarationMessage;
            entityPOCO.HideSharedlogistics = entityPM.HideSharedlogistics;
            entityPOCO.SilverlightEndDate = entityPM.SilverlightEndDate;
            entityPOCO.IsParentTenant = entityPM.IsParentTenant;
            entityPOCO.ParentTenantId = entityPM.ParentTenantId;
            entityPOCO.AgentSharedLogisticsStatisticsLastDate = entityPM.AgentSharedLogisticsStatisticsLastDate;
            entityPOCO.AgentSharedLogisticsStatisticsLastWeek = entityPM.AgentSharedLogisticsStatisticsLastWeek;
            entityPOCO.AgentSharedLogisticsStatisticsLastMonth = entityPM.AgentSharedLogisticsStatisticsLastMonth;
            entityPOCO.ChangeHeaderColor = entityPM.ChangeHeaderColor;
            entityPOCO.HeaderColor = entityPM.HeaderColor;
            entityPOCO.StockTypeCode = entityPM.StockTypeCode;
            entityPOCO.PackageCodeSearchField = entityPM.PackageCodeSearchField;
            entityPOCO.IsINTTRAStockPrepaid = entityPM.IsINTTRAStockPrepaid;
            entityPOCO.IsINTTRAOnlyDemo = entityPM.IsINTTRAOnlyDemo;
            entityPOCO.MainAdditionalPackageApplied = entityPM.MainAdditionalPackageApplied;
            entityPOCO.TotalPrice = entityPM.TotalPrice;
            entityPOCO.SupportDomain = entityPM.SupportDomain;
            entityPOCO.MainColor = entityPM.MainColor;
            entityPOCO.TotalNumberOfUsers = entityPM.TotalNumberOfUsers;
            entityPOCO.TotalFreeUsers = entityPM.TotalFreeUsers;
            entityPOCO.AveragePrice = entityPM.AveragePrice;
            entityPOCO.TotalPaymentamount = entityPM.TotalPaymentamount;
            entityPOCO.TertiaryColor = entityPM.TertiaryColor;
            entityPOCO.SecondaryColor = entityPM.SecondaryColor;
            entityPM.MainColor = entityPM.MainColor;
            entityPM.SecondaryColor = entityPM.SecondaryColor;
            entityPOCO.ComapnylogoId = entityPM.ComapnylogoId;
            entityPOCO.InvertedLogoId = entityPM.InvertedLogoId;
            entityPOCO.BackgroundId = entityPM.BackgroundId;
            entityPOCO.MobileBackgroundId = entityPM.MobileBackgroundId;
            entityPOCO.BrowserIconId = entityPM.BrowserIconId;
            entityPOCO.ShipmentHeaderImageId = entityPM.ShipmentHeaderImageId;
            entityPOCO.NoPaymentForChildTenants = entityPM.NoPaymentForChildTenants;

            entityPOCO.LastEbookingSentDate = entityPM.LastEbookingSentDate;
            entityPOCO.LastSISentDate = entityPM.LastSISentDate;
            entityPOCO.NumberOfBookingSentLastWeek = entityPM.NumberOfBookingSentLastWeek;
            entityPOCO.NumberOfSISentLastWeek = entityPM.NumberOfSISentLastWeek;
            entityPOCO.LastContainerStatusReceived = entityPM.LastContainerStatusReceived;

            entityPOCO.LastTariffUpdateDate = entityPM.LastTariffUpdateDate;
            entityPOCO.LastTariffUsageDate = entityPM.LastTariffUsageDate;
            entityPOCO.LastWeekCreatedTariffs = entityPM.LastWeekCreatedTariffs;
            entityPOCO.LastMonthCreatedTariffs = entityPM.LastMonthCreatedTariffs;
            entityPOCO.PermissionBuildMonths = entityPM.PermissionBuildMonths;
            entityPOCO.ScheduledTasksLimitPerReport = entityPM.ScheduledTasksLimitPerReport;
            entityPOCO.WhatsAppMessagingPhoneNumber = entityPM.WhatsAppMessagingPhoneNumber;
            entityPOCO.CargoTokenTimeout = entityPM.CargoTokenTimeout;
            entityPOCO.IsContainerTrackingPrepaid = entityPM.IsContainerTrackingPrepaid;
            entityPOCO.DigitalPortalLastDate = entityPM.DigitalPortalLastDate;
            entityPOCO.DigitalPortalTotalLastWeek = entityPM.DigitalPortalTotalLastWeek;
            entityPOCO.DigitalPortalTotalLastMonth = entityPM.DigitalPortalTotalLastMonth;
            entityPOCO.DigitalPortalMobileLastDate = entityPM.DigitalPortalMobileLastDate;
            entityPOCO.DigitalPortalMobTotalLastWeek = entityPM.DigitalPortalMobTotalLastWeek;
            entityPOCO.DigitalPortalMobTotalLastMonth = entityPM.DigitalPortalMobTotalLastMonth;
            entityPOCO.DPArchiveShipmentCreateFilter  = entityPM.DPArchiveShipmentCreateFilter;
            entityPOCO.DPArchiveShipmentArrivalFilter = entityPM.DPArchiveShipmentArrivalFilter;
            entityPOCO.DPArchiveShipmentDepartFilter =  entityPM.DPArchiveShipmentDepartFilter;
            entityPOCO.CargoTrackingPublicShowEvents = entityPM.CargoTrackingPublicShowEvents;
            entityPOCO.CargoTrackingPrivateShowEvents = entityPM.CargoTrackingPrivateShowEvents;
            entityPOCO.LogoURL = entityPM.LogoURL;
            entityPOCO.ServiceAgreementURL = entityPM.ServiceAgreementURL;

            string packageName = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantRepository tenantRepository = new TenantRepository(entityPM.Id);
                PackageRepository packageRepository = new PackageRepository(entityPM.Id);
                LogBoxTenantSettingRepository LBtenantsettingRepository = new LogBoxTenantSettingRepository(entityPM.Id);

                Tenant tenant = tenantRepository.GetSingleTenant(entityPM.Id);
                LogBoxTenantSetting LBtenantsetting = LBtenantsettingRepository.GetSingleLBTenant(entityPM.Id);



                if (tenant != null)
                {
                    tenant.Company = entityPM.Name;
                    if(LBtenantsetting!= null)
                    {
                        LBtenantsetting.DocumentShareAsDefault = entityPM.DocumentShareAsDefault;
                        LBtenantsetting.AutoArchiveOnInvoice = entityPM.AutoArchiveOnInvoice;
                        LBtenantsetting.AutoArchiveOnPODExport = entityPM.AutoArchiveOnPODExport;
                    }
                
                    if (!entityPM.ManagesRegisteredAgent)
                    {
                        tenant.RegulatedAgentRegimeActivated = false;

                    }

                    tenant.IsTestTenant = entityPM.IsTestTenant;
                    tenant.IsHybrid = entityPM.IsHybrid;
                    tenant.EcommerceSupportEmail = entityPM.EcommerceSupportEmail;
                    tenant.EcommerceTenant = entityPM.EcommerceTenant;


                    tenantRepository.Update(tenant);
                    tenantRepository.SubmitChanges();
                }

                Package package = packageRepository.GetSinglePackage(entityPM.PackageCode);
                if (package != null)
                {
                    packageName = package.Name;
                }

                scope.Complete();
            }

            if (entityPM.MainAdditionalPackageApplied || !entityPM.IsMultiPackage)
            {
                entityPM.PackageName = packageName;
            }

            else
            {
                entityPM.PackageName = "Multi Package";
            }

            entityPOCO.PackageName = entityPM.PackageName;
            BuildPackageCodeSearchFields(entityPM, entityPOCO);
        }

        private static void BuildPackageCodeSearchFields(TenantManagementPM entityPM, TenantManagement entityPOCO)
        {
            string myPackageCodeSearchField = "";

            var AddOnsCodes = entityPM.AddOns.Where(p => p.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).Select(p => p.PackageCode);
            foreach (string item in AddOnsCodes)
            {
                MethodHelper.AddToSearchFields(ref myPackageCodeSearchField, item);
            }

            var LicensesCodes = entityPM.TenantManagementLicenses.Where(p => p.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).Select(p => p.PackageCode);
            foreach (string item in LicensesCodes)
            {
                MethodHelper.AddToSearchFields(ref myPackageCodeSearchField, item);
            }

            MethodHelper.AddToSearchFields(ref myPackageCodeSearchField, entityPM.PackageCode);


            if (myPackageCodeSearchField.Length > 250)
            {
                myPackageCodeSearchField = myPackageCodeSearchField.Substring(0, 1000);
            }


            entityPOCO.PackageCodeSearchField = myPackageCodeSearchField;
            entityPM.PackageCodeSearchField = myPackageCodeSearchField;

        }
    }
}
