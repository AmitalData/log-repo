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

    public partial class TaxDeductionReportStatusListQueryService
    {
	    private IQueryable<TaxDeductionReportStatusList> GetIqueryableList(IQueryable<TaxDeductionReportStatus> iQueryable)
        {
		IQueryable<TaxDeductionReportStatusList> query = (from a in iQueryable
                                            select new TaxDeductionReportStatusList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<TaxDeductionReportStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaxDeductionReportStatus> iQueryable)
        {
            return iQueryable;
        }
				private IQueryable<TaxDeductionReportStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaxDeductionReportStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	