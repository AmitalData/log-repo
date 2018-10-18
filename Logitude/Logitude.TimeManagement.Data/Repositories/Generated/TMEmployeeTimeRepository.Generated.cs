 
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
   public partial class TMEmployeeTimeRepository:IRepository<TMEmployeeTime>
   {
   
        private ITimeManagementContext currentContext;
        public TMEmployeeTimeRepository(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMEmployeeTimeRepository(ITimeManagementContext context)
        {
            currentContext = context;
        }

		 
		
		public  TMEmployeeTime GetSingle(string id, int tenant)
        {
            return (from a in context.TMEmployeeTimes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TMEmployeeTime> GetAll(int tenant)
        {
            return from a in context.TMEmployeeTimes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TMEmployeeTime GetSingle(EntityKeyFields entityKeys)
        {
            TMEmployeeTimeKeys keys = entityKeys as TMEmployeeTimeKeys;
            return (from a in context.TMEmployeeTimes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TMEmployeeTime entity)
        {
            onAdd();
            context.TMEmployeeTimes.Add(entity);
        }

        public void Remove(TMEmployeeTime entity)
        {
            context.TMEmployeeTimes.Attach(entity);
            context.TMEmployeeTimes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TMEmployeeTime entity)
        {
            onUpdate();
            context.TMEmployeeTimes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TMEmployeeTime> All()
        {
            return context.TMEmployeeTimes.ToList();
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
	 