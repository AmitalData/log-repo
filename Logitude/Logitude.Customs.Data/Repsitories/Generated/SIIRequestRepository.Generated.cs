 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SIIRequestRepository:IRepository<SIIRequest>
   {
   
        private ICustomContext currentContext;
        public SIIRequestRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SIIRequestRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SIIRequest GetSingle(string id, int tenant)
        {
            return (from a in context.SIIRequests
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SIIRequest> GetAll(int tenant)
        {
            return from a in context.SIIRequests  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SIIRequest GetSingle(EntityKeyFields entityKeys)
        {
            SIIRequestKeys keys = entityKeys as SIIRequestKeys;
            return (from a in context.SIIRequests
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SIIRequest entity)
        {
            onAdd();
            context.SIIRequests.Add(entity);
        }

        public void Remove(SIIRequest entity)
        {
            context.SIIRequests.Attach(entity);
            context.SIIRequests.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SIIRequest entity)
        {
            onUpdate();
            context.SIIRequests.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SIIRequest> All()
        {
            return context.SIIRequests.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 