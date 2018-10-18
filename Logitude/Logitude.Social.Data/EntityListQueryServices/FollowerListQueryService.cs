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
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Social.Data.EntityListQueryServices
{ 

    public partial class FollowerListQueryService
    {
	    private IQueryable<FollowerList> GetIqueryableList(IQueryable<Follower> iQueryable)
        {
			throw new NotImplementedException();
		}


        public int UserFllowersCount(string userId, int tenant)
        {
            return (from a in context.Followers where a.FolloweeUserId == userId && a.Tenant == tenant select a).Count();

        }
        public bool CheckIfFllowers(string followerUserId, string followeeUserId, int tenant)
        {
            Follower follower =  (from a in context.Followers where a.FolloweeUserId == followeeUserId && a.FollowerUserId == followerUserId && a.Tenant == tenant select a).FirstOrDefault();
            if (follower != null)
            {
                return true;
            }
            else return false;
        }




        private IQueryable<Follower> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Follower> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

        private IQueryable<Follower> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Follower> iQueryable, int tenant)
        {
            return iQueryable;
        }



	}


}
	