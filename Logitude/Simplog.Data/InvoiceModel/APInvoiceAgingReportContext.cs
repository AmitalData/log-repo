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
    public class APInvoiceAgingReportContext:DbContext,IApInvoiceAgingReportContext
    {

        

        public APInvoiceAgingReportContext()
            
        {
            Database.SetInitializer<APInvoiceAgingReportContext>(null);
        }

        public APInvoiceAgingReportContext(DbConnection connection)
            : base(connection,true)
        {
            Database.SetInitializer<APInvoiceAgingReportContext>(null);
        }

        public IDbSet<APAgingReportDataView> AgingReportInvoiceDataViews
        {
            get; set;
        }

        public void SetAsModified(object entity)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<InvoiceContext>(null);
            //string databasename = DatabaseInitializer.GetDatabaseName();
            //Database.DefaultConnectionFactory.CreateConnection(databasename);
           
            modelBuilder.Configurations.Add(new APAgingReportDataViewMap());

            base.OnModelCreating(modelBuilder);
        }

        public static IApInvoiceAgingReportContext GetContext(int tenant)
        {
           
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
               
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            APInvoiceAgingReportContext context = new APInvoiceAgingReportContext(connection);
            return context;

        }





        public void DetectChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}
