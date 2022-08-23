 
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
   public partial class WidgetTypeRepository:IRepository<WidgetType>
   {
   
        private IInfrastructureContext currentContext;
        public WidgetTypeRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public WidgetTypeRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  WidgetType GetSingle(string code)
        {
            return (from a in context.WidgetTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<WidgetType> GetAll()
        {
            return from a in context.WidgetTypes  
                   select a;
        }
				 
        public WidgetType GetSingle(EntityKeyFields entityKeys)
        {
            WidgetTypeKeys keys = entityKeys as WidgetTypeKeys;
            return (from a in context.WidgetTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WidgetType entity)
        {
            onAdd();
            context.WidgetTypes.Add(entity);
        }

        public void Remove(WidgetType entity)
        {
            context.WidgetTypes.Attach(entity);
            context.WidgetTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WidgetType entity)
        {
            onUpdate();
            context.WidgetTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WidgetType> All()
        {
            return context.WidgetTypes.ToList();
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
	 