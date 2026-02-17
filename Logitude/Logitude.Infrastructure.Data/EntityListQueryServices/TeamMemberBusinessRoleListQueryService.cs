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

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class TeamMemberBusinessRoleListQueryService
    {
        private IQueryable<TeamMemberBusinessRoleList> GetIqueryableList(IQueryable<TeamMemberBusinessRole> iQueryable)
        {
            IQueryable<TeamMemberBusinessRoleList> query = (from a in iQueryable
                                                            select new TeamMemberBusinessRoleList()
                                                            {

                                                                Id = a.Id,

                                                                Tenant = a.Tenant,

                                                                TeamMemberId = a.TeamMemberId,

                                                                AddDate = a.AddDate,

                                                                BusinessRoleId = a.BusinessRoleId,

                                                                RoleName = a.BusinessRole != null ? a.BusinessRole.Name : null,

                                                            });
            return query;
        }

		private IQueryable<TeamMemberBusinessRole> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TeamMemberBusinessRole> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<TeamMemberBusinessRole> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TeamMemberBusinessRole> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	