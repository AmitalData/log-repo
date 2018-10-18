//using Logitude.Customs.Data.EntityPOCOs;
//using Simplog.Global.Data.GlobalModel.EntityPOCOs;
//using Simplog.Global.Data.GlobalModel.Helpers;
//using Simplog.Server.Infrastructure;
//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Transactions;

//namespace Logitude.Customs.Data
//{
//    public class ImporterDepositionVendorViewContext : DbContext, IImporterDepositionVendorViewContext
//    {

//        IDbSet<ImporterDepositionVendorsView> importerDepositionVendorsViews;
//        public ImporterDepositionVendorViewContext()
            
//        {
//            Database.SetInitializer<ImporterDepositionVendorViewContext>(null);
//        }

//        public ImporterDepositionVendorViewContext(DbConnection connection)
//            : base(connection,true)
//        {
//            Database.SetInitializer<ImporterDepositionVendorViewContext>(null);
//        }

//        public IDbSet<ImporterDepositionVendorsView> ImporterDepositionVendorsViews
//        {
//           get; set;
//        }

//        public void SetAsModified(object entity)
//        {
           
//        }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            Database.SetInitializer<CustomContext>(null);

//            base.OnModelCreating(modelBuilder);
           
          
//        }

//        public static IImporterDepositionVendorViewContext GetContext(int tenant)
//        {
       
//            GlobalDB currentDb;
//            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
//            {
//                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
//            }
//            string dbConnectionInfo = currentDb.DBConnection;
//            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
//            ImporterDepositionVendorViewContext context = new ImporterDepositionVendorViewContext(connection);
//            return context;

//        }



//        public void DetectChanges()
//        {
//           // throw new System.NotImplementedException();
//        }


//        public DbConnection GetConnection()
//        {
//            return this.Database.Connection;
//        }

//        public DbContext GetActiveDbContext()
//        {
//            return this;
//        }


    
//    }
//}
