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

    public partial class FeedListQueryService
    {
	    private IQueryable<FeedList> GetIqueryableList(IQueryable<Feed> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<Feed> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Feed> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}
        private IQueryable<Feed> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Feed> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	