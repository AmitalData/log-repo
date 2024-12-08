using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Mapping;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Devart.Data.Oracle.Entity.Configuration;
namespace Simplog.Global.Data.GlobalModel
{
    public class GlobalContext : DbContextBase, IGlobalContext
    {
        public GlobalContext()
            : base("LogitudeGlobalStr")
        {
            Database.SetInitializer<GlobalContext>(new MigrateDatabaseToLatestVersion<GlobalContext, Simplog.Global.Data.Migrations.Configuration>());
        }
        public GlobalContext(DbConnection connection)
            : base(connection, true)
        {

            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.SetInitializer<GlobalContext>(null);//(new MigrateDatabaseToLatestVersion<GlobalContext, Simplog.Global.Data.Migrations.Configuration>());
                                                         //bool exists = Database.CreateIfNotExists();
                                                         //var configuration = new Simplog.Global.Data.Migrations.Configuration();
                                                         //configuration.AutomaticMigrationDataLossAllowed = true;
                                                         //configuration.TargetDatabase = new DbConnectionInfo(connection.ConnectionString, "System.Data.SqlClient");
                                                         //var migrator = new DbMigrator(configuration);

            //migrator.Update();
        }

        public GlobalContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.SetInitializer<GlobalContext>(null);
        }

        public void SetAsModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }
        public static IGlobalContext OverrideIGlobalContextFake { get; set; }

        public static IGlobalContext GetContext(int? ConnectionLifetime = null, bool? suppressPool = null)
        {
            if (OverrideIGlobalContextFake != null)
            {
                return OverrideIGlobalContextFake;
            }
            string dbConnectionInfo = "";
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Oracle_Globalstr"].ConnectionString;
            }
            else
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
            }
            dbConnectionInfo = DbContextBaseUtil.GetConnectionStringWithAmitalNetRole(dbConnectionInfo);
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, ConnectionLifetime, suppressPool);
                return new GlobalContext(connection);
            }
            else
            {
                dbConnectionInfo = DatabaseInitializer.GetConnectionString(dbConnectionInfo, ConnectionLifetime, suppressPool);
                return new GlobalContext(dbConnectionInfo);
            }
        }

        public static GlobalContext GetContextByDBId(string dbId)
        {
            GlobalDB currentDb;

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{                
            currentDb = GlobalDbHelper.GetGlobalDBById(dbId);
            //}
            string dbConnectionInfo = "";
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Oracle_Globalstr"].ConnectionString;
            }
            else
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
            }
            if (dbConnectionInfo.Contains("Main"))
            { }

             dbConnectionInfo = DbContextBaseUtil.GetConnectionStringWithAmitalNetRole(dbConnectionInfo);
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            GlobalContext context = new GlobalContext(connection);

            return context;
        }

        public override LogitudeDBSchema LogitudeDBSchema
        {
            get { return Simplog.Server.Infrastructure.LogitudeDBSchema.LOGITUDE_GLOBAL; }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                var config = OracleEntityProviderConfig.Instance;
                config.Workarounds.DisableQuoting = true;
                //config.QueryOptions.CaseInsensitiveComparison = true;
                //config.QueryOptions.CaseInsensitiveLike = true;

                //modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();
            }



            Database.SetInitializer<GlobalContext>(null);
            //string databasename = DatabaseInitializer.GetDatabaseName();
            //Database.DefaultConnectionFactory.CreateConnection(databasename);
            modelBuilder.Configurations.Add(new AnalyzeQueueMap());
            modelBuilder.Configurations.Add(new AnalyzeQueueStatuMap());
            modelBuilder.Configurations.Add(new ConvertProgramInfoMap());
            modelBuilder.Configurations.Add(new GlobalContactMap());
            modelBuilder.Configurations.Add(new GlobalDBMap());
            modelBuilder.Configurations.Add(new GlobalTenantCounterMap());
            modelBuilder.Configurations.Add(new GlobalTenantMap());
            modelBuilder.Configurations.Add(new LogitudeLeadsMap());
            modelBuilder.Configurations.Add(new PaymentChannelMap());
            modelBuilder.Configurations.Add(new PaymentMethodMap());
            modelBuilder.Configurations.Add(new PerformanceLogMap());
            modelBuilder.Configurations.Add(new RecurringPeriodMap());
            modelBuilder.Configurations.Add(new TenantManagementMap());
            modelBuilder.Configurations.Add(new ContactPasswordMap());
            modelBuilder.Configurations.Add(new PasswordResetRequestMap());
            modelBuilder.Configurations.Add(new SettingMap());
            modelBuilder.Configurations.Add(new PaymentCurrencyMap());
            modelBuilder.Configurations.Add(new AutoSignupEmailMap());
            modelBuilder.Configurations.Add(new BluesnapContractMap());
            modelBuilder.Configurations.Add(new BluesnapTransactionMap());
            modelBuilder.Configurations.Add(new BluesnapContractTypeMap());
            modelBuilder.Configurations.Add(new AWBMessagesCCSTypeMap());
            modelBuilder.Configurations.Add(new MobileNotificationLogMap());
            modelBuilder.Configurations.Add(new ContactMobileDeviceMap());
            modelBuilder.Configurations.Add(new SystemMetadataLastUpdateMap());
            modelBuilder.Configurations.Add(new MonitorServiceLastUpdateMap());
            modelBuilder.Configurations.Add(new HelpResourceMap());
            modelBuilder.Configurations.Add(new ChangePasswordLogMap());
            modelBuilder.Configurations.Add(new TenantTypeMap());
            modelBuilder.Configurations.Add(new ApiCredintialsMap());
            modelBuilder.Configurations.Add(new TenantManagementLicenseMap());
            modelBuilder.Configurations.Add(new TenantAddOnMap());
            modelBuilder.Configurations.Add(new BatchServicesDefinitionModsMap());
            modelBuilder.Configurations.Add(new OneTimePasswordMap());
            modelBuilder.Configurations.Add(new TenantManagmentPrivateLabelsMap());
            modelBuilder.Configurations.Add(new AgentSharedLogisticsKeyMap());
            modelBuilder.Configurations.Add(new SessionPolicyMap());
            modelBuilder.Configurations.Add(new CaptchaKeyMap());
            modelBuilder.Configurations.Add(new InvalidEmailResetPasswordMap());
            modelBuilder.Configurations.Add(new WebhookKeysMap());

            //Was Missing
            modelBuilder.Configurations.Add(new BatchServicesDefinitionMap());

            base.OnModelCreating(modelBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            //Database.Connection.Close();
            base.Dispose(disposing);
        }

        public void DetectChanges()
        {
            this.ChangeTracker.DetectChanges();
        }

        public int SaveChanges()
        {
            DetectChanges();
            return base.SaveChanges();
        }

        public string GetCurrentConnection()
        {
            return this.Database.Connection.ConnectionString;
        }

        public IDbSet<Setting> Settings { get; set; }
        public IDbSet<ContactMobileDevice> ContactMobileDevices { get; set; }
        public IDbSet<MobileNotificationLog> MobileNotificationLogs { get; set; }
        public IDbSet<GlobalContact> GlobalContacts { get; set; }
        public IDbSet<GlobalTenant> GlobalTenants { get; set; }
        public IDbSet<GlobalDB> GlobalDBs { get; set; }
        public IDbSet<ConvertProgramInfo> ConvertProgramInfoes { get; set; }
        public IDbSet<PerformanceLog> PerformanceLogs { get; set; }
        public IDbSet<LogitudeLead> LogitudeLeads { get; set; }
        public IDbSet<TenantManagement> TenantManagements { get; set; }
        public IDbSet<AnalyzeQueue> AnalyzeQueues { get; set; }
        public IDbSet<AnalyzeQueueStatus> AnalyzeQueueStatus { get; set; }
        public IDbSet<RecurringPeriod> RecurringPeriods { get; set; }
        public IDbSet<PaymentChannel> PaymentChannels { get; set; }
        public IDbSet<PaymentMethod> PaymentMethods { get; set; }
        public IDbSet<ContactPassword> ContactPasswords { get; set; }
        public IDbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
        public IDbSet<PaymentCurrency> PaymentCurrencies { get; set; }
        public IDbSet<BluesnapContract> BluesnapContracts { get; set; }
        public IDbSet<BluesnapTransaction> BluesnapTransactions { get; set; }
        public IDbSet<BluesnapContractType> BluesnapContractTypes { get; set; }
        public IDbSet<AutoSignupEmail> AutoSignupEmails { get; set; }
        public IDbSet<AWBMessagesCCSType> AWBMessagesCCSTypes { get; set; }
        public IDbSet<SystemMetadataLastUpdate> SystemMetadataLastUpdates { get; set; }
        public IDbSet<MonitorServiceLastUpdate> MonitorServiceLastUpdates { get; set; }
        public IDbSet<HelpResource> HelpResources { get; set; }
        public IDbSet<BatchServicesDefinition> BatchServicesDefinitions { get; set; }
        public IDbSet<ChangePasswordLog> ChangePasswordLogs { get; set; }
        public IDbSet<TenantType> TenantTypes { get; set; }
        public IDbSet<ApiCredintials> ApiCredintials { get; set; }
        public IDbSet<TenantManagementLicense> TenantManagementLicenses { get; set; }
        public IDbSet<TenantAddOn> TenantAddOns { get; set; }
        public IDbSet<BatchServicesDefinitionMods> BatchServicesDefinitionMods { get; set; }
        public IDbSet<OneTimePassword> OneTimePasswords { get; set; }
        public IDbSet<TenantManagmentPrivateLabels> TenantManagmentPrivateLabels { get; set; }

        public IDbSet<AgentSharedLogisticsKey> AgentSharedLogisticsKeys { get; set; }
        public IDbSet<SessionPolicy> SessionPolicies { get; set; }
        public IDbSet<CaptchaKey> CaptchaKeys { get; set; }
        public IDbSet<InvalidEmailResetPassword> InvalidEmailResetPasswords { get; set; }
        public IDbSet<WebhookKeys> WebhookKeys { get; set; }



    }
}