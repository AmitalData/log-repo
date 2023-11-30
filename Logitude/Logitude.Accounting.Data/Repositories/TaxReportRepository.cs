 
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
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class TaxReportRepository:IRepository<TaxReport>
   {
        private const string TaxReportTransmittedAndJournalCreatedStatus = "J";
        private const string TaxReportTransmittedStatus = "T";

        public List<TaxReport> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public bool CheckIfTaxReportExist(DateTime taxReportDate, int tenant)
        {
            return (from a in context.TaxReports
                    where (DbFunctions.TruncateTime(a.TaxReportMonth) == taxReportDate.Date ||
                    (a.CreatedInTwoMonthsLogic == true && SqlFunctions.DateAdd("month", -1, a.TaxReportMonth) == taxReportDate.Date))
                    && a.Tenant == tenant
                    && a.IsCancelled == false
                    select a).Any();
        }

        public bool CheckIfTaxReportExistForPreviousMonth(DateTime taxReportDate, int tenant)
        {
            DateTime taxReportDateWithPreviousMonth = taxReportDate.AddMonths(-1);
            return (from a in context.TaxReports
                    where DbFunctions.TruncateTime(a.TaxReportMonth) == taxReportDateWithPreviousMonth.Date
                    && a.Tenant == tenant
                    && a.IsCancelled == false
                    select a).Any();
        }

        public bool CheckIfPreviousReportExist(DateTime taxReportDate, int tenant, bool VATreportEveryTwoMonths)
        {
            DateTime taxReportDateWithPreviousMonth = taxReportDate.AddMonths(-1);
            DateTime taxReportDateWithPreviousTwoMonths = taxReportDate.AddMonths(-2);
            return (from a in context.TaxReports
                     where DbFunctions.TruncateTime(a.TaxReportMonth) == (VATreportEveryTwoMonths == true ?
                     taxReportDateWithPreviousTwoMonths.Date : taxReportDateWithPreviousMonth.Date)
                            && a.Tenant == tenant && (a.StatusCode == TaxReportTransmittedStatus || a.StatusCode == TaxReportTransmittedAndJournalCreatedStatus) && a.IsCancelled == false
                     select a).Any();
        }

        public bool CheckIfPreviousNotCompReportExist(DateTime taxReportDate, int tenant, bool VATreportEveryTwoMonths)
        {
            DateTime taxReportDateWithPreviousMonth = taxReportDate.AddMonths(-1);
            DateTime taxReportDateWithPreviousTwoMonths = taxReportDate.AddMonths(-2);
            return (from a in context.TaxReports
                    where DbFunctions.TruncateTime(a.TaxReportMonth) == (VATreportEveryTwoMonths == true ?
                    taxReportDateWithPreviousTwoMonths.Date : taxReportDateWithPreviousMonth.Date)
                           && a.Tenant == tenant && a.StatusCode != TaxReportTransmittedStatus && a.StatusCode != TaxReportTransmittedAndJournalCreatedStatus && a.IsCancelled == false
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

        public IQueryable<TaxReport> GetFutureReportsByTaxReportMonth(DateTime dateTime, int tenant)
        {
            return (from a in context.TaxReports
                    where a.TaxReportMonth > dateTime && a.Tenant == tenant
                    select a);
        }

        public IQueryable<TaxReport> GetTransmittedReports( int tenant)
        {
            return (from a in context.TaxReports
                    where (a.StatusCode=="T" || a.StatusCode == TaxReportTransmittedAndJournalCreatedStatus) && a.Tenant == tenant
                    select a);
        }

    }

}
   