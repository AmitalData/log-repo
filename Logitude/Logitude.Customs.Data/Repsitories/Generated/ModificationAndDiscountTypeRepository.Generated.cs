 
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
   public partial class ModificationAndDiscountTypeRepository:IRepository<ModificationAndDiscountType>
   {
   
        private ICustomContext currentContext;
        public ModificationAndDiscountTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ModificationAndDiscountTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ModificationAndDiscountType GetSingle(string code)
        {
            return (from a in context.ModificationAndDiscountTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ModificationAndDiscountType> GetAll()
        {
            return from a in context.ModificationAndDiscountTypes  
                   select a;
        }
				 
        public ModificationAndDiscountType GetSingle(EntityKeyFields entityKeys)
        {
            ModificationAndDiscountTypeKeys keys = entityKeys as ModificationAndDiscountTypeKeys;
            return (from a in context.ModificationAndDiscountTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ModificationAndDiscountType entity)
        {
            onAdd();
            context.ModificationAndDiscountTypes.Add(entity);
        }

        public void Remove(ModificationAndDiscountType entity)
        {
            context.ModificationAndDiscountTypes.Attach(entity);
            context.ModificationAndDiscountTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ModificationAndDiscountType entity)
        {
            onUpdate();
            context.ModificationAndDiscountTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ModificationAndDiscountType> All()
        {
            return context.ModificationAndDiscountTypes.ToList();
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
	 