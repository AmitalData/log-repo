using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.SystemLogs.Mapping;
using Logitude.SystemLogs.POCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Entity.ModelConfiguration.Conventions;
using Devart.Data.Oracle.Entity.Configuration;

namespace Logitude.SystemLogs
{
    public class SystemLogContext : DbContextBase, ISystemLogContext
    {
        public SystemLogContext()
            : base("LogitudeSystemLogsStr")
        {
            Database.SetInitializer<SystemLogContext>(new MigrateDatabaseToLatestVersion<SystemLogContext,Logitude.SystemLogs.Migrations.Configuration>());
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

        public static ISystemLogContext GetContext()
        {
            string dbConnectionInfo = "";
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
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
        public override LogitudeDBSchema LogitudeDBSchema
        {
            get { return Simplog.Server.Infrastructure.LogitudeDBSchema.LOGITUDE_LOGS; }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                var config = OracleEntityProviderConfig.Instance;
                config.Workarounds.DisableQuoting = true;
                //modelBuilder.SetDefaultSchema("LOGITUDE_LOGS");
            }

            Database.SetInitializer<SystemLogContext>(null);


            //bool exists = Database.CreateIfNotExists();
            //var configuration = new Logitude.SystemLogs.Migrations.Configuration();
            //configuration.AutomaticMigrationDataLossAllowed = true;
            //configuration.TargetDatabase = new DbConnectionInfo(dbConnection.ConnectionString, "System.Data.SqlClient");
            //var migrator = new DbMigrator(configuration);
            //migrator.Update();

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



        public void SetAsModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        public void DetectChanges()
        {
            this.ChangeTracker.DetectChanges();
        }
        public int SaveChanges()
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




    }


}
