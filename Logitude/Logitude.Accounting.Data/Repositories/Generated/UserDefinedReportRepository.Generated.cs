 
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
   public partial class UserDefinedReportRepository:IRepository<UserDefinedReport>
   {
   
        private IAccountingContext currentContext;
        public UserDefinedReportRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public UserDefinedReportRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  UserDefinedReport GetSingle(string id, int tenant)
        {
            return (from a in context.UserDefinedReports
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<UserDefinedReport> GetAll(int tenant)
        {
            return from a in context.UserDefinedReports  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public UserDefinedReport GetSingle(EntityKeyFields entityKeys)
        {
            UserDefinedReportKeys keys = entityKeys as UserDefinedReportKeys;
            return (from a in context.UserDefinedReports
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(UserDefinedReport entity)
        {
            onAdd();
            context.UserDefinedReports.Add(entity);
        }

        public void Remove(UserDefinedReport entity)
        {
            context.UserDefinedReports.Attach(entity);
            context.UserDefinedReports.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(UserDefinedReport entity)
        {
            onUpdate();
            context.UserDefinedReports.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UserDefinedReport> All()
        {
            return context.UserDefinedReports.ToList();
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
	 