 
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
   public partial class WidgetMeasureRepository:IRepository<WidgetMeasure>
   {
   
        private IDashboardContext currentContext;
        public WidgetMeasureRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public WidgetMeasureRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  WidgetMeasure GetSingle(string id, int tenant)
        {
            return (from a in context.WidgetMeasures
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WidgetMeasure> GetAll(int tenant)
        {
            return from a in context.WidgetMeasures  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WidgetMeasure GetSingle(EntityKeyFields entityKeys)
        {
            WidgetMeasureKeys keys = entityKeys as WidgetMeasureKeys;
            return (from a in context.WidgetMeasures
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WidgetMeasure entity)
        {
            onAdd();
            context.WidgetMeasures.Add(entity);
        }

        public void Remove(WidgetMeasure entity)
        {
            context.WidgetMeasures.Attach(entity);
            context.WidgetMeasures.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WidgetMeasure entity)
        {
            onUpdate();
            context.WidgetMeasures.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WidgetMeasure> All()
        {
            return context.WidgetMeasures.ToList();
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
	 