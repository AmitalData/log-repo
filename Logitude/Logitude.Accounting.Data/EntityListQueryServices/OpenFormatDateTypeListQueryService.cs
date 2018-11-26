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

    public partial class OpenFormatDateTypeListQueryService
    {
	    private IQueryable<OpenFormatDateTypeList> GetIqueryableList(IQueryable<OpenFormatDateType> iQueryable)
        {
		IQueryable<OpenFormatDateTypeList> query = (from a in iQueryable
                                            select new OpenFormatDateTypeList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
		                    	            });
            return query;
		}

		private IQueryable<OpenFormatDateType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OpenFormatDateType> iQueryable)
        {
            return iQueryable;
        }
				private IQueryable<OpenFormatDateType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<OpenFormatDateType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	