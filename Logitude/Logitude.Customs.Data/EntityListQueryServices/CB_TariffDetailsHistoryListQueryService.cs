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

    public partial class CB_TariffDetailsHistoryListQueryService
    {
	    private IQueryable<CB_TariffDetailsHistoryList> GetIqueryableList(IQueryable<CB_TariffDetailsHistory> iQueryable)
        {
		IQueryable<CB_TariffDetailsHistoryList> query = (from a in iQueryable
                                            select new CB_TariffDetailsHistoryList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<CB_TariffDetailsHistory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_TariffDetailsHistory> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	