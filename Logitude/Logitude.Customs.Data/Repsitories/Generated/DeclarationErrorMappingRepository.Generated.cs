 
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
   public partial class DeclarationErrorMappingRepository:IRepository<DeclarationErrorMapping>
   {
   
        private ICustomContext currentContext;
        public DeclarationErrorMappingRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationErrorMappingRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationErrorMapping GetSingle(string id)
        {
            return (from a in context.DeclarationErrorMappings
                    where a.Id == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationErrorMapping> GetAll()
        {
            return from a in context.DeclarationErrorMappings  
                   select a;
        }
				 
        public DeclarationErrorMapping GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationErrorMappingKeys keys = entityKeys as DeclarationErrorMappingKeys;
            return (from a in context.DeclarationErrorMappings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationErrorMapping entity)
        {
            onAdd();
            context.DeclarationErrorMappings.Add(entity);
        }

        public void Remove(DeclarationErrorMapping entity)
        {
            context.DeclarationErrorMappings.Attach(entity);
            context.DeclarationErrorMappings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationErrorMapping entity)
        {
            onUpdate();
            context.DeclarationErrorMappings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationErrorMapping> All()
        {
            return context.DeclarationErrorMappings.ToList();
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
	 