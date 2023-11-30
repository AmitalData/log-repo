 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class ActivityTimeTypeRepository:IRepository<ActivityTimeType>
   {
   
        private ICRMContext currentContext;
        public ActivityTimeTypeRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public ActivityTimeTypeRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  ActivityTimeType GetSingle(string code)
        {
            return (from a in context.ActivityTimeTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ActivityTimeType> GetAll()
        {
            return from a in context.ActivityTimeTypes  
                   select a;
        }
				 
        public ActivityTimeType GetSingle(EntityKeyFields entityKeys)
        {
            ActivityTimeTypeKeys keys = entityKeys as ActivityTimeTypeKeys;
            return (from a in context.ActivityTimeTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ActivityTimeType entity)
        {
            onAdd();
            context.ActivityTimeTypes.Add(entity);
        }

        public void Remove(ActivityTimeType entity)
        {
            context.ActivityTimeTypes.Attach(entity);
            context.ActivityTimeTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ActivityTimeType entity)
        {
            onUpdate();
            context.ActivityTimeTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ActivityTimeType> All()
        {
            return context.ActivityTimeTypes.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 