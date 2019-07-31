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


namespace Logitude.SystemLogs.OracleMigratrion
{
    public class SystemLogContext : DbContext, ISystemLogContext
    {
        public SystemLogContext()
            : base("LogitudeSystemLogsStr")
        {
            Database.SetInitializer<SystemLogContext>(new MigrateDatabaseToLatestVersion<SystemLogContext, Logitude.SystemLogs.OracleMigratrion.Migrations.Configuration>());
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
            string dbConnectionInfo = ConfigurationManager.ConnectionStrings["SystemLogsStr"].ConnectionString; ;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<ErrorLog> ErrorLogs { get; set; }
        public IDbSet<ContactActivityLog> ContactActivityLogs { get; set; }
        public IDbSet<BatchServicesLog> BatchServicesLogs
        {
            get;
            set;
        }

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
            return base.SaveChanges();
          
           
           //  return 1;

        }






      
    }


}
