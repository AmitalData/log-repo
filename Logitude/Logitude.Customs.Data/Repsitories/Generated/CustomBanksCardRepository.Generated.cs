 
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
   public partial class CustomBanksCardRepository:IRepository<CustomBanksCard>
   {
   
        private ICustomContext currentContext;
        public CustomBanksCardRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomBanksCardRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomBanksCard GetSingle(string id, int tenant)
        {
            return (from a in context.CustomBanksCards
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomBanksCard> GetAll(int tenant)
        {
            return from a in context.CustomBanksCards  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomBanksCard GetSingle(EntityKeyFields entityKeys)
        {
            CustomBanksCardKeys keys = entityKeys as CustomBanksCardKeys;
            return (from a in context.CustomBanksCards
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomBanksCard entity)
        {
            onAdd();
            context.CustomBanksCards.Add(entity);
        }

        public void Remove(CustomBanksCard entity)
        {
            context.CustomBanksCards.Attach(entity);
            context.CustomBanksCards.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomBanksCard entity)
        {
            onUpdate();
            context.CustomBanksCards.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomBanksCard> All()
        {
            return context.CustomBanksCards.ToList();
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
	 