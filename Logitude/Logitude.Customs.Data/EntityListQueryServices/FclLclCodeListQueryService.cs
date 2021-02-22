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

    public partial class FclLclCodeListQueryService
    {
	    private IQueryable<FclLclCodeList> GetIqueryableList(IQueryable<FclLclCode> iQueryable)
        {
		IQueryable<FclLclCodeList> query = (from a in iQueryable
                                            select new FclLclCodeList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<FclLclCode> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<FclLclCode> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	