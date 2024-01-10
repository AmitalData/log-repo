 
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
   public partial class CustomsItemCategoryRepository:IRepository<CustomsItemCategory>
   {
   
        private ICustomContext currentContext;
        public CustomsItemCategoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsItemCategoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsItemCategory GetSingle(string code)
        {
            return (from a in context.CustomsItemCategories
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsItemCategory> GetAll()
        {
            return from a in context.CustomsItemCategories  
                   select a;
        }
				 
        public CustomsItemCategory GetSingle(EntityKeyFields entityKeys)
        {
            CustomsItemCategoryKeys keys = entityKeys as CustomsItemCategoryKeys;
            return (from a in context.CustomsItemCategories
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsItemCategory entity)
        {
            onAdd();
            context.CustomsItemCategories.Add(entity);
        }

        public void Remove(CustomsItemCategory entity)
        {
            context.CustomsItemCategories.Attach(entity);
            context.CustomsItemCategories.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsItemCategory entity)
        {
            onUpdate();
            context.CustomsItemCategories.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsItemCategory> All()
        {
            return context.CustomsItemCategories.ToList();
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
	 