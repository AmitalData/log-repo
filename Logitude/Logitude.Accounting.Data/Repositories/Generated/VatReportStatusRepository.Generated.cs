 
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
   public partial class VatReportStatusRepository:IRepository<VatReportStatus>
   {
   
        private IAccountingContext currentContext;
        public VatReportStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public VatReportStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  VatReportStatus GetSingle(string code)
        {
            return (from a in context.VatReportStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VatReportStatus> GetAll()
        {
            return from a in context.VatReportStatuses  
                   select a;
        }
				 
        public VatReportStatus GetSingle(EntityKeyFields entityKeys)
        {
            VatReportStatusKeys keys = entityKeys as VatReportStatusKeys;
            return (from a in context.VatReportStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VatReportStatus entity)
        {
            onAdd();
            context.VatReportStatuses.Add(entity);
        }

        public void Remove(VatReportStatus entity)
        {
            context.VatReportStatuses.Attach(entity);
            context.VatReportStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VatReportStatus entity)
        {
            onUpdate();
            context.VatReportStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VatReportStatus> All()
        {
            return context.VatReportStatuses.ToList();
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
	 