 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class LastRunDetailRepository:IRepository<LastRunDetail>
   {
   
        private IInfrastructureContext currentContext;
        public LastRunDetailRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public LastRunDetailRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  LastRunDetail GetSingle(string id, int tenant)
        {
            return (from a in context.LastRunDetails
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<LastRunDetail> GetAll(int tenant)
        {
            return from a in context.LastRunDetails  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public LastRunDetail GetSingle(EntityKeyFields entityKeys)
        {
            LastRunDetailKeys keys = entityKeys as LastRunDetailKeys;
            return (from a in context.LastRunDetails
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LastRunDetail entity)
        {
            onAdd();
            context.LastRunDetails.Add(entity);
        }

        public void Remove(LastRunDetail entity)
        {
            context.LastRunDetails.Attach(entity);
            context.LastRunDetails.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LastRunDetail entity)
        {
            onUpdate();
            context.LastRunDetails.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LastRunDetail> All()
        {
            return context.LastRunDetails.ToList();
        }

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 