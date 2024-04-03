 
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
   public partial class ReturnConditionRepository:IRepository<ReturnCondition>
   {
   
        private ICustomContext currentContext;
        public ReturnConditionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ReturnConditionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReturnCondition GetSingle(string code)
        {
            return (from a in context.ReturnConditions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ReturnCondition> GetAll()
        {
            return from a in context.ReturnConditions  
                   select a;
        }
				 
        public ReturnCondition GetSingle(EntityKeyFields entityKeys)
        {
            ReturnConditionKeys keys = entityKeys as ReturnConditionKeys;
            return (from a in context.ReturnConditions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReturnCondition entity)
        {
            onAdd();
            context.ReturnConditions.Add(entity);
        }

        public void Remove(ReturnCondition entity)
        {
            context.ReturnConditions.Attach(entity);
            context.ReturnConditions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReturnCondition entity)
        {
            onUpdate();
            context.ReturnConditions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReturnCondition> All()
        {
            return context.ReturnConditions.ToList();
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
	 