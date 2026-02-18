using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace WebFreight.Web.GlobalModelDB
{
    public class MockGlobalContext : IGlobalContext
    {
        public IDbSet<GlobalContact> GlobalContacts
        {
            get { throw new NotImplementedException(); }
        }

        List<GlobalTenant> globalTanents;
        MockObjectSet<GlobalTenant> globalTenantObjectSet;
        public IDbSet<GlobalTenant> GlobalTenants
        {
            get
            {
                if (globalTanents == null)
                {
                    globalTanents = new List<GlobalTenant>() {
                        new GlobalTenant() { Id= 1 , GlobalDBId="1-1" , TenantManagement=TenantManagements.Where(d=>d.Id==1).FirstOrDefault() },
                        new GlobalTenant() { Id = 2 , GlobalDBId="1-2" , TenantManagement=TenantManagements.Where(d=>d.Id==2).FirstOrDefault() } };
                    globalTenantObjectSet = new MockObjectSet<GlobalTenant>(globalTanents);
                }
                return globalTenantObjectSet;

            }
        }

        List<GlobalDB> globalDBs;
        MockObjectSet<GlobalDB> globalDbObjectSet;
        public IDbSet<GlobalDB> GlobalDBs
        {
            get
            {
                if (globalDBs == null)
                {
                    //globalDBs = new List<GlobalDB>() {
                    //    new GlobalDB() { Id= "1-1" , GlobalTenants=GlobalTenants.Where(d=>d.Id==1).ToList()  },
                    //    new GlobalDB() { Id = "1-2" , GlobalTenants=GlobalTenants.Where(d=>d.Id==2).ToList() }};
                    //globalDbObjectSet = new MockObjectSet<GlobalDB>(globalDBs);
                }
                return globalDbObjectSet;
            }
        }

        public IDbSet<ConvertProgramInfo> ConvertProgramInfoes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<PerformanceLog> PerformanceLogs
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ChangePasswordLog> ChangePasswordLogs
        {
            get { throw new NotImplementedException(); }
        }


        public IDbSet<LogitudeLead> LogitudeLeads
        {
            get { throw new NotImplementedException(); }
        }


        List<TenantManagement> tenantsManagements;
        public IDbSet<TenantManagement> TenantManagements
        {
            get
            {
                if (tenantsManagements == null)
                {
                    tenantsManagements = new List<TenantManagement>() {
                        new TenantManagement() { Id= 1 },
                        new TenantManagement() { Id = 2} };
                }
                return new MockObjectSet<TenantManagement>(tenantsManagements);
            }
        }

        List<RecurringPeriod> recurringPeriods;
        MockObjectSet<RecurringPeriod> recurringPeriodsObjectSet;
        public IDbSet<RecurringPeriod> RecurringPeriods
        {
            get
            {
                if (recurringPeriods == null)
                {
                    recurringPeriods = new List<RecurringPeriod>() {
                        new RecurringPeriod() { Code = "MN" , Name = "Monthly"  },
                        new RecurringPeriod() { Code = "YE" , Name = "Yearly" }};
                    recurringPeriodsObjectSet = new MockObjectSet<RecurringPeriod>(recurringPeriods);
                }
                return recurringPeriodsObjectSet;
            }
        }

        public IDbSet<PaymentChannel> PaymentChannels
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<PaymentMethod> PaymentMethods
        {
            get { throw new NotImplementedException(); }
        }

        public void SetAsModified(object entity)
        {
          
        }

        public void DetectChanges()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return 1;
        }

        public IDbSet<AnalyzeQueue> AnalyzeQueues
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<AnalyzeQueueStatus> AnalyzeQueueStatus
        {
            get { throw new NotImplementedException(); }
        }
        
        public string GetCurrentConnection()
        {
            throw new NotImplementedException();
        }
        
        public IDbSet<ContactPassword> ContactPasswords
        {
            get { throw new NotImplementedException(); }
        }
        
        public IDbSet<PasswordResetRequest> PasswordResetRequests
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Setting> Settings
        {
            get { throw new NotImplementedException(); }
        }
        
        public IDbSet<PaymentCurrency> PaymentCurrencies
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<AutoSignupEmail> AutoSignupEmails
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<BluesnapContract> BluesnapContracts
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<AWBMessagesCCSType> AWBMessagesCCSTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<MobileNotificationLog> MobileNotificationLogs
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ContactMobileDevice> ContactMobileDevices
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<SystemMetadataLastUpdate> SystemMetadataLastUpdates
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<MonitorServiceLastUpdate> MonitorServiceLastUpdates
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<HelpResource> HelpResources
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<BatchServicesDefinition> BatchServicesDefinitions
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<BatchServicesDefinitionMods> BatchServicesDefinitionMods
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<TenantType> TenantTypes
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<ApiCredintials> ApiCredintials
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<TenantManagementLicense> TenantManagementLicenses
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<TenantAddOn> TenantAddOns
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<OneTimePassword> OneTimePasswords
        {
            get { throw new NotImplementedException(); }
        }



        public IDbSet<TenantManagmentPrivateLabels> TenantManagmentPrivateLabels
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<AgentSharedLogisticsKey> AgentSharedLogisticsKeys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<SessionPolicy> SessionPolicies
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<CaptchaKey> CaptchaKeys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<InvalidEmailResetPassword> InvalidEmailResetPasswords
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<WebhookKeys> WebhookKeys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<BluesnapContractType> BluesnapContractTypes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<BluesnapTransaction> BluesnapTransactions
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<AuthenticationToken> AuthenticationTokens
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<DefaultAndConfiguration> DefaultAndConfigurations
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<DefaultAndConfigurationKey> DefaultAndConfigurationKeys
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
