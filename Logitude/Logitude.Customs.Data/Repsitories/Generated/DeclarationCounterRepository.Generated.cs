 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class DeclarationCounterRepository:IRepository<DeclarationCounter>
   {
   
        private ICustomContext currentContext;
        public DeclarationCounterRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationCounterRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationCounter GetSingle(string declarationid, int tenant)
        {
            return (from a in context.DeclarationCounters
                    where a.DeclarationId == declarationid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationCounter> GetAll(int tenant)
        {
            return from a in context.DeclarationCounters  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationCounter GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationCounterKeys keys = entityKeys as DeclarationCounterKeys;
            return (from a in context.DeclarationCounters
                    where a.DeclarationId == keys.DeclarationId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationCounter entity)
        {
            onAdd();
            context.DeclarationCounters.Add(entity);
        }

        public void Remove(DeclarationCounter entity)
        {
            context.DeclarationCounters.Attach(entity);
            context.DeclarationCounters.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationCounter entity)
        {
            onUpdate();
            context.DeclarationCounters.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationCounter> All()
        {
            return context.DeclarationCounters.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 