 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class WidgetMeasureRepository:IRepository<WidgetMeasure>
   {
   
        private IInfrastructureContext currentContext;
        public WidgetMeasureRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public WidgetMeasureRepository(IInfrastructureContext context)
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

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 