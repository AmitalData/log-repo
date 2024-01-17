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

    public partial class CB_RuleDetailsHistoryListQueryService
    {
	    private IQueryable<CB_RuleDetailsHistoryList> GetIqueryableList(IQueryable<CB_RuleDetailsHistory> iQueryable)
        {
		IQueryable<CB_RuleDetailsHistoryList> query = (from a in iQueryable
                                            select new CB_RuleDetailsHistoryList()
											{
                     
					                          ID = a.ID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_RuleDetailsHistory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_RuleDetailsHistory> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	