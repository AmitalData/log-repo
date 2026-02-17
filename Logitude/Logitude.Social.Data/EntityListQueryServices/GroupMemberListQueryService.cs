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

using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.EntityLists;

namespace Logitude.Social.Data.EntityListQueryServices
{ 

    public partial class GroupMemberListQueryService
    {
	    private IQueryable<GroupMemberList> GetIqueryableList(IQueryable<GroupMember> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<GroupMember> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GroupMember> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

        private IQueryable<GroupMember> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<GroupMember> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	