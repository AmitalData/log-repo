 
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
   public partial class AnalyticsFactsFieldsMetaDataRepository:IRepository<AnalyticsFactsFieldsMetaData>
   {
   
        private IDashboardContext currentContext;
        public AnalyticsFactsFieldsMetaDataRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public AnalyticsFactsFieldsMetaDataRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  AnalyticsFactsFieldsMetaData GetSingle(string id, int tenant)
        {
            return (from a in context.AnalyticsFactsFieldsMetaDatas
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AnalyticsFactsFieldsMetaData> GetAll(int tenant)
        {
            return from a in context.AnalyticsFactsFieldsMetaDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AnalyticsFactsFieldsMetaData GetSingle(EntityKeyFields entityKeys)
        {
            AnalyticsFactsFieldsMetaDataKeys keys = entityKeys as AnalyticsFactsFieldsMetaDataKeys;
            return (from a in context.AnalyticsFactsFieldsMetaDatas
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AnalyticsFactsFieldsMetaData entity)
        {
            onAdd();
            context.AnalyticsFactsFieldsMetaDatas.Add(entity);
        }

        public void Remove(AnalyticsFactsFieldsMetaData entity)
        {
            context.AnalyticsFactsFieldsMetaDatas.Attach(entity);
            context.AnalyticsFactsFieldsMetaDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AnalyticsFactsFieldsMetaData entity)
        {
            onUpdate();
            context.AnalyticsFactsFieldsMetaDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AnalyticsFactsFieldsMetaData> All()
        {
            return context.AnalyticsFactsFieldsMetaDatas.ToList();
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
	 