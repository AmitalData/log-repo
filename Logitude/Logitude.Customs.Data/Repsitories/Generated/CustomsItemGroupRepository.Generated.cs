 
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
   public partial class CustomsItemGroupRepository:IRepository<CustomsItemGroup>
   {
   
        private ICustomContext currentContext;
        public CustomsItemGroupRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsItemGroupRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsItemGroup GetSingle(string code)
        {
            return (from a in context.CustomsItemGroups
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsItemGroup> GetAll()
        {
            return from a in context.CustomsItemGroups  
                   select a;
        }
				 
        public CustomsItemGroup GetSingle(EntityKeyFields entityKeys)
        {
            CustomsItemGroupKeys keys = entityKeys as CustomsItemGroupKeys;
            return (from a in context.CustomsItemGroups
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsItemGroup entity)
        {
            onAdd();
            context.CustomsItemGroups.Add(entity);
        }

        public void Remove(CustomsItemGroup entity)
        {
            context.CustomsItemGroups.Attach(entity);
            context.CustomsItemGroups.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsItemGroup entity)
        {
            onUpdate();
            context.CustomsItemGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsItemGroup> All()
        {
            return context.CustomsItemGroups.ToList();
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
	 