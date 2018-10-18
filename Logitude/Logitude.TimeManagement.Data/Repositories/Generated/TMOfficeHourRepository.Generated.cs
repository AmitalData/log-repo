 
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
   public partial class TMOfficeHourRepository:IRepository<TMOfficeHour>
   {
   
        private ITimeManagementContext currentContext;
        public TMOfficeHourRepository(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMOfficeHourRepository(ITimeManagementContext context)
        {
            currentContext = context;
        }

		 
		
		public  TMOfficeHour GetSingle(string id, int tenant)
        {
            return (from a in context.TMOfficeHours
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TMOfficeHour> GetAll(int tenant)
        {
            return from a in context.TMOfficeHours  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TMOfficeHour GetSingle(EntityKeyFields entityKeys)
        {
            TMOfficeHourKeys keys = entityKeys as TMOfficeHourKeys;
            return (from a in context.TMOfficeHours
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TMOfficeHour entity)
        {
            onAdd();
            context.TMOfficeHours.Add(entity);
        }

        public void Remove(TMOfficeHour entity)
        {
            context.TMOfficeHours.Attach(entity);
            context.TMOfficeHours.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TMOfficeHour entity)
        {
            onUpdate();
            context.TMOfficeHours.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TMOfficeHour> All()
        {
            return context.TMOfficeHours.ToList();
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
	 