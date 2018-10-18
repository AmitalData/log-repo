 
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
   public partial class GuaranteeConditionRepository:IRepository<GuaranteeCondition>
   {
   
        private ICustomContext currentContext;
        public GuaranteeConditionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public GuaranteeConditionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  GuaranteeCondition GetSingle(string id, int tenant)
        {
            return (from a in context.GuaranteeConditions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GuaranteeCondition> GetAll(int tenant)
        {
            return from a in context.GuaranteeConditions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GuaranteeCondition GetSingle(EntityKeyFields entityKeys)
        {
            GuaranteeConditionKeys keys = entityKeys as GuaranteeConditionKeys;
            return (from a in context.GuaranteeConditions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GuaranteeCondition entity)
        {
            onAdd();
            context.GuaranteeConditions.Add(entity);
        }

        public void Remove(GuaranteeCondition entity)
        {
            context.GuaranteeConditions.Attach(entity);
            context.GuaranteeConditions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GuaranteeCondition entity)
        {
            onUpdate();
            context.GuaranteeConditions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GuaranteeCondition> All()
        {
            return context.GuaranteeConditions.ToList();
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
	 