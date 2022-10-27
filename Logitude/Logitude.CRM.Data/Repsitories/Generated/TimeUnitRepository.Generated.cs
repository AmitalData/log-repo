 
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
   public partial class TimeUnitRepository:IRepository<TimeUnit>
   {
   
        private ICRMContext currentContext;
        public TimeUnitRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TimeUnitRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  TimeUnit GetSingle(string code)
        {
            return (from a in context.TimeUnits
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TimeUnit> GetAll()
        {
            return from a in context.TimeUnits  
                   select a;
        }
				 
        public TimeUnit GetSingle(EntityKeyFields entityKeys)
        {
            TimeUnitKeys keys = entityKeys as TimeUnitKeys;
            return (from a in context.TimeUnits
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TimeUnit entity)
        {
            onAdd();
            context.TimeUnits.Add(entity);
        }

        public void Remove(TimeUnit entity)
        {
            context.TimeUnits.Attach(entity);
            context.TimeUnits.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TimeUnit entity)
        {
            onUpdate();
            context.TimeUnits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TimeUnit> All()
        {
            return context.TimeUnits.ToList();
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
	 