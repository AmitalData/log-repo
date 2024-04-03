 
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
   public partial class CustomBankRepository:IRepository<CustomBank>
   {
   
        private ICustomContext currentContext;
        public CustomBankRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomBankRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomBank GetSingle(string id, int tenant)
        {
            return (from a in context.CustomBanks
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomBank> GetAll(int tenant)
        {
            return from a in context.CustomBanks  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomBank GetSingle(EntityKeyFields entityKeys)
        {
            CustomBankKeys keys = entityKeys as CustomBankKeys;
            return (from a in context.CustomBanks
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomBank entity)
        {
            onAdd();
            context.CustomBanks.Add(entity);
        }

        public void Remove(CustomBank entity)
        {
            context.CustomBanks.Attach(entity);
            context.CustomBanks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomBank entity)
        {
            onUpdate();
            context.CustomBanks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomBank> All()
        {
            return context.CustomBanks.ToList();
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
	 