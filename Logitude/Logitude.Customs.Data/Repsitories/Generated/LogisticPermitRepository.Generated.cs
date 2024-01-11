 
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
   public partial class LogisticPermitRepository:IRepository<LogisticPermit>
   {
   
        private ICustomContext currentContext;
        public LogisticPermitRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LogisticPermitRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LogisticPermit GetSingle(string id, int tenant)
        {
            return (from a in context.LogisticPermits
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<LogisticPermit> GetAll(int tenant)
        {
            return from a in context.LogisticPermits  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public LogisticPermit GetSingle(EntityKeyFields entityKeys)
        {
            LogisticPermitKeys keys = entityKeys as LogisticPermitKeys;
            return (from a in context.LogisticPermits
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LogisticPermit entity)
        {
            onAdd();
            context.LogisticPermits.Add(entity);
        }

        public void Remove(LogisticPermit entity)
        {
            context.LogisticPermits.Attach(entity);
            context.LogisticPermits.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LogisticPermit entity)
        {
            onUpdate();
            context.LogisticPermits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LogisticPermit> All()
        {
            return context.LogisticPermits.ToList();
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
	 