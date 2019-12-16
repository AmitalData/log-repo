 
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
   public partial class PriceStepsRepository:IRepository<PriceSteps>
   {
   
        private IInfrastructureContext currentContext;
        public PriceStepsRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public PriceStepsRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  PriceSteps GetSingle(string id, int tenant)
        {
            return (from a in context.PricesSteps
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PriceSteps> GetAll(int tenant)
        {
            return from a in context.PricesSteps  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PriceSteps GetSingle(EntityKeyFields entityKeys)
        {
            PriceStepsKeys keys = entityKeys as PriceStepsKeys;
            return (from a in context.PricesSteps
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PriceSteps entity)
        {
            onAdd();
            context.PricesSteps.Add(entity);
        }

        public void Remove(PriceSteps entity)
        {
            context.PricesSteps.Attach(entity);
            context.PricesSteps.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PriceSteps entity)
        {
            onUpdate();
            context.PricesSteps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PriceSteps> All()
        {
            return context.PricesSteps.ToList();
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
	 