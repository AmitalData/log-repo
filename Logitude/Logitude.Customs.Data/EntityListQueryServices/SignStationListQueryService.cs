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

    public partial class SignStationListQueryService
    {
	    private IQueryable<SignStationList> GetIqueryableList(IQueryable<SignStation> iQueryable)
        {
		IQueryable<SignStationList> query = (from a in iQueryable
                                            select new SignStationList()
											{
                     
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<SignStation> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SignStation> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	