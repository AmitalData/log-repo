using Logitude.BL.GlobalModel.EntityPMs;
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
            entityPOCO.BluesnapContractCode = entityPM.BluesnapContractCode;
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
            entityPOCO.ContactEmail = entityPM.ContactEmail;
            entityPOCO.CustomerURL = entityPM.CustomerURL;
            entityPOCO.HideSharedlogistics = entityPM.HideSharedlogistics;
            entityPOCO.SilverlightEndDate = entityPM.SilverlightEndDate;
            entityPM.PackageName = entityPM.PackageCode;
            entityPOCO.IsParentTenant = entityPM.IsParentTenant;
            entityPOCO.ParentTenantId = entityPM.ParentTenantId;
            entityPOCO.AgentSharedLogisticsStatisticsLastDate = entityPM.AgentSharedLogisticsStatisticsLastDate;
            entityPOCO.AgentSharedLogisticsStatisticsLastWeek = entityPM.AgentSharedLogisticsStatisticsLastWeek;
            entityPOCO.AgentSharedLogisticsStatisticsLastMonth = entityPM.AgentSharedLogisticsStatisticsLastMonth;
            entityPOCO.ChangeHeaderColor = entityPM.ChangeHeaderColor;
            entityPOCO.StockTypeCode = entityPM.StockTypeCode;

            if (entityPM.IsMultiPackage)
            {
                entityPM.PackageName = "Multi Package";
            }

            entityPOCO.PackageName = entityPM.PackageName;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantRepository tenantRepository = new TenantRepository(entityPM.Id);
                Tenant tenant = tenantRepository.GetSingleTenant(entityPM.Id);

                if (tenant != null)
                {
                    tenant.Company = entityPM.Name;
                    tenant.DocumentShareAsDefault = entityPM.DocumentShareAsDefault;
                    if (!entityPM.ManagesRegisteredAgent)
                    {
                        tenant.RegulatedAgentRegimeActivated = false;
                        
                    }

                    tenantRepository.Update(tenant);
                    tenantRepository.SubmitChanges();
                }

                scope.Complete();
            }
        }
    }
}
