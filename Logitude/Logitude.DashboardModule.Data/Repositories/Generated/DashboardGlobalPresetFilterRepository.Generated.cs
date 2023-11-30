 
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
   public partial class DashboardGlobalPresetFilterRepository:IRepository<DashboardGlobalPresetFilter>
   {
   
        private IDashboardContext currentContext;
        public DashboardGlobalPresetFilterRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public DashboardGlobalPresetFilterRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  DashboardGlobalPresetFilter GetSingle(string code)
        {
            return (from a in context.DashboardGlobalPresetFilters
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DashboardGlobalPresetFilter> GetAll()
        {
            return from a in context.DashboardGlobalPresetFilters  
                   select a;
        }
				 
        public DashboardGlobalPresetFilter GetSingle(EntityKeyFields entityKeys)
        {
            DashboardGlobalPresetFilterKeys keys = entityKeys as DashboardGlobalPresetFilterKeys;
            return (from a in context.DashboardGlobalPresetFilters
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DashboardGlobalPresetFilter entity)
        {
            onAdd();
            context.DashboardGlobalPresetFilters.Add(entity);
        }

        public void Remove(DashboardGlobalPresetFilter entity)
        {
            context.DashboardGlobalPresetFilters.Attach(entity);
            context.DashboardGlobalPresetFilters.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DashboardGlobalPresetFilter entity)
        {
            onUpdate();
            context.DashboardGlobalPresetFilters.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DashboardGlobalPresetFilter> All()
        {
            return context.DashboardGlobalPresetFilters.ToList();
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
	 