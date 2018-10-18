 
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
   public partial class TMProjectRepository:IRepository<TMProject>
   {
   
        private ITimeManagementContext currentContext;
        public TMProjectRepository(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMProjectRepository(ITimeManagementContext context)
        {
            currentContext = context;
        }

		 
		
		public  TMProject GetSingle(string id, int tenant)
        {
            return (from a in context.TMProjects
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TMProject> GetAll(int tenant)
        {
            return from a in context.TMProjects  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TMProject GetSingle(EntityKeyFields entityKeys)
        {
            TMProjectKeys keys = entityKeys as TMProjectKeys;
            return (from a in context.TMProjects
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TMProject entity)
        {
            onAdd();
            context.TMProjects.Add(entity);
        }

        public void Remove(TMProject entity)
        {
            context.TMProjects.Attach(entity);
            context.TMProjects.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TMProject entity)
        {
            onUpdate();
            context.TMProjects.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TMProject> All()
        {
            return context.TMProjects.ToList();
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
	 