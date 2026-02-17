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

    public partial class TaxReportLineTransmitStatusListQueryService
    {
	    private IQueryable<TaxReportLineTransmitStatusList> GetIqueryableList(IQueryable<TaxReportLineTransmitStatus> iQueryable)
        {
		IQueryable<TaxReportLineTransmitStatusList> query = (from a in iQueryable
                                            select new TaxReportLineTransmitStatusList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<TaxReportLineTransmitStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaxReportLineTransmitStatus> iQueryable)
        {
            return iQueryable;
        }
        private IQueryable<TaxReportLineTransmitStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaxReportLineTransmitStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	