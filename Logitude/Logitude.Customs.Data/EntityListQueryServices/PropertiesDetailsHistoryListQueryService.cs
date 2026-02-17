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

    public partial class PropertiesDetailsHistoryListQueryService
    {
	    private IQueryable<PropertiesDetailsHistoryList> GetIqueryableList(IQueryable<PropertiesDetailsHistory> iQueryable)
        {
		IQueryable<PropertiesDetailsHistoryList> query = (from a in iQueryable
                                            select new PropertiesDetailsHistoryList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<PropertiesDetailsHistory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<PropertiesDetailsHistory> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	