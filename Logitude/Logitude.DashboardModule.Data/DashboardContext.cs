using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Data.Entity.ModelConfiguration.Conventions;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data; 
using Logitude.DashboardModule.Data.EntityMapping;

namespace Logitude.DashboardModule.Data
{
   public class DashboardContext: DbContextBase, IDashboardContext
    {
        public DashboardContext()
        {
            Database.SetInitializer<DashboardContext>(null); 
			Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
			
        }

        public DashboardContext(DbConnection conn)
            : base(conn,true)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.SetInitializer<DashboardContext>(null);
			Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();

        }

        public static IDashboardContext GetContext(int tenant)
        {           
            GlobalDB currentDb;
			currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            DashboardContext context = new DashboardContext(connection);
            return context;
        }
		public override LogitudeDBSchema LogitudeDBSchema
        {
            get { return Simplog.Server.Infrastructure.LogitudeDBSchema.LOGITUDE_MAIN; }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

		    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
                config.Workarounds.DisableQuoting = true;
                
            }

            Database.SetInitializer<DashboardContext>(null);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			
            modelBuilder.Configurations.Add(new AnalyticsFactsMetaDataMap());
				
						 


            base.OnModelCreating(modelBuilder);
        }

        public void SetAsModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        public void DetectChanges()
        {
            this.ChangeTracker.DetectChanges();
        }

        public int SaveChanges()
        {
		    DetectChanges();
		    return base.SaveChanges();
       
	    }

      
		public DbConnection GetConnection()
		{
			return this.Database.Connection;
		}

		public DbContext GetActiveDbContext()
		{
			return this;
		}
 

	 public IDbSet<AnalyticsFactsMetaData> AnalyticsFactsMetaDatas 
	 {
	      get; set;
	 
	 }
	  
 }


}