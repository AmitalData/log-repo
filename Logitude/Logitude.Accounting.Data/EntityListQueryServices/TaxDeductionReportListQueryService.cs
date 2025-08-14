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

    public partial class TaxDeductionReportListQueryService
    {
	    private IQueryable<TaxDeductionReportList> GetIqueryableList(IQueryable<TaxDeductionReport> iQueryable)
        {
		IQueryable<TaxDeductionReportList> query = (from a in iQueryable
                                            select new TaxDeductionReportList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          ReportNumber = a.ReportNumber,
					
					                          StatusTypeCode = a.StatusTypeCode,
					
					                          IsAdditionalReportExist = a.IsAdditionalReportExist,
					
					                          TaxYear = a.TaxYear,
					
					                          Email = a.Email,
											  ByMonth = a.ByMonth,
											  Month = a.Month,
											  FromMonth = a.FromMonth,
											   
					       					 ErrorMessage = a.ErrorMessage,
                                              Status = a.TaxDeductionReportStatus != null? a.TaxDeductionReportStatus.EnglishName: null, 
                                              CreatedByUser= a.CreatedByUser != null? a.CreatedByUser.Contact.LocalName : null,
                                             StatusLocalName = a.TaxDeductionReportStatus != null ? a.TaxDeductionReportStatus.LocalName : null,

                                            });
            return query;
		}

		private IQueryable<TaxDeductionReport> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaxDeductionReport> iQueryable, int tenant)
        {
            return iQueryable;

        }
				private IQueryable<TaxDeductionReport> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaxDeductionReport> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	