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

    public partial class OpenFormatReportStatusListQueryService
    {
	    private IQueryable<OpenFormatReportStatusList> GetIqueryableList(IQueryable<OpenFormatReportStatus> iQueryable)
        {
		IQueryable<OpenFormatReportStatusList> query = (from a in iQueryable
                                            select new OpenFormatReportStatusList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
		                    	            });
            return query;
		}

		private IQueryable<OpenFormatReportStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OpenFormatReportStatus> iQueryable)
        {
            return iQueryable;
        }
				private IQueryable<OpenFormatReportStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<OpenFormatReportStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	