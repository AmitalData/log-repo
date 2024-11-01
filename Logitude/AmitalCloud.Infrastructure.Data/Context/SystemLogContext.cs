using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.BaseClasses;
using AmitalCloud.Infrastructure.Domain.EntityMapping;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Transactions;
using AmitalCloud.Infrastructure.Data.DBHelpers;
using Devart.Data.Oracle.Entity.Configuration;
using AmitalCloud.Infrastructure.Data.Migrations;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Domain.Enums;
namespace AmitalCloud.Infrastructure.Data.Context

{
    public class SystemLogContext : DbContextBase, ISystemLogContext
    {
        private int tenant;
        public SystemLogContext()
            : base("LogitudeSystemLogsStr")
        {
            Database.SetInitializer<SystemLogContext>(new MigrateDatabaseToLatestVersion<SystemLogContext,MigrationConfiguration<SystemLogContext>>());
        }
        DbConnection dbConnection;
        public SystemLogContext(DbConnection connection)
            : base(connection, true)
        {
            InitializeContext();
            dbConnection = connection;
        }
        private void InitializeContext()
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.AutoDetectChangesEnabled = false;
        }
        public static ISystemLogContext GetContext(int tenant)
        {
            string dbConnectionInfo = "";
            if (AmitalCloudSettings.DatabaseManagementSystem == "oracle")
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Oracle_SystemLogsStr"].ConnectionString; ;
            }
            else
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["SystemLogsStr"].ConnectionString; ;
            }
            dbConnectionInfo =DbContextBaseUtil.GetConnectionStringWithAmitalNetRole(dbConnectionInfo);
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,5);
            SystemLogContext context = new SystemLogContext(connection);
            return context;
        }
        public static SystemLogContext GetContextByDBId(string dbId)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDBById(dbId);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            SystemLogContext context = new SystemLogContext(connection);
            return context;
        }
        public override AmitalCloudDBSchema AmitalCloudDBSchema
        {
            get { return AmitalCloudDBSchema.LOGITUDE_LOGS; }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (AmitalCloudSettings.DatabaseManagementSystem == "oracle")
            {
                var config = OracleEntityProviderConfig.Instance;
                config.Workarounds.DisableQuoting = true;
            }
            Database.SetInitializer<SystemLogContext>(null);
            modelBuilder.Configurations.Add(new ContactActivityLogMap());
            modelBuilder.Configurations.Add(new ErrorLogMap());
            modelBuilder.Configurations.Add(new BatchServicesLogMap());
            modelBuilder.Configurations.Add(new FailedLoginLogMap());
            modelBuilder.Configurations.Add(new FailedTokenLogMap());
            base.OnModelCreating(modelBuilder);
        }
        public IDbSet<ErrorLog> ErrorLogs { get; set; }
        public IDbSet<ContactActivityLog> ContactActivityLogs { get; set; }
        public IDbSet<BatchServicesLog> BatchServicesLogs { get; set; }
        public IDbSet<FailedLoginLog> FailedLoginLogs { get; set; }
        public IDbSet<FailedTokenLog> FailedTokenLogs { get; set; }

        public int Tenant => throw new NotImplementedException();

        public void SetAsModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }
        public void DetectChanges()
        {
            this.ChangeTracker.DetectChanges();
        }
        public override int SaveChanges()
        {
            DetectChanges();
            try
            {
                return base.SaveChanges();
            }
            catch (Exception e)
            {
            }
             return 1;
        }
        public DbConnection GetConnection()
        {
            return this.Database.Connection;
        }

        public DbContext GetActiveDbContext()
        {
            return this;
        }

        void IContext.Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
