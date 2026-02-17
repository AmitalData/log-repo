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

    public partial class ActivityInviteeListQueryService
    {
	    private IQueryable<ActivityInviteeList> GetIqueryableList(IQueryable<ActivityInvitee> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<ActivityInvitee> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ActivityInvitee> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

        private IQueryable<ActivityInvitee> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ActivityInvitee> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	