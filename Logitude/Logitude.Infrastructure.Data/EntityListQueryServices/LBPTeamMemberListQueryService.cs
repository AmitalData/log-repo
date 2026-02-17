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

    public partial class LBPTeamMemberListQueryService
    {
	    private IQueryable<LBPTeamMemberList> GetIqueryableList(IQueryable<LBPTeamMember> iQueryable)
        {
		IQueryable<LBPTeamMemberList> query = (from a in iQueryable
                                            select new LBPTeamMemberList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          MemberUserId = a.MemberUserId,
					
					                          TeamId = a.TeamId,
					
					                          AddDate = a.AddDate,
					
		                    	            });
            return query;
		}

		private IQueryable<LBPTeamMember> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<LBPTeamMember> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<LBPTeamMember> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<LBPTeamMember> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	