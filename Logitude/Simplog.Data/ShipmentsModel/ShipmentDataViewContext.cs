using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Transactions;
using Simplog.Data.ShipmentModel.Mapping;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.ShipmentsModel
{
    public class ShipmentDataViewContext : DbContext, IShipmentDataViewContext
    {
        IDbSet<ShipmentDataView> shipmentDataViews;
        public ShipmentDataViewContext()
            
        {
            Database.SetInitializer<ShipmentDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public ShipmentDataViewContext(DbConnection connection)
            : base(connection,true)
        {
            Database.SetInitializer<ShipmentDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public IDbSet<ShipmentDataView> ShipmentDataViews
        {
           get; set;
        }

        public IDbSet<ShipmentCountryDashboardView> ShipmentCountryDashboardViews
        {
            get;
            set;
        }
        public IDbSet<ShipmentDirectionTransmodeView> ShipmentDirectionTransmodeViews
        {
            get;
            set;
        }
        public IDbSet<ShipmentsCustomersDashboardView> ShipmentsCustomersDashboardViews
        {
            get;
            set;
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

            modelBuilder.Configurations.Add(new ShipmentDataViewMap());
            modelBuilder.Configurations.Add(new ShipmentCountryDashboardViewMap());
            modelBuilder.Configurations.Add(new ShipmentDirectionTransmodeViewMap());
            modelBuilder.Configurations.Add(new ShipmentsCustomersDashboardViewMap());
           
            base.OnModelCreating(modelBuilder);
        }

        public static IShipmentDataViewContext GetContext(int tenant)
        {
       
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            ShipmentDataViewContext context = new ShipmentDataViewContext(connection);
            return context;

        }



        public void DetectChanges()
        {
           // throw new System.NotImplementedException();
        }


        public DbConnection GetConnection()
        {
            return this.Database.Connection;
        }

        public DbContext GetActiveDbContext()
        {
            return this;
        }
    }
}
