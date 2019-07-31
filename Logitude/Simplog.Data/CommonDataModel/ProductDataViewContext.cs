using Simplog.Data.CommonDataModel.EntityPOCOs;
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
    public class ProductDataViewContext : DbContext, IProductDataViewContext
    {

        IDbSet<ProductTypeModificationView> productDataViews;
        public ProductDataViewContext()
            
        {
            Database.SetInitializer<ProductDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public ProductDataViewContext(DbConnection connection)
            : base(connection,true)
        {
            Database.SetInitializer<ProductDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public IDbSet<ProductTypeModificationView> ProductDataViews
        {
           get; set;
        }

        public void SetAsModified(object entity)
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CommonDataContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
            //string databasename = DatabaseInitializer.GetDatabaseName();
            //Database.DefaultConnectionFactory.CreateConnection(databasename);

            modelBuilder.Configurations.Add(new ProductTypeModificationViewMap());
           
            base.OnModelCreating(modelBuilder);
        }

        public static IProductDataViewContext GetContext(int tenant)
        {
       
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            ProductDataViewContext context = new ProductDataViewContext(connection);
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
