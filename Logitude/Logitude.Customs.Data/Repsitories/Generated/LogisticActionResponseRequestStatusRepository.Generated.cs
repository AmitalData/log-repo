 
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
   public partial class LogisticActionResponseRequestStatusRepository:IRepository<LogisticActionResponseRequestStatus>
   {
   
        private ICustomContext currentContext;
        public LogisticActionResponseRequestStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LogisticActionResponseRequestStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LogisticActionResponseRequestStatus GetSingle(string code)
        {
            return (from a in context.LogisticActionResponseRequestStatuss
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<LogisticActionResponseRequestStatus> GetAll()
        {
            return from a in context.LogisticActionResponseRequestStatuss  
                   select a;
        }
				 
        public LogisticActionResponseRequestStatus GetSingle(EntityKeyFields entityKeys)
        {
            LogisticActionResponseRequestStatusKeys keys = entityKeys as LogisticActionResponseRequestStatusKeys;
            return (from a in context.LogisticActionResponseRequestStatuss
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LogisticActionResponseRequestStatus entity)
        {
            onAdd();
            context.LogisticActionResponseRequestStatuss.Add(entity);
        }

        public void Remove(LogisticActionResponseRequestStatus entity)
        {
            context.LogisticActionResponseRequestStatuss.Attach(entity);
            context.LogisticActionResponseRequestStatuss.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LogisticActionResponseRequestStatus entity)
        {
            onUpdate();
            context.LogisticActionResponseRequestStatuss.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LogisticActionResponseRequestStatus> All()
        {
            return context.LogisticActionResponseRequestStatuss.ToList();
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
	 