 
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
   public partial class ConfirmationNumberStatusRepository:IRepository<ConfirmationNumberStatus>
   {
   
        private IAccountingContext currentContext;
        public ConfirmationNumberStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ConfirmationNumberStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConfirmationNumberStatus GetSingle(string code)
        {
            return (from a in context.ConfirmationNumberStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ConfirmationNumberStatus> GetAll()
        {
            return from a in context.ConfirmationNumberStatuses  
                   select a;
        }
				 
        public ConfirmationNumberStatus GetSingle(EntityKeyFields entityKeys)
        {
            ConfirmationNumberStatusKeys keys = entityKeys as ConfirmationNumberStatusKeys;
            return (from a in context.ConfirmationNumberStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConfirmationNumberStatus entity)
        {
            onAdd();
            context.ConfirmationNumberStatuses.Add(entity);
        }

        public void Remove(ConfirmationNumberStatus entity)
        {
            context.ConfirmationNumberStatuses.Attach(entity);
            context.ConfirmationNumberStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConfirmationNumberStatus entity)
        {
            onUpdate();
            context.ConfirmationNumberStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConfirmationNumberStatus> All()
        {
            return context.ConfirmationNumberStatuses.ToList();
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
	 