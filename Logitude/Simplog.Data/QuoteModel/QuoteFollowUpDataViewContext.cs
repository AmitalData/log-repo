using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Mapping;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.QuoteModel
{
    public class QuoteFollowUpDataViewContext:DbContext,IQuoteFollowUpDataViewContext
    {
       

        public QuoteFollowUpDataViewContext(DbConnection connection)
            : base(connection,true)
        {
            Database.SetInitializer<QuoteFollowUpDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public IDbSet<QuoteFollowUpDataView> QuoteFollowUpDataViews
        {
            get; set;
        }

        public void SetAsModified(object entity)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<QuoteFollowUpDataViewContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
            //string databasename = DatabaseInitializer.GetDatabaseName();
            //Database.DefaultConnectionFactory.CreateConnection(databasename);


            modelBuilder.Configurations.Add(new QuoteFollowUpDataViewMap());
            base.OnModelCreating(modelBuilder);
        }

        public static IQuoteFollowUpDataViewContext GetContext(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            QuoteFollowUpDataViewContext context = new QuoteFollowUpDataViewContext(connection);
            return context;

        }

     


        public void DetectChanges()
        {
            throw new NotImplementedException();
        }
    }
}
