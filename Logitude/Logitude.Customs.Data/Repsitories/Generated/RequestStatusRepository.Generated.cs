 
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
   public partial class RequestStatusRepository:IRepository<RequestStatus>
   {
   
        private ICustomContext currentContext;
        public RequestStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RequestStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RequestStatus GetSingle(string code)
        {
            return (from a in context.RequestStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RequestStatus> GetAll()
        {
            return from a in context.RequestStatuses  
                   select a;
        }
				 
        public RequestStatus GetSingle(EntityKeyFields entityKeys)
        {
            RequestStatusKeys keys = entityKeys as RequestStatusKeys;
            return (from a in context.RequestStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RequestStatus entity)
        {
            onAdd();
            context.RequestStatuses.Add(entity);
        }

        public void Remove(RequestStatus entity)
        {
            context.RequestStatuses.Attach(entity);
            context.RequestStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RequestStatus entity)
        {
            onUpdate();
            context.RequestStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RequestStatus> All()
        {
            return context.RequestStatuses.ToList();
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
	 