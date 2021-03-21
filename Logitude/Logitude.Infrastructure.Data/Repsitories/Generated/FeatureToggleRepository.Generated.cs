 
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
   public partial class FeatureToggleRepository:IRepository<FeatureToggle>
   {
   
        private IInfrastructureContext currentContext;
        public FeatureToggleRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public FeatureToggleRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  FeatureToggle GetSingle(string id, int tenant)
        {
            return (from a in context.FeatureToggles
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<FeatureToggle> GetAll(int tenant)
        {
            return from a in context.FeatureToggles  
                   where a.Tenant == tenant || (tenant >= a.FromTenantNumber && tenant <= a.ToTenantNumber)
                   select a;
        }
				 
        public FeatureToggle GetSingle(EntityKeyFields entityKeys)
        {
            FeatureToggleKeys keys = entityKeys as FeatureToggleKeys;
            return (from a in context.FeatureToggles
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FeatureToggle entity)
        {
            onAdd();
            context.FeatureToggles.Add(entity);
        }

        public void Remove(FeatureToggle entity)
        {
            context.FeatureToggles.Attach(entity);
            context.FeatureToggles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FeatureToggle entity)
        {
            onUpdate();
            context.FeatureToggles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FeatureToggle> All()
        {
            return context.FeatureToggles.ToList();
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
	 