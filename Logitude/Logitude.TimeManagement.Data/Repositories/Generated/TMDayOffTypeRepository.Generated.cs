 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.TimeManagement.Data.Repositories
{
   public partial class TMDayOffTypeRepository:IRepository<TMDayOffType>
   {
   
        private ITimeManagementContext currentContext;
        public TMDayOffTypeRepository(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMDayOffTypeRepository(ITimeManagementContext context)
        {
            currentContext = context;
        }

		 
		
		public  TMDayOffType GetSingle(string code)
        {
            return (from a in context.TMDayOffTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TMDayOffType> GetAll()
        {
            return from a in context.TMDayOffTypes  
                   select a;
        }
				 
        public TMDayOffType GetSingle(EntityKeyFields entityKeys)
        {
            TMDayOffTypeKeys keys = entityKeys as TMDayOffTypeKeys;
            return (from a in context.TMDayOffTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TMDayOffType entity)
        {
            onAdd();
            context.TMDayOffTypes.Add(entity);
        }

        public void Remove(TMDayOffType entity)
        {
            context.TMDayOffTypes.Attach(entity);
            context.TMDayOffTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TMDayOffType entity)
        {
            onUpdate();
            context.TMDayOffTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TMDayOffType> All()
        {
            return context.TMDayOffTypes.ToList();
        }

        private ITimeManagementContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 