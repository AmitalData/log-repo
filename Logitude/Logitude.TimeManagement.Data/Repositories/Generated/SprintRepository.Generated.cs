 
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
   public partial class SprintRepository:IRepository<Sprint>
   {
   
        private ITimeManagementContext currentContext;
        public SprintRepository(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public SprintRepository(ITimeManagementContext context)
        {
            currentContext = context;
        }

		 
		
		public  Sprint GetSingle(string id, int tenant)
        {
            return (from a in context.Sprints
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Sprint> GetAll(int tenant)
        {
            return from a in context.Sprints  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Sprint GetSingle(EntityKeyFields entityKeys)
        {
            SprintKeys keys = entityKeys as SprintKeys;
            return (from a in context.Sprints
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Sprint entity)
        {
            onAdd();
            context.Sprints.Add(entity);
        }

        public void Remove(Sprint entity)
        {
            context.Sprints.Attach(entity);
            context.Sprints.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Sprint entity)
        {
            onUpdate();
            context.Sprints.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Sprint> All()
        {
            return context.Sprints.ToList();
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
	 