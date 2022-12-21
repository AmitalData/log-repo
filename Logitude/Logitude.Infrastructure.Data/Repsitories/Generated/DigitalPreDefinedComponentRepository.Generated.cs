 
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
   public partial class DigitalPreDefinedComponentRepository:IRepository<DigitalPreDefinedComponent>
   {
   
        private IInfrastructureContext currentContext;
        public DigitalPreDefinedComponentRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public DigitalPreDefinedComponentRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  DigitalPreDefinedComponent GetSingle(string id, int tenant)
        {
            return (from a in context.DigitalPreDefinedComponents
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DigitalPreDefinedComponent> GetAll(int tenant)
        {
            return from a in context.DigitalPreDefinedComponents  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DigitalPreDefinedComponent GetSingle(EntityKeyFields entityKeys)
        {
            DigitalPreDefinedComponentKeys keys = entityKeys as DigitalPreDefinedComponentKeys;
            return (from a in context.DigitalPreDefinedComponents
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DigitalPreDefinedComponent entity)
        {
            onAdd();
            context.DigitalPreDefinedComponents.Add(entity);
        }

        public void Remove(DigitalPreDefinedComponent entity)
        {
            context.DigitalPreDefinedComponents.Attach(entity);
            context.DigitalPreDefinedComponents.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DigitalPreDefinedComponent entity)
        {
            onUpdate();
            context.DigitalPreDefinedComponents.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DigitalPreDefinedComponent> All()
        {
            return context.DigitalPreDefinedComponents.ToList();
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
	 