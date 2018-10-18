using Logitude.Server.Tools;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.BL.EntityQueryServices
{
    public partial class FollowerQueryService : EntityQueryService<Follower, FollowerKeys, FollowerPM, object, FollowerKeys>
    {
        public List<FollowerPM> GetUserFollowers(string followeeId, int tenant)
        {
            List<Follower> followers = repository.GetUserFollowers(followeeId, tenant);

          
            List<FollowerPM> followersPms = new List<FollowerPM>();

            foreach (Follower follower in followers)
            {
                EntityKeys = new FollowerKeys() { FollowerUserId = follower.FollowerUserId, FolloweeUserId = follower.FolloweeUserId };
                FollowerPM followerPM = new FollowerPM();
                mapping.CustomPOCOToPM(followerPM, follower);
                mapping.POCOToPM(followerPM, follower);


                followersPms.Add(followerPM);

            }

            return followersPms;
        }


        public List<FollowerPM> GetUserFollowee(string userId, int tenant)
        {
            List<Follower> followeee = repository.GetUserFollowee(userId, tenant);


            List<FollowerPM> followeeesPms = new List<FollowerPM>();

            foreach (Follower follower in followeee)
            {
                EntityKeys = new FollowerKeys() { FollowerUserId = follower.FollowerUserId, FolloweeUserId = follower.FolloweeUserId };
                FollowerPM followerPM = new FollowerPM();
                mapping.CustomPOCOToPM(followerPM, follower);
                mapping.POCOToPM(followerPM, follower);


                followeeesPms.Add(followerPM);

            }

            return followeeesPms;
        }

         
    }
}
