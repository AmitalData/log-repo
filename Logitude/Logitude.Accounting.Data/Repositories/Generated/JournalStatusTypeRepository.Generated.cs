 
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
   public partial class JournalStatusTypeRepository:IRepository<JournalStatusType>
   {
   
        private IAccountingContext currentContext;
        public JournalStatusTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public JournalStatusTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  JournalStatusType GetSingle(string journalstatusid)
        {
            return (from a in context.JournalStatusTypes
                    where a.JournalStatusID == journalstatusid 
                    select a).FirstOrDefault();
        }

        public IQueryable<JournalStatusType> GetAll()
        {
            return from a in context.JournalStatusTypes  
                   select a;
        }
				 
        public JournalStatusType GetSingle(EntityKeyFields entityKeys)
        {
            JournalStatusTypeKeys keys = entityKeys as JournalStatusTypeKeys;
            return (from a in context.JournalStatusTypes
                    where a.JournalStatusID == keys.JournalStatusID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(JournalStatusType entity)
        {
            onAdd();
            context.JournalStatusTypes.Add(entity);
        }

        public void Remove(JournalStatusType entity)
        {
            context.JournalStatusTypes.Attach(entity);
            context.JournalStatusTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(JournalStatusType entity)
        {
            onUpdate();
            context.JournalStatusTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<JournalStatusType> All()
        {
            return context.JournalStatusTypes.ToList();
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
	 