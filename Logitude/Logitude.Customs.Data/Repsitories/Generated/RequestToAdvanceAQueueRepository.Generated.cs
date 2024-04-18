 
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
   public partial class RequestToAdvanceAQueueRepository:IRepository<RequestToAdvanceAQueue>
   {
   
        private ICustomContext currentContext;
        public RequestToAdvanceAQueueRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RequestToAdvanceAQueueRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RequestToAdvanceAQueue GetSingle(string code)
        {
            return (from a in context.RequestToAdvanceAQueues
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RequestToAdvanceAQueue> GetAll()
        {
            return from a in context.RequestToAdvanceAQueues  
                   select a;
        }
				 
        public RequestToAdvanceAQueue GetSingle(EntityKeyFields entityKeys)
        {
            RequestToAdvanceAQueueKeys keys = entityKeys as RequestToAdvanceAQueueKeys;
            return (from a in context.RequestToAdvanceAQueues
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RequestToAdvanceAQueue entity)
        {
            onAdd();
            context.RequestToAdvanceAQueues.Add(entity);
        }

        public void Remove(RequestToAdvanceAQueue entity)
        {
            context.RequestToAdvanceAQueues.Attach(entity);
            context.RequestToAdvanceAQueues.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RequestToAdvanceAQueue entity)
        {
            onUpdate();
            context.RequestToAdvanceAQueues.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RequestToAdvanceAQueue> All()
        {
            return context.RequestToAdvanceAQueues.ToList();
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
	 