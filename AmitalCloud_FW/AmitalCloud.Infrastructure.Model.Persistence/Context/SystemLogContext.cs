using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Model.EntityClasses;
using AmitalCloud.Infrastructure.Model.Enums;
using AmitalCloud.Infrastructure.Model.Interfaces;
using AmitalCloud.Infrastructure.Model.Persistence.BaseClasses;
using AmitalCloud.Infrastructure.Model.Persistence.Helpers;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Transactions;
namespace AmitalCloud.Infrastructure.Model.Persistence.Context

{
    public class SystemLogContext : DbContextBase, ISystemLogContext
    {
        private int tenant;
        private SystemLogContext()
            : base("AamitalSystemLogsStr")
        {
            Database.SetInitializer<SystemLogContext>(new MigrateDatabaseToLatestVersion<SystemLogContext, MigrationConfiguration<SystemLogContext>>());
        }
        //DbConnection dbConnection;
        private SystemLogContext(DbConnection connection)
            : base(connection, true)
        {
            InitializeContext();
            ////dbConnection = connection;
        }
        private SystemLogContext(string connection) : base(connection)
        {
            InitializeContext();
            //dbConnection = connection;
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
            dbConnectionInfo = DbContextBaseUtil.GetConnectionStringWithAmitalNetRole(dbConnectionInfo);
            if (AmitalCloudSettings.DatabaseManagementSystem == "oracle")
            {
                return new SystemLogContext(DatabaseInitializer.GetConnection(dbConnectionInfo, 5));
            }
            else
            {
                return new SystemLogContext(dbConnectionInfo);

            }
        }
        protected override AmitalCloudDBSchema AmitalCloudDBSchema
        {
            get { return AmitalCloudDBSchema.AMITAL_LOGS; }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<SystemLogContext>(null);
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
