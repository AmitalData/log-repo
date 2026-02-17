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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class ActivityStatusListQueryService
    {
	    private IQueryable<ActivityStatusList> GetIqueryableList(IQueryable<ActivityStatus> iQueryable)
        {
            IQueryable<ActivityStatusList> query = (from a in iQueryable
                                                    select new ActivityStatusList()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields,
                                              });
            return query;
		}

        private IQueryable<ActivityStatus> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ActivityStatus> iQueryable)
        {
            return iQueryable;
        }

        private IQueryable<ActivityStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ActivityStatus> iQueryable)
        {
            return iQueryable;
        }
	}


}
	