 
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
   public partial class DashboardGlobalFilterRepository:IRepository<DashboardGlobalFilter>
   {
   
        private IDashboardContext currentContext;
        public DashboardGlobalFilterRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public DashboardGlobalFilterRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  DashboardGlobalFilter GetSingle(string id, int tenant)
        {
            return (from a in context.DashboardGlobalFilters
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DashboardGlobalFilter> GetAll(int tenant)
        {
            return from a in context.DashboardGlobalFilters  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DashboardGlobalFilter GetSingle(EntityKeyFields entityKeys)
        {
            DashboardGlobalFilterKeys keys = entityKeys as DashboardGlobalFilterKeys;
            return (from a in context.DashboardGlobalFilters
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DashboardGlobalFilter entity)
        {
            onAdd();
            context.DashboardGlobalFilters.Add(entity);
        }

        public void Remove(DashboardGlobalFilter entity)
        {
            context.DashboardGlobalFilters.Attach(entity);
            context.DashboardGlobalFilters.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DashboardGlobalFilter entity)
        {
            onUpdate();
            context.DashboardGlobalFilters.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DashboardGlobalFilter> All()
        {
            return context.DashboardGlobalFilters.ToList();
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
	 