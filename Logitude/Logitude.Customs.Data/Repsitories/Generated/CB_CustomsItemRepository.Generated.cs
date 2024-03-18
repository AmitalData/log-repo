 
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
   public partial class CB_CustomsItemRepository:IRepository<CB_CustomsItem>
   {
   
        private ICustomContext currentContext;
        public CB_CustomsItemRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CustomsItemRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_CustomsItem GetSingle(int id)
        {
            return (from a in context.CB_CustomsItems
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_CustomsItem> GetAll()
        {
            return from a in context.CB_CustomsItems  
                   select a;
        }
				 
        public CB_CustomsItem GetSingle(EntityKeyFields entityKeys)
        {
            CB_CustomsItemKeys keys = entityKeys as CB_CustomsItemKeys;
            return (from a in context.CB_CustomsItems
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_CustomsItem entity)
        {
            onAdd();
            context.CB_CustomsItems.Add(entity);
        }

        public void Remove(CB_CustomsItem entity)
        {
            context.CB_CustomsItems.Attach(entity);
            context.CB_CustomsItems.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_CustomsItem entity)
        {
            onUpdate();
            context.CB_CustomsItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_CustomsItem> All()
        {
            return context.CB_CustomsItems.ToList();
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
	 