 
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
   public partial class DigitalProfileRepository:IRepository<DigitalProfile>
   {
   
        private IInfrastructureContext currentContext;
        public DigitalProfileRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public DigitalProfileRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  DigitalProfile GetSingle(string id, int tenant)
        {
            return (from a in context.DigitalProfiles
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DigitalProfile> GetAll(int tenant)
        {
            return from a in context.DigitalProfiles  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DigitalProfile GetSingle(EntityKeyFields entityKeys)
        {
            DigitalProfileKeys keys = entityKeys as DigitalProfileKeys;
            return (from a in context.DigitalProfiles
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DigitalProfile entity)
        {
            onAdd();
            context.DigitalProfiles.Add(entity);
        }

        public void Remove(DigitalProfile entity)
        {
            context.DigitalProfiles.Attach(entity);
            context.DigitalProfiles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DigitalProfile entity)
        {
            onUpdate();
            context.DigitalProfiles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DigitalProfile> All()
        {
            return context.DigitalProfiles.ToList();
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
	 