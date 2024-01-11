 
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
   public partial class DepositConditionRepository:IRepository<DepositCondition>
   {
   
        private ICustomContext currentContext;
        public DepositConditionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DepositConditionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DepositCondition GetSingle(string depositid, string depositconditioncode, int tenant)
        {
            return (from a in context.DepositConditions
                    where a.DepositId == depositid && a.DepositConditionCode == depositconditioncode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DepositCondition> GetAll(int tenant)
        {
            return from a in context.DepositConditions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DepositCondition GetSingle(EntityKeyFields entityKeys)
        {
            DepositConditionKeys keys = entityKeys as DepositConditionKeys;
            return (from a in context.DepositConditions
                    where a.DepositId == keys.DepositId && a.DepositConditionCode == keys.DepositConditionCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DepositCondition entity)
        {
            onAdd();
            context.DepositConditions.Add(entity);
        }

        public void Remove(DepositCondition entity)
        {
            context.DepositConditions.Attach(entity);
            context.DepositConditions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DepositCondition entity)
        {
            onUpdate();
            context.DepositConditions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DepositCondition> All()
        {
            return context.DepositConditions.ToList();
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
	 