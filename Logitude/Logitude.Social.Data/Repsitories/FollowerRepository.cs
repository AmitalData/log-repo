 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Social.Data.Repsitories
{
   public partial class FollowerRepository:IRepository<Follower>
   {
        
		public List<Follower> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public List<Follower> GetUserFollowers(string followeeId, int tenant)
        {
            List<Follower> followers = (from a in context.Followers
                                        where a.FolloweeUserId == followeeId && a.Tenant == tenant
                                        select a).ToList();

            return followers;
        }

        public List<Follower> GetUserFollowee(string followerId, int tenant)
        {
            List<Follower> followers = (from a in context.Followers
                                        where a.FollowerUserId == followerId && a.Tenant == tenant
                                        select a).ToList();

            return followers;
        }

   }

}
   