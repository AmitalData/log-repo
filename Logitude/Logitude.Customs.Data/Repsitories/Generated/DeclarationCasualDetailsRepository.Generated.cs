 
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
   public partial class DeclarationCasualDetailsRepository:IRepository<DeclarationCasualDetails>
   {
   
        private ICustomContext currentContext;
        public DeclarationCasualDetailsRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationCasualDetailsRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationCasualDetails GetSingle(string declarationid, int tenant)
        {
            return (from a in context.DeclarationCasualDetailses
                    where a.DeclarationId == declarationid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationCasualDetails> GetAll(int tenant)
        {
            return from a in context.DeclarationCasualDetailses  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationCasualDetails GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationCasualDetailsKeys keys = entityKeys as DeclarationCasualDetailsKeys;
            return (from a in context.DeclarationCasualDetailses
                    where a.DeclarationId == keys.DeclarationId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationCasualDetails entity)
        {
            onAdd();
            context.DeclarationCasualDetailses.Add(entity);
        }

        public void Remove(DeclarationCasualDetails entity)
        {
            context.DeclarationCasualDetailses.Attach(entity);
            context.DeclarationCasualDetailses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationCasualDetails entity)
        {
            onUpdate();
            context.DeclarationCasualDetailses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationCasualDetails> All()
        {
            return context.DeclarationCasualDetailses.ToList();
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
	 