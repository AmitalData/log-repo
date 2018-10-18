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
    public class ShipmentFollowUpDataViewContext : DbContext, IShipmentFollowUpDataViewContext
    {
        IDbSet<ShipmentFollowUpDataView> shipmentFollowUpDataViews;

        public ShipmentFollowUpDataViewContext(DbConnection connection)
            : base(connection,true)
        {
            Database.SetInitializer<ShipmentFollowUpDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public IDbSet<ShipmentFollowUpDataView> ShipmentFollowUpDataViews
        {
            get; set;
        }

        public void SetAsModified(object entity)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ShipmentsContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
            //string databasename = DatabaseInitializer.GetDatabaseName();
            //Database.DefaultConnectionFactory.CreateConnection(databasename);


            modelBuilder.Configurations.Add(new ShipmentFollowUpDataViewMap());
            base.OnModelCreating(modelBuilder);
        }

        public static IShipmentFollowUpDataViewContext GetContext(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            ShipmentFollowUpDataViewContext context = new ShipmentFollowUpDataViewContext(connection);
            return context;

        }

     


        public void DetectChanges()
        {
            throw new NotImplementedException();
        }
    }
}
