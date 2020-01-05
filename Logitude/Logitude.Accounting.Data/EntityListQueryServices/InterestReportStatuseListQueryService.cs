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

    public partial class InterestReportStatuseListQueryService
    {
	    private IQueryable<InterestReportStatuseList> GetIqueryableList(IQueryable<InterestReportStatuse> iQueryable)
        {
		IQueryable<InterestReportStatuseList> query = (from a in iQueryable
                                            select new InterestReportStatuseList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          EnglishName = a.EnglishName,
					
		                    	            });
            return query;
		}

		private IQueryable<InterestReportStatuse> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestReportStatuse> iQueryable)
        {
            return iQueryable;

        }
        private IQueryable<InterestReportStatuse> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestReportStatuse> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	