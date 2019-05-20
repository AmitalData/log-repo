 
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
   public partial class TaxReportRepository:IRepository<TaxReport>
   {
        
		public List<TaxReport> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public bool CheckIfTaxReportExist(int month, int year, int tenant)
        {

            return (from a in context.TaxReports where a.TaxReportMonth.Month == month && a.TaxReportMonth.Year == year
                   && a.Tenant == tenant && a.IsCancelled == false select a).Any();
        }

        public bool CheckIfPreviousReportExist(int month, int year, int tenant)
        {
            int preMonth = month - 1;
            bool exist = false;

            if(month == 1)
            {
                preMonth = 12;

                exist = (from a in context.TaxReports
                         where a.TaxReportMonth.Month == preMonth && a.TaxReportMonth.Year == year-1
                                && a.Tenant == tenant && a.StatusCode == "T" && a.IsCancelled == false
                         select a).Any();
            }
            else
            {
                exist = (from a in context.TaxReports
                 where a.TaxReportMonth.Month == preMonth && a.TaxReportMonth.Year == year
                        && a.Tenant == tenant && a.StatusCode == "T" && a.IsCancelled == false
                 select a).Any();
            }
            return exist;
        }

        public bool CheckIfPreviousNotCompReportExist(int month, int year, int tenant)
        {
            int preMonth = month - 1;
            return (from a in context.TaxReports
                    where a.TaxReportMonth.Month == preMonth && a.TaxReportMonth.Year == year
                           && a.Tenant == tenant && a.StatusCode != "T" && a.IsCancelled == false
                    select a).Any();
        }

        public int ReportCount( int tenant)
        {

            return (from a in context.TaxReports
                    where a.Tenant == tenant && a.IsCancelled == false
                    select a).Count();
        }
        public bool CheckIfTaxReportWithHigherDateExist(int month, int year, int tenant)
        {
         
            return (from a in context.TaxReports
                    where ((a.TaxReportMonth.Month > month && a.TaxReportMonth.Year == year)|| ( a.TaxReportMonth.Year > year) )
                          && a.Tenant == tenant && a.IsCancelled == false
                    select a).Any();
          
        }

        public IQueryable<TaxReport> GetFutureReports(DateTime dateTime, int tenant)
        {
            return (from a in context.TaxReports
                    where a.CreateDate > dateTime && a.Tenant == tenant
                    select a);
        }

    }

}
   