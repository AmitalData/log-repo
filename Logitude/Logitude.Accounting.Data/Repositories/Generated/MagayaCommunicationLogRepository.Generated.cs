 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class MagayaCommunicationLogRepository:IRepository<MagayaCommunicationLog>
   {
   
        private IAccountingContext currentContext;
        public MagayaCommunicationLogRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public MagayaCommunicationLogRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  MagayaCommunicationLog GetSingle(string id, int tenant)
        {
            return (from a in context.MagayaCommunicationLogs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<MagayaCommunicationLog> GetAll(int tenant)
        {
            return from a in context.MagayaCommunicationLogs  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public MagayaCommunicationLog GetSingle(EntityKeyFields entityKeys)
        {
            MagayaCommunicationLogKeys keys = entityKeys as MagayaCommunicationLogKeys;
            return (from a in context.MagayaCommunicationLogs
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MagayaCommunicationLog entity)
        {
            onAdd();
            context.MagayaCommunicationLogs.Add(entity);
        }

        public void Remove(MagayaCommunicationLog entity)
        {
            context.MagayaCommunicationLogs.Attach(entity);
            context.MagayaCommunicationLogs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MagayaCommunicationLog entity)
        {
            onUpdate();
            context.MagayaCommunicationLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MagayaCommunicationLog> All()
        {
            return context.MagayaCommunicationLogs.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 