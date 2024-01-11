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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class PerYearFrequencyListQueryService
    {
	    private IQueryable<PerYearFrequencyList> GetIqueryableList(IQueryable<PerYearFrequency> iQueryable)
        {
		IQueryable<PerYearFrequencyList> query = (from a in iQueryable
                                            select new PerYearFrequencyList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          EnglishName = a.EnglishName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<PerYearFrequency> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<PerYearFrequency> iQueryable)
        {
            return iQueryable;
        }
			}


}
	