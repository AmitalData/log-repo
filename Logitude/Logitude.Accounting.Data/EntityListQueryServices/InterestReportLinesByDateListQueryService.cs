	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class InterestReportLinesByDateListQueryService
    {
	    private IQueryable<InterestReportLinesByDateList> GetIqueryableList(IQueryable<InterestReportLinesByDate> iQueryable)
        {
		IQueryable<InterestReportLinesByDateList> query = (from a in iQueryable
                                            select new InterestReportLinesByDateList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,

                                              LineNumber = a.LineNumber,
					
					                          InterestReportId = a.InterestReportId,
					
					                          FromDate = a.FromDate,
					
					                          ToDate = a.ToDate,
					
					                          TotalInterestDays = a.TotalInterestDays,
					
					                          TotalAmount = a.TotalAmount,
					
					                          AccumulatedAmount = a.AccumulatedAmount,
					
					                          StandardInterestPercentage = a.StandardInterestPercentage,
					
					                          ExceptionalInterestPercentage = a.ExceptionalInterestPercentage,
					
					                          CreditInterestPercentage = a.CreditInterestPercentage,
					
					                          StandardInterestAmount = a.StandardInterestAmount,
					
					                          ExceptionalInterestAmount = a.ExceptionalInterestAmount,
					
					                          CreditInterestAmount = a.CreditInterestAmount,
					
					                          CalculatedStandInterestAmount = a.CalculatedStandInterestAmount,
					
					                          CalculatedExcepInterestAmount = a.CalculatedExcepInterestAmount,
					
					                          CalculatedCreditInterestAmount = a.CalculatedCreditInterestAmount,
					
					                          CalculationDetails = a.CalculationDetails,
					
                                              TotalInterest = a.CalculatedCreditInterestAmount + a.CalculatedExcepInterestAmount + a.CalculatedStandInterestAmount,

                                              IsOpenBalanceLine = a.IsOpenBalanceLine,
		                    	            });
            return query;
		}

		private IQueryable<InterestReportLinesByDate> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestReportLinesByDate> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<InterestReportLinesByDate> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestReportLinesByDate> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	