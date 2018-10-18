 
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
   public partial class TMReleaseRepository:IRepository<TMRelease>
   {
   
        private ITimeManagementContext currentContext;
        public TMReleaseRepository(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMReleaseRepository(ITimeManagementContext context)
        {
            currentContext = context;
        }

		 
		
		public  TMRelease GetSingle(string id, int tenant)
        {
            return (from a in context.TMReleases
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TMRelease> GetAll(int tenant)
        {
            return from a in context.TMReleases  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TMRelease GetSingle(EntityKeyFields entityKeys)
        {
            TMReleaseKeys keys = entityKeys as TMReleaseKeys;
            return (from a in context.TMReleases
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TMRelease entity)
        {
            onAdd();
            context.TMReleases.Add(entity);
        }

        public void Remove(TMRelease entity)
        {
            context.TMReleases.Attach(entity);
            context.TMReleases.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TMRelease entity)
        {
            onUpdate();
            context.TMReleases.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TMRelease> All()
        {
            return context.TMReleases.ToList();
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
	 