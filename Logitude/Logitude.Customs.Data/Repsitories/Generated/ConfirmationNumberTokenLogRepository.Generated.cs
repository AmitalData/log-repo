 
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
   public partial class ConfirmationNumberTokenLogRepository:IRepository<ConfirmationNumberTokenLog>
   {
   
        private ICustomContext currentContext;
        public ConfirmationNumberTokenLogRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConfirmationNumberTokenLogRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConfirmationNumberTokenLog GetSingle(string id, int tenant)
        {
            return (from a in context.ConfirmationNumberTokenLogs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ConfirmationNumberTokenLog> GetAll(int tenant)
        {
            return from a in context.ConfirmationNumberTokenLogs  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ConfirmationNumberTokenLog GetSingle(EntityKeyFields entityKeys)
        {
            ConfirmationNumberTokenLogKeys keys = entityKeys as ConfirmationNumberTokenLogKeys;
            return (from a in context.ConfirmationNumberTokenLogs
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConfirmationNumberTokenLog entity)
        {
            onAdd();
            context.ConfirmationNumberTokenLogs.Add(entity);
        }

        public void Remove(ConfirmationNumberTokenLog entity)
        {
            context.ConfirmationNumberTokenLogs.Attach(entity);
            context.ConfirmationNumberTokenLogs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConfirmationNumberTokenLog entity)
        {
            onUpdate();
            context.ConfirmationNumberTokenLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConfirmationNumberTokenLog> All()
        {
            return context.ConfirmationNumberTokenLogs.ToList();
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
	 