 
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
   public partial class DigitalFieldSecurityRepository:IRepository<DigitalFieldSecurity>
   {
   
        private IInfrastructureContext currentContext;
        public DigitalFieldSecurityRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public DigitalFieldSecurityRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  DigitalFieldSecurity GetSingle(string id, int tenant)
        {
            return (from a in context.DigitalFieldSecurities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DigitalFieldSecurity> GetAll(int tenant)
        {
            return from a in context.DigitalFieldSecurities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DigitalFieldSecurity GetSingle(EntityKeyFields entityKeys)
        {
            DigitalFieldSecurityKeys keys = entityKeys as DigitalFieldSecurityKeys;
            return (from a in context.DigitalFieldSecurities
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DigitalFieldSecurity entity)
        {
            onAdd();
            context.DigitalFieldSecurities.Add(entity);
        }

        public void Remove(DigitalFieldSecurity entity)
        {
            context.DigitalFieldSecurities.Attach(entity);
            context.DigitalFieldSecurities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DigitalFieldSecurity entity)
        {
            onUpdate();
            context.DigitalFieldSecurities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DigitalFieldSecurity> All()
        {
            return context.DigitalFieldSecurities.ToList();
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
	 