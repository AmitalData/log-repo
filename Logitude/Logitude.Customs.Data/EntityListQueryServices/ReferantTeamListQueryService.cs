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

    public partial class ReferantTeamListQueryService
    {
	    private IQueryable<ReferantTeamList> GetIqueryableList(IQueryable<ReferantTeam> iQueryable)
        {
		IQueryable<ReferantTeamList> query = (from a in iQueryable
                                            select new ReferantTeamList()
											{
                     
					                          SearchFields = a.SearchFields,
					
					                          Code = a.Code,
					
					                          Inactive = a.Inactive,
					
					                          Tenant = a.Tenant,
											  EnglishName=a.EnglishName,
											  LocalName=a.LocalName,
		                    	            });
            return query;
		}

		private IQueryable<ReferantTeam> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ReferantTeam> iQueryable, int tenant)
        {
			return iQueryable;
		}
			}


}
	