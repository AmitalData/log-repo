 
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
   public partial class JournalLineRepository:IRepository<JournalLine>
   {
   
        private IAccountingContext currentContext;
        public JournalLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public JournalLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  JournalLine GetSingle(string journalid, int line, int tenant)
        {
            return (from a in context.JournalLines
                    where a.JournalId == journalid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<JournalLine> GetAll(int tenant)
        {
            return from a in context.JournalLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public JournalLine GetSingle(EntityKeyFields entityKeys)
        {
            JournalLineKeys keys = entityKeys as JournalLineKeys;
            return (from a in context.JournalLines
                    where a.JournalId == keys.JournalId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(JournalLine entity)
        {
            onAdd();
            context.JournalLines.Add(entity);
        }

        public void Remove(JournalLine entity)
        {
            context.JournalLines.Attach(entity);
            context.JournalLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(JournalLine entity)
        {
            onUpdate();
            context.JournalLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<JournalLine> All()
        {
            return context.JournalLines.ToList();
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
	 