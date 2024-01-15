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

    public partial class CB_RuleListQueryService
    {
	    private IQueryable<CB_RuleList> GetIqueryableList(IQueryable<CB_Rule> iQueryable)
        {
		IQueryable<CB_RuleList> query = (from a in iQueryable
                                            select new CB_RuleList()
											{
                     
					                          ID = a.ID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_Rule> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_Rule> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	