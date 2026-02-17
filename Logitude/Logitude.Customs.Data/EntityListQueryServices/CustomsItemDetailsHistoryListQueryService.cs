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

    public partial class CustomsItemDetailsHistoryListQueryService
    {
	    private IQueryable<CustomsItemDetailsHistoryList> GetIqueryableList(IQueryable<CustomsItemDetailsHistory> iQueryable)
        {
		IQueryable<CustomsItemDetailsHistoryList> query = (from a in iQueryable
                                            select new CustomsItemDetailsHistoryList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<CustomsItemDetailsHistory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsItemDetailsHistory> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	