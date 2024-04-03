 
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
   public partial class JournalMoreDataRepository:IRepository<JournalMoreData>
   {
   
        private IAccountingContext currentContext;
        public JournalMoreDataRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public JournalMoreDataRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  JournalMoreData GetSingle(string journalid, int line, int tenant)
        {
            return (from a in context.JournalMoreDatas
                    where a.JournalId == journalid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<JournalMoreData> GetAll(int tenant)
        {
            return from a in context.JournalMoreDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public JournalMoreData GetSingle(EntityKeyFields entityKeys)
        {
            JournalMoreDataKeys keys = entityKeys as JournalMoreDataKeys;
            return (from a in context.JournalMoreDatas
                    where a.JournalId == keys.JournalId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(JournalMoreData entity)
        {
            onAdd();
            context.JournalMoreDatas.Add(entity);
        }

        public void Remove(JournalMoreData entity)
        {
            context.JournalMoreDatas.Attach(entity);
            context.JournalMoreDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(JournalMoreData entity)
        {
            onUpdate();
            context.JournalMoreDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<JournalMoreData> All()
        {
            return context.JournalMoreDatas.ToList();
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
	 