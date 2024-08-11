using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Data.Entity.Core.EntityClient;
using System.Transactions;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Common;
using Simplog.Data.ShipmentModel.Mapping;
using Simplog.Server.Infrastructure;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.ShipmentsModel
{
    public class LogBoxShipmentDataViewContext : DbContext, ILogBoxShipmentDataViewContext
    {
        public IDbSet<LogBoxShipmentDataView> LogBoxShipmentDataView { get; set; }
        public LogBoxShipmentDataViewContext()
        {
            Database.SetInitializer<LogBoxShipmentDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }
        public LogBoxShipmentDataViewContext(DbConnection connection)
            : base(connection, true)
        {
            Database.SetInitializer<LogBoxShipmentDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }


        public void SetAsModified(object entity)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ShipmentsContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();

            modelBuilder.Configurations.Add(new LogBoxShipmentDataViewMap());
            base.OnModelCreating(modelBuilder);
        }

        public static ILogBoxShipmentDataViewContext GetContext(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            LogBoxShipmentDataViewContext context = new LogBoxShipmentDataViewContext(connection);
            return context;

        }
        public void DetectChanges()
        {
        }
        public DbConnection GetConnection()
        {
            return this.Database.Connection;
        }

        public DbContext GetActiveDbContext()
        {
            return this;
        }
        public static ILogBoxShipmentDataViewContext GetSecContext(int tenant)
        {
            GlobalDB currentDb;
            currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbSeconderyConnectionInfo, null, null);
            LogBoxShipmentDataViewContext context = new LogBoxShipmentDataViewContext(connection);
            return context;
        }
    }
}
