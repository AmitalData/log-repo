 
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
   public partial class DashboardRepository:IRepository<Dashboard>
   {
   
        private IDashboardContext currentContext;
        public DashboardRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public DashboardRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  Dashboard GetSingle(string id, int tenant)
        {
            return (from a in context.Dashboards
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Dashboard> GetAll(int tenant)
        {
            return from a in context.Dashboards  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Dashboard GetSingle(EntityKeyFields entityKeys)
        {
            DashboardKeys keys = entityKeys as DashboardKeys;
            return (from a in context.Dashboards
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Dashboard entity)
        {
            onAdd();
            context.Dashboards.Add(entity);
        }

        public void Remove(Dashboard entity)
        {
            context.Dashboards.Attach(entity);
            context.Dashboards.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Dashboard entity)
        {
            onUpdate();
            context.Dashboards.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Dashboard> All()
        {
            return context.Dashboards.ToList();
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
	 