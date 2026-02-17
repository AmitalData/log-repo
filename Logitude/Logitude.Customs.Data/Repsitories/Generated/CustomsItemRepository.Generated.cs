 
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
   public partial class CustomsItemRepository:IRepository<CustomsItem>
   {
   
        private ICustomContext currentContext;
        public CustomsItemRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsItemRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsItem GetSingle(string id)
        {
            return (from a in context.CustomsItems
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsItem> GetAll()
        {
            return from a in context.CustomsItems  
                   select a;
        }
				 
        public CustomsItem GetSingle(EntityKeyFields entityKeys)
        {
            CustomsItemKeys keys = entityKeys as CustomsItemKeys;
            return (from a in context.CustomsItems
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsItem entity)
        {
            onAdd();
            context.CustomsItems.Add(entity);
        }

        public void Remove(CustomsItem entity)
        {
            context.CustomsItems.Attach(entity);
            context.CustomsItems.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsItem entity)
        {
            onUpdate();
            context.CustomsItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsItem> All()
        {
            return context.CustomsItems.ToList();
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
	 