using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Simplog.Data.CommonDataModel
{
    public class CustomersDataViewContext : DbContext,ICustomersDataViewContext
    {

        public System.Data.Entity.IDbSet<EntityPOCOs.CustomersDataView> CustomersDataViews
        {
            get;
            set;
        }

         
        public CustomersDataViewContext()
        {
            Database.SetInitializer<CustomersDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public CustomersDataViewContext(DbConnection connection)
            : base(connection,true)
        {
            Database.SetInitializer<CustomersDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public void SetAsModified(object entity)
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CommonDataContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
            modelBuilder.Configurations.Add(new CustomersDataViewMap());
           
            base.OnModelCreating(modelBuilder);
        }

        public static ICustomersDataViewContext GetContext(int tenant)
        {
       
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            CustomersDataViewContext context = new CustomersDataViewContext(connection);
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
