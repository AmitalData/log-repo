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

    public partial class PhysicalCheckSearchResultTypeListQueryService
    {
	    private IQueryable<PhysicalCheckSearchResultTypeList> GetIqueryableList(IQueryable<PhysicalCheckSearchResultType> iQueryable)
        {
		IQueryable<PhysicalCheckSearchResultTypeList> query = (from a in iQueryable
                                            select new PhysicalCheckSearchResultTypeList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<PhysicalCheckSearchResultType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<PhysicalCheckSearchResultType> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	