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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.CustomFilters;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{

    public partial class TeamListQueryService
    {
        private IQueryable<TeamList> GetIqueryableList(IQueryable<Team> iQueryable)
        {
            IQueryable<TeamList> query = (from a in iQueryable
                                          select new TeamList()
                                          {

                                              Id = a.Id,

                                              Tenant = a.Tenant,

                                              CreateDate = a.CreateDate,

                                              CreatedByUserId = a.CreatedByUserId,

                                              UpdateDate = a.UpdateDate,

                                              UpdatedByUserId = a.UpdatedByUserId,

                                              SearchFields = a.SearchFields,

                                              Name = a.Name,

                                              LocalName = a.LocalName,

                                              InActive = a.InActive,

                                              ManagerUserId = a.ManagerUserId,

                                              Notify = a.Notify,

                                              Notes = a.Notes,

                                              ManagerUserName = a.ManagerUser != null ? (a.ManagerUser.Contact != null ? a.ManagerUser.Contact.EnglishName : null) : null,
                                          });
            return query;
        }

        private IQueryable<Team> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Team> iQueryable, int tenant)
        {
            return TeamCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);
        }
        private IQueryable<Team> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Team> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }
}
	