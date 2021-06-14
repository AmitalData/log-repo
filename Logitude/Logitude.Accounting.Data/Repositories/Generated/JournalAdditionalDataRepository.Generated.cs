 
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
   public partial class JournalAdditionalDataRepository:IRepository<JournalAdditionalData>
   {
   
        private IAccountingContext currentContext;
        public JournalAdditionalDataRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public JournalAdditionalDataRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  JournalAdditionalData GetSingle(string journalid, int journallinenumber, int tenant)
        {
            return (from a in context.JournalAdditionalDatas
                    where a.JournalId == journalid && a.JournalLineNumber == journallinenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<JournalAdditionalData> GetAll(int tenant)
        {
            return from a in context.JournalAdditionalDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public JournalAdditionalData GetSingle(EntityKeyFields entityKeys)
        {
            JournalAdditionalDataKeys keys = entityKeys as JournalAdditionalDataKeys;
            return (from a in context.JournalAdditionalDatas
                    where a.JournalId == keys.JournalId && a.JournalLineNumber == keys.JournalLineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(JournalAdditionalData entity)
        {
            onAdd();
            context.JournalAdditionalDatas.Add(entity);
        }

        public void Remove(JournalAdditionalData entity)
        {
            context.JournalAdditionalDatas.Attach(entity);
            context.JournalAdditionalDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(JournalAdditionalData entity)
        {
            onUpdate();
            context.JournalAdditionalDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<JournalAdditionalData> All()
        {
            return context.JournalAdditionalDatas.ToList();
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
	 