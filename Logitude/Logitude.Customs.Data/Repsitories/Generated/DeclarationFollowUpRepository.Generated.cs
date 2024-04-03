 
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
   public partial class DeclarationFollowUpRepository:IRepository<DeclarationFollowUp>
   {
   
        private ICustomContext currentContext;
        public DeclarationFollowUpRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationFollowUpRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationFollowUp GetSingle(string id, int tenant)
        {
            return (from a in context.DeclarationFollowUps
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationFollowUp> GetAll(int tenant)
        {
            return from a in context.DeclarationFollowUps  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationFollowUp GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationFollowUpKeys keys = entityKeys as DeclarationFollowUpKeys;
            return (from a in context.DeclarationFollowUps
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationFollowUp entity)
        {
            onAdd();
            context.DeclarationFollowUps.Add(entity);
        }

        public void Remove(DeclarationFollowUp entity)
        {
            context.DeclarationFollowUps.Attach(entity);
            context.DeclarationFollowUps.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationFollowUp entity)
        {
            onUpdate();
            context.DeclarationFollowUps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationFollowUp> All()
        {
            return context.DeclarationFollowUps.ToList();
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
	 