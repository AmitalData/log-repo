 
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
   public partial class WidgetRepository:IRepository<Widget>
   {
   
        private IInfrastructureContext currentContext;
        public WidgetRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public WidgetRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  Widget GetSingle(string id, int tenant)
        {
            return (from a in context.Widgets
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Widget> GetAll(int tenant)
        {
            return from a in context.Widgets  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Widget GetSingle(EntityKeyFields entityKeys)
        {
            WidgetKeys keys = entityKeys as WidgetKeys;
            return (from a in context.Widgets
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Widget entity)
        {
            onAdd();
            context.Widgets.Add(entity);
        }

        public void Remove(Widget entity)
        {
            context.Widgets.Attach(entity);
            context.Widgets.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Widget entity)
        {
            onUpdate();
            context.Widgets.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Widget> All()
        {
            return context.Widgets.ToList();
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
	 