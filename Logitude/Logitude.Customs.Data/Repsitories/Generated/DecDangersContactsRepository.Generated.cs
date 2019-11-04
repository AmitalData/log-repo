 
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
   public partial class DecDangersContactRepository:IRepository<DecDangersContact>
   {
   
        private ICustomContext currentContext;
        public DecDangersContactRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DecDangersContactRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DecDangersContact GetSingle(string declarationid, int tenant)
        {
            return (from a in context.DecDangersContacts
                    where a.DeclarationId == declarationid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DecDangersContact> GetAll(int tenant)
        {
            return from a in context.DecDangersContacts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DecDangersContact GetSingle(EntityKeyFields entityKeys)
        {
            DecDangersContactKeys keys = entityKeys as DecDangersContactKeys;
            return (from a in context.DecDangersContacts
                    where a.DeclarationId == keys.DeclarationId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DecDangersContact entity)
        {
            onAdd();
            context.DecDangersContacts.Add(entity);
        }

        public void Remove(DecDangersContact entity)
        {
            context.DecDangersContacts.Attach(entity);
            context.DecDangersContacts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DecDangersContact entity)
        {
            onUpdate();
            context.DecDangersContacts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DecDangersContact> All()
        {
            return context.DecDangersContacts.ToList();
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
	 