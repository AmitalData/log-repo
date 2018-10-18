using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Transactions;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InvoiceModel
{
    public class AgingReportInvoiceDataViewContext:DbContext,IAgingReportInvoiceDataViewContext
    {

         
         public AgingReportInvoiceDataViewContext()
        {
            Database.SetInitializer<AgingReportInvoiceDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

         public AgingReportInvoiceDataViewContext(DbConnection connection)
             : base(connection,true)
         {
             Database.SetInitializer<AgingReportInvoiceDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public IDbSet<AgingReportInvoiceDataView> AgingReportInvoiceDataViews
        {
            get; set;
        }

        public void SetAsModified(object entity)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<InvoiceContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
            //string databasename = DatabaseInitializer.GetDatabaseName();
            //Database.DefaultConnectionFactory.CreateConnection(databasename);

            modelBuilder.Configurations.Add(new AgingReportInvoiceDataViewMap());
            

            base.OnModelCreating(modelBuilder);
        }
        public static IAgingReportInvoiceDataViewContext GetContext(int tenant)
        {
            //GlobalDBRepository globalDbRep;
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                //GlobalDBRep = new GlobalDBRepository();
                //currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            AgingReportInvoiceDataViewContext context = new AgingReportInvoiceDataViewContext(connection);
            return context;

        }


        public void DetectChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}