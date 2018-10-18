 
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
   public partial class TMLocationRepository:IRepository<TMLocation>
   {
   
        private ITimeManagementContext currentContext;
        public TMLocationRepository(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMLocationRepository(ITimeManagementContext context)
        {
            currentContext = context;
        }

		 
		
		public  TMLocation GetSingle(string code)
        {
            return (from a in context.TMLocations
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TMLocation> GetAll()
        {
            return from a in context.TMLocations  
                   select a;
        }
				 
        public TMLocation GetSingle(EntityKeyFields entityKeys)
        {
            TMLocationKeys keys = entityKeys as TMLocationKeys;
            return (from a in context.TMLocations
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TMLocation entity)
        {
            onAdd();
            context.TMLocations.Add(entity);
        }

        public void Remove(TMLocation entity)
        {
            context.TMLocations.Attach(entity);
            context.TMLocations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TMLocation entity)
        {
            onUpdate();
            context.TMLocations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TMLocation> All()
        {
            return context.TMLocations.ToList();
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
	 