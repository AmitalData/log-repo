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
    public class AWBStocksDataViewContext : DbContext, IAWBStocksDataViewContext
    {
        public System.Data.Entity.IDbSet<EntityPOCOs.AWBStocksDataView> AWBStocksDataViews
        {
            get;
            set;
        }
                 
        public AWBStocksDataViewContext()
        {
            Database.SetInitializer<AWBStocksDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public AWBStocksDataViewContext(DbConnection connection)
            : base(connection,true)
        {
            Database.SetInitializer<AWBStocksDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public void SetAsModified(object entity)
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ShipmentsContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
            modelBuilder.Configurations.Add(new AWBStocksDataViewMap());
           
            base.OnModelCreating(modelBuilder);
        }

        public static IAWBStocksDataViewContext GetContext(int tenant)
        {
       
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            AWBStocksDataViewContext context = new AWBStocksDataViewContext(connection);
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
