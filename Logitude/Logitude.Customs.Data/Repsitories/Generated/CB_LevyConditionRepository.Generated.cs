 
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
   public partial class CB_LevyConditionRepository:IRepository<CB_LevyCondition>
   {
   
        private ICustomContext currentContext;
        public CB_LevyConditionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_LevyConditionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_LevyCondition GetSingle(string cb_id)
        {
            return (from a in context.CB_LevyConditions
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_LevyCondition> GetAll()
        {
            return from a in context.CB_LevyConditions  
                   select a;
        }
				 
        public CB_LevyCondition GetSingle(EntityKeyFields entityKeys)
        {
            CB_LevyConditionKeys keys = entityKeys as CB_LevyConditionKeys;
            return (from a in context.CB_LevyConditions
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_LevyCondition entity)
        {
            onAdd();
            context.CB_LevyConditions.Add(entity);
        }

        public void Remove(CB_LevyCondition entity)
        {
            context.CB_LevyConditions.Attach(entity);
            context.CB_LevyConditions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_LevyCondition entity)
        {
            onUpdate();
            context.CB_LevyConditions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_LevyCondition> All()
        {
            return context.CB_LevyConditions.ToList();
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
	 