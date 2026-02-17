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

    public partial class TaxReportLineTypeListQueryService
    {
	    private IQueryable<TaxReportLineTypeList> GetIqueryableList(IQueryable<TaxReportLineType> iQueryable)
        {
		IQueryable<TaxReportLineTypeList> query = (from a in iQueryable
                                            select new TaxReportLineTypeList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<TaxReportLineType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaxReportLineType> iQueryable)
        {
            return iQueryable;
        }
        private IQueryable<TaxReportLineType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaxReportLineType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	