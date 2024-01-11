 
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
   public partial class CustomsPartnersItemRepository:IRepository<CustomsPartnersItem>
   {
   
        private ICustomContext currentContext;
        public CustomsPartnersItemRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsPartnersItemRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsPartnersItem GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsPartnersItems
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsPartnersItem> GetAll(int tenant)
        {
            return from a in context.CustomsPartnersItems  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsPartnersItem GetSingle(EntityKeyFields entityKeys)
        {
            CustomsPartnersItemKeys keys = entityKeys as CustomsPartnersItemKeys;
            return (from a in context.CustomsPartnersItems
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsPartnersItem entity)
        {
            onAdd();
            context.CustomsPartnersItems.Add(entity);
        }

        public void Remove(CustomsPartnersItem entity)
        {
            context.CustomsPartnersItems.Attach(entity);
            context.CustomsPartnersItems.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsPartnersItem entity)
        {
            onUpdate();
            context.CustomsPartnersItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsPartnersItem> All()
        {
            return context.CustomsPartnersItems.ToList();
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
	 