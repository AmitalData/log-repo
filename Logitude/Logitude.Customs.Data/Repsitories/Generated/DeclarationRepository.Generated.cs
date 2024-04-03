 
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
   public partial class DeclarationRepository:IRepository<Declaration>
   {
   
        private ICustomContext currentContext;
        public DeclarationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Declaration GetSingle(string id, int tenant)
        {
            return (from a in context.Declarations
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Declaration> GetAll(int tenant)
        {
            return from a in context.Declarations  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Declaration GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationKeys keys = entityKeys as DeclarationKeys;
            return (from a in context.Declarations
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Declaration entity)
        {
            onAdd();
            context.Declarations.Add(entity);
        }

        public void Remove(Declaration entity)
        {
            context.Declarations.Attach(entity);
            context.Declarations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Declaration entity)
        {
            onUpdate();
            context.Declarations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Declaration> All()
        {
            return context.Declarations.ToList();
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
	 