using Simplog.Data.CommonDataModel;
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

namespace Logitude.Accounting.Data
{
    public class CashBookStatusChartDataViewContext : DbContext, ICashBookStatusChartDataViewContext
    {

        public System.Data.Entity.IDbSet<EntityPOCOs.CashBookStatusChartDataView> CashBookStatusChartDataViews
        {
            get;
            set;
        }


        public CashBookStatusChartDataViewContext()
        {
            Database.SetInitializer<CashBookStatusChartDataViewContext>(null);
        }

        public CashBookStatusChartDataViewContext(DbConnection connection)
            : base(connection, true)
        {
            Database.SetInitializer<CashBookStatusChartDataViewContext>(null);
        }

        public void SetAsModified(object entity)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AccountingContext>(null);

            modelBuilder.Configurations.Add(new CashBookStatusChartDataViewMap());

            base.OnModelCreating(modelBuilder);
        }

        public static ICashBookStatusChartDataViewContext GetContext(int tenant)
        {

            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            CashBookStatusChartDataViewContext context = new CashBookStatusChartDataViewContext(connection);
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
