 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.DashboardModule.Data.Repositories
{
   public partial class AnalyticsFactsMetaDataRepository:IRepository<AnalyticsFactsMetaData>
   {
   
        private IDashboardContext currentContext;
        public AnalyticsFactsMetaDataRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public AnalyticsFactsMetaDataRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  AnalyticsFactsMetaData GetSingle(string id, int tenant)
        {
            return (from a in context.AnalyticsFactsMetaDatas
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AnalyticsFactsMetaData> GetAll(int tenant)
        {
            return from a in context.AnalyticsFactsMetaDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AnalyticsFactsMetaData GetSingle(EntityKeyFields entityKeys)
        {
            AnalyticsFactsMetaDataKeys keys = entityKeys as AnalyticsFactsMetaDataKeys;
            return (from a in context.AnalyticsFactsMetaDatas
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AnalyticsFactsMetaData entity)
        {
            onAdd();
            context.AnalyticsFactsMetaDatas.Add(entity);
        }

        public void Remove(AnalyticsFactsMetaData entity)
        {
            context.AnalyticsFactsMetaDatas.Attach(entity);
            context.AnalyticsFactsMetaDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AnalyticsFactsMetaData entity)
        {
            onUpdate();
            context.AnalyticsFactsMetaDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AnalyticsFactsMetaData> All()
        {
            return context.AnalyticsFactsMetaDatas.ToList();
        }

        private IDashboardContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 