 
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
   public partial class OpenFormatReportRepository:IRepository<OpenFormatReport>
   {
   
        private IAccountingContext currentContext;
        public OpenFormatReportRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public OpenFormatReportRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpenFormatReport GetSingle(string id, int tenant)
        {
            return (from a in context.OpenFormatReports
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OpenFormatReport> GetAll(int tenant)
        {
            return from a in context.OpenFormatReports  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OpenFormatReport GetSingle(EntityKeyFields entityKeys)
        {
            OpenFormatReportKeys keys = entityKeys as OpenFormatReportKeys;
            return (from a in context.OpenFormatReports
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpenFormatReport entity)
        {
            onAdd();
            context.OpenFormatReports.Add(entity);
        }

        public void Remove(OpenFormatReport entity)
        {
            context.OpenFormatReports.Attach(entity);
            context.OpenFormatReports.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpenFormatReport entity)
        {
            onUpdate();
            context.OpenFormatReports.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpenFormatReport> All()
        {
            return context.OpenFormatReports.ToList();
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
	 