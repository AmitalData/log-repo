 
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
   public partial class OpenFormatReportStatusRepository:IRepository<OpenFormatReportStatus>
   {
   
        private IAccountingContext currentContext;
        public OpenFormatReportStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public OpenFormatReportStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpenFormatReportStatus GetSingle(string code)
        {
            return (from a in context.OpenFormatReportStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<OpenFormatReportStatus> GetAll()
        {
            return from a in context.OpenFormatReportStatuses  
                   select a;
        }
				 
        public OpenFormatReportStatus GetSingle(EntityKeyFields entityKeys)
        {
            OpenFormatReportStatusKeys keys = entityKeys as OpenFormatReportStatusKeys;
            return (from a in context.OpenFormatReportStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpenFormatReportStatus entity)
        {
            onAdd();
            context.OpenFormatReportStatuses.Add(entity);
        }

        public void Remove(OpenFormatReportStatus entity)
        {
            context.OpenFormatReportStatuses.Attach(entity);
            context.OpenFormatReportStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpenFormatReportStatus entity)
        {
            onUpdate();
            context.OpenFormatReportStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpenFormatReportStatus> All()
        {
            return context.OpenFormatReportStatuses.ToList();
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
	 