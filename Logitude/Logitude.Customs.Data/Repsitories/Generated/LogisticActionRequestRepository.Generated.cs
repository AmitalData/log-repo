 
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
   public partial class LogisticActionRequestRepository:IRepository<LogisticActionRequest>
   {
   
        private ICustomContext currentContext;
        public LogisticActionRequestRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LogisticActionRequestRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LogisticActionRequest GetSingle(string id, int tenant)
        {
            return (from a in context.LogisticActionRequests
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<LogisticActionRequest> GetAll(int tenant)
        {
            return from a in context.LogisticActionRequests  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public LogisticActionRequest GetSingle(EntityKeyFields entityKeys)
        {
            LogisticActionRequestKeys keys = entityKeys as LogisticActionRequestKeys;
            return (from a in context.LogisticActionRequests
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LogisticActionRequest entity)
        {
            onAdd();
            context.LogisticActionRequests.Add(entity);
        }

        public void Remove(LogisticActionRequest entity)
        {
            context.LogisticActionRequests.Attach(entity);
            context.LogisticActionRequests.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LogisticActionRequest entity)
        {
            onUpdate();
            context.LogisticActionRequests.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LogisticActionRequest> All()
        {
            return context.LogisticActionRequests.ToList();
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
	 