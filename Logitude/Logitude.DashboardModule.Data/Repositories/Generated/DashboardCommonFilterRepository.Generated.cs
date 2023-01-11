 
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
   public partial class DashboardCommonFilterRepository:IRepository<DashboardCommonFilter>
   {
   
        private IDashboardContext currentContext;
        public DashboardCommonFilterRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public DashboardCommonFilterRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  DashboardCommonFilter GetSingle(string code)
        {
            return (from a in context.DashboardCommonFilters
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DashboardCommonFilter> GetAll()
        {
            return from a in context.DashboardCommonFilters  
                   select a;
        }
				 
        public DashboardCommonFilter GetSingle(EntityKeyFields entityKeys)
        {
            DashboardCommonFilterKeys keys = entityKeys as DashboardCommonFilterKeys;
            return (from a in context.DashboardCommonFilters
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DashboardCommonFilter entity)
        {
            onAdd();
            context.DashboardCommonFilters.Add(entity);
        }

        public void Remove(DashboardCommonFilter entity)
        {
            context.DashboardCommonFilters.Attach(entity);
            context.DashboardCommonFilters.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DashboardCommonFilter entity)
        {
            onUpdate();
            context.DashboardCommonFilters.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DashboardCommonFilter> All()
        {
            return context.DashboardCommonFilters.ToList();
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
	 