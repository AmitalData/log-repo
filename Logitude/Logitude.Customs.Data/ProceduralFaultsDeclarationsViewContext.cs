using Logitude.Customs.Data.EntityPOCOs;
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

namespace Logitude.Customs.Data
{
    public class ProceduralFaultsDeclarationsViewContext : DbContext, IProceduralFaultsDeclarationsViewContext
    {
        IDbSet<ProceduralFaultDeclarationView> proceduralFaultDeclarationViews;
        public ProceduralFaultsDeclarationsViewContext()
            
        {
            Database.SetInitializer<ProceduralFaultsDeclarationsViewContext>(null);
        }

        public ProceduralFaultsDeclarationsViewContext(DbConnection connection)
            : base(connection,true)
        {
            Database.SetInitializer<ProceduralFaultsDeclarationsViewContext>(null);
        }

        public IDbSet<ProceduralFaultDeclarationView> ProceduralFaultDeclarationViews
        {
           get; set;
        }

        public void SetAsModified(object entity)
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CustomContext>(null);

            base.OnModelCreating(modelBuilder);
           
          
        }

        public static IProceduralFaultsDeclarationsViewContext GetContext(int tenant)
        {
       
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            }
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            ProceduralFaultsDeclarationsViewContext context = new ProceduralFaultsDeclarationsViewContext(connection);
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
