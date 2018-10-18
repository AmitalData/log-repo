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

    public partial class ActivityPriorityListQueryService
    {
        private IQueryable<ActivityPriorityList> GetIqueryableList(IQueryable<ActivityPriority> iQueryable)
        {
            IQueryable<ActivityPriorityList> query = (from a in iQueryable
                                                      select new ActivityPriorityList()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields,

                                              });
            return query;
        }

        private IQueryable<ActivityPriority> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ActivityPriority> iQueryable)
        {
            return iQueryable;
        }

        private IQueryable<ActivityPriority> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ActivityPriority> iQueryable)
        {
            return iQueryable;
        }
	}


}
	