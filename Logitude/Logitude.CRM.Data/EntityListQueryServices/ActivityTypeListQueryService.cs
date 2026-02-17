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

    public partial class ActivityTypeListQueryService
    {
        private IQueryable<ActivityTypeList> GetIqueryableList(IQueryable<ActivityType> iQueryable)
        {
            IQueryable<ActivityTypeList> query = (from a in iQueryable
                                                  select new ActivityTypeList()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields,
                                              });
            return query;
		}

        private IQueryable<ActivityType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ActivityType> iQueryable)
        {
            return iQueryable;
        }

        private IQueryable<ActivityType> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ActivityType> iQueryable)
        {
            return iQueryable;
        }
	}


}
	