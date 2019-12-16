 
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
   public partial class PriceStepRepository:IRepository<PriceStep>
   {
   
        private IInfrastructureContext currentContext;
        public PriceStepRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public PriceStepRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  PriceStep GetSingle(string id, int tenant)
        {
            return (from a in context.PriceSteps
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PriceStep> GetAll(int tenant)
        {
            return from a in context.PriceSteps  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PriceStep GetSingle(EntityKeyFields entityKeys)
        {
            PriceStepKeys keys = entityKeys as PriceStepKeys;
            return (from a in context.PriceSteps
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PriceStep entity)
        {
            onAdd();
            context.PriceSteps.Add(entity);
        }

        public void Remove(PriceStep entity)
        {
            context.PriceSteps.Attach(entity);
            context.PriceSteps.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PriceStep entity)
        {
            onUpdate();
            context.PriceSteps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PriceStep> All()
        {
            return context.PriceSteps.ToList();
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
	 