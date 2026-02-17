 
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
   public partial class JournalActionTypeRepository:IRepository<JournalActionType>
   {
   
        private IAccountingContext currentContext;
        public JournalActionTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public JournalActionTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  JournalActionType GetSingle(string id, int tenant)
        {
            return (from a in context.JournalActionTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<JournalActionType> GetAll(int tenant)
        {
            return from a in context.JournalActionTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public JournalActionType GetSingle(EntityKeyFields entityKeys)
        {
            JournalActionTypeKeys keys = entityKeys as JournalActionTypeKeys;
            return (from a in context.JournalActionTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(JournalActionType entity)
        {
            onAdd();
            context.JournalActionTypes.Add(entity);
        }

        public void Remove(JournalActionType entity)
        {
            context.JournalActionTypes.Attach(entity);
            context.JournalActionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(JournalActionType entity)
        {
            onUpdate();
            context.JournalActionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<JournalActionType> All()
        {
            return context.JournalActionTypes.ToList();
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
	 