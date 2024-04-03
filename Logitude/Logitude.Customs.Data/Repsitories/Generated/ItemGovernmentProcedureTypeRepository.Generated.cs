 
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
   public partial class ItemGovernmentProcedureTypeRepository:IRepository<ItemGovernmentProcedureType>
   {
   
        private ICustomContext currentContext;
        public ItemGovernmentProcedureTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ItemGovernmentProcedureTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ItemGovernmentProcedureType GetSingle(string code)
        {
            return (from a in context.ItemGovernmentProcedureTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ItemGovernmentProcedureType> GetAll()
        {
            return from a in context.ItemGovernmentProcedureTypes  
                   select a;
        }
				 
        public ItemGovernmentProcedureType GetSingle(EntityKeyFields entityKeys)
        {
            ItemGovernmentProcedureTypeKeys keys = entityKeys as ItemGovernmentProcedureTypeKeys;
            return (from a in context.ItemGovernmentProcedureTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ItemGovernmentProcedureType entity)
        {
            onAdd();
            context.ItemGovernmentProcedureTypes.Add(entity);
        }

        public void Remove(ItemGovernmentProcedureType entity)
        {
            context.ItemGovernmentProcedureTypes.Attach(entity);
            context.ItemGovernmentProcedureTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ItemGovernmentProcedureType entity)
        {
            onUpdate();
            context.ItemGovernmentProcedureTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ItemGovernmentProcedureType> All()
        {
            return context.ItemGovernmentProcedureTypes.ToList();
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
	 