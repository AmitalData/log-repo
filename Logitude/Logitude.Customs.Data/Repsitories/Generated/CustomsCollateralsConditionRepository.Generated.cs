 
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
   public partial class CustomsCollateralsConditionRepository:IRepository<CustomsCollateralsCondition>
   {
   
        private ICustomContext currentContext;
        public CustomsCollateralsConditionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsCollateralsConditionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsCollateralsCondition GetSingle(string customscollateralid, string conditioncode, int tenant)
        {
            return (from a in context.CustomsCollateralsConditions
                    where a.CustomsCollateralId == customscollateralid && a.ConditionCode == conditioncode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsCollateralsCondition> GetAll(int tenant)
        {
            return from a in context.CustomsCollateralsConditions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsCollateralsCondition GetSingle(EntityKeyFields entityKeys)
        {
            CustomsCollateralsConditionKeys keys = entityKeys as CustomsCollateralsConditionKeys;
            return (from a in context.CustomsCollateralsConditions
                    where a.CustomsCollateralId == keys.CustomsCollateralId && a.ConditionCode == keys.ConditionCode
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsCollateralsCondition entity)
        {
            onAdd();
            context.CustomsCollateralsConditions.Add(entity);
        }

        public void Remove(CustomsCollateralsCondition entity)
        {
            context.CustomsCollateralsConditions.Attach(entity);
            context.CustomsCollateralsConditions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsCollateralsCondition entity)
        {
            onUpdate();
            context.CustomsCollateralsConditions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsCollateralsCondition> All()
        {
            return context.CustomsCollateralsConditions.ToList();
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
	 