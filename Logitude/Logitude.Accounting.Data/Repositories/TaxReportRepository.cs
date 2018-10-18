 
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

        public bool CheckIfTaxReportWithHigherDateExist(int month, int year, int tenant)
        {
         
            return (from a in context.TaxReports
                    where ((a.TaxReportMonth.Month > month && a.TaxReportMonth.Year == year)|| ( a.TaxReportMonth.Year > year) )
                          && a.Tenant == tenant && a.IsCancelled == false
                    select a).Any();
          
        }

    }

}
   