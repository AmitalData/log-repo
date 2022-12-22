 
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
   public partial class DigitalPortalScreenRepository:IRepository<DigitalPortalScreen>
   {
   
        private IInfrastructureContext currentContext;
        public DigitalPortalScreenRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public DigitalPortalScreenRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  DigitalPortalScreen GetSingle(string id, int tenant)
        {
            return (from a in context.DigitalPortalScreens
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DigitalPortalScreen> GetAll(int tenant)
        {
            return from a in context.DigitalPortalScreens  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DigitalPortalScreen GetSingle(EntityKeyFields entityKeys)
        {
            DigitalPortalScreenKeys keys = entityKeys as DigitalPortalScreenKeys;
            return (from a in context.DigitalPortalScreens
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DigitalPortalScreen entity)
        {
            onAdd();
            context.DigitalPortalScreens.Add(entity);
        }

        public void Remove(DigitalPortalScreen entity)
        {
            context.DigitalPortalScreens.Attach(entity);
            context.DigitalPortalScreens.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DigitalPortalScreen entity)
        {
            onUpdate();
            context.DigitalPortalScreens.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DigitalPortalScreen> All()
        {
            return context.DigitalPortalScreens.ToList();
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
	 