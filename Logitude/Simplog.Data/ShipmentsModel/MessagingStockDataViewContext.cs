using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.ShipmentsModel
{
    public class MessagingStockDataViewContext : DbContext, IMessagingStockDataViewContext
    {
        public System.Data.Entity.IDbSet<EntityPOCOs.MessagingStockDataView> MessagingStockDataViews
        {
            get;
            set;
        }
                 
        public MessagingStockDataViewContext()
        {
            Database.SetInitializer<MessagingStockDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public MessagingStockDataViewContext(DbConnection connection)
            : base(connection,true)
        {
            Database.SetInitializer<MessagingStockDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public void SetAsModified(object entity)
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ShipmentsContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
            modelBuilder.Configurations.Add(new MessagingStockDataViewMap());
           
            base.OnModelCreating(modelBuilder);
        }

        public static IMessagingStockDataViewContext GetContext(int tenant)
        {
       
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            MessagingStockDataViewContext context = new MessagingStockDataViewContext(connection);
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
    }
}
