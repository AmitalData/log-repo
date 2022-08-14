 
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
using System.Transactions;
using System.Data.Entity.Validation;
using System.Data.Entity.ModelConfiguration.Conventions;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.InfrastructureModel.Mapping;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Data.QuoteModel.Mapping;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data; 
using Logitude.Workflow.Data.EntityMapping;

namespace Logitude.Workflow.Data
{
   public class WorkflowContext: DbContextBase, IWorkflowContext
    {
        public WorkflowContext()
        {
            Database.SetInitializer<WorkflowContext>(null);   
			Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public WorkflowContext(DbConnection conn)
            : base(conn,true)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.SetInitializer<WorkflowContext>(null);
			Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public static IWorkflowContext GetContext(int tenant)
        {           
            GlobalDB currentDb;
            currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            WorkflowContext context = new WorkflowContext(connection);
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
            Database.SetInitializer<WorkflowContext>(null);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			
            modelBuilder.Configurations.Add(new WorkFlowMap());
				

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

 

	 public IDbSet<WorkFlow> WorkFlows 
	 {
	      get; set;
	 
	 }
	  
 }


}