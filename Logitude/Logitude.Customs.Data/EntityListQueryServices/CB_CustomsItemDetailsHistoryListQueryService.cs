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

    public partial class CB_CustomsItemDetailsHistoryListQueryService
    {
	    private IQueryable<CB_CustomsItemDetailsHistoryList> GetIqueryableList(IQueryable<CB_CustomsItemDetailsHistory> iQueryable)
        {
		IQueryable<CB_CustomsItemDetailsHistoryList> query = (from a in iQueryable
                                            select new CB_CustomsItemDetailsHistoryList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<CB_CustomsItemDetailsHistory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_CustomsItemDetailsHistory> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	