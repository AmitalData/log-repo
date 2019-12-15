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

    public partial class InterestEntityTypeListQueryService
    {
	    private IQueryable<InterestEntityTypeList> GetIqueryableList(IQueryable<InterestEntityType> iQueryable)
        {
		IQueryable<InterestEntityTypeList> query = (from a in iQueryable
                                            select new InterestEntityTypeList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
		                    	            });
            return query;
		}

		private IQueryable<InterestEntityType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestEntityType> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<InterestEntityType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestEntityType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	