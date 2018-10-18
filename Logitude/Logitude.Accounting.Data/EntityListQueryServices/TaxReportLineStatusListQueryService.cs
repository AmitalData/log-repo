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

    public partial class TaxReportLineStatusListQueryService
    {
	    private IQueryable<TaxReportLineStatusList> GetIqueryableList(IQueryable<TaxReportLineStatus> iQueryable)
        {
		IQueryable<TaxReportLineStatusList> query = (from a in iQueryable
                                            select new TaxReportLineStatusList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<TaxReportLineStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaxReportLineStatus> iQueryable)
        {
            return iQueryable;
        }
        private IQueryable<TaxReportLineStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaxReportLineStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	