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

    public partial class TaxReportStatusListQueryService
    {
	    private IQueryable<TaxReportStatusList> GetIqueryableList(IQueryable<TaxReportStatus> iQueryable)
        {
		IQueryable<TaxReportStatusList> query = (from a in iQueryable
                                            select new TaxReportStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
		                    	            });
            return query;
		}

		private IQueryable<TaxReportStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaxReportStatus> iQueryable)
        {
            return iQueryable;
        }
        private IQueryable<TaxReportStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaxReportStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	