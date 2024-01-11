 
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
   public partial class ValidCustomsItemRepository:IRepository<ValidCustomsItem>
   {
   
        private ICustomContext currentContext;
        public ValidCustomsItemRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ValidCustomsItemRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ValidCustomsItem GetSingle(string code)
        {
            return (from a in context.ValidCustomsItems
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ValidCustomsItem> GetAll()
        {
            return from a in context.ValidCustomsItems  
                   select a;
        }
				 
        public ValidCustomsItem GetSingle(EntityKeyFields entityKeys)
        {
            ValidCustomsItemKeys keys = entityKeys as ValidCustomsItemKeys;
            return (from a in context.ValidCustomsItems
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ValidCustomsItem entity)
        {
            onAdd();
            context.ValidCustomsItems.Add(entity);
        }

        public void Remove(ValidCustomsItem entity)
        {
            context.ValidCustomsItems.Attach(entity);
            context.ValidCustomsItems.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ValidCustomsItem entity)
        {
            onUpdate();
            context.ValidCustomsItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ValidCustomsItem> All()
        {
            return context.ValidCustomsItems.ToList();
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
	 