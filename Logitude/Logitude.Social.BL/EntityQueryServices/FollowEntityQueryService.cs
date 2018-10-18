using Logitude.Social.BL.EntityPMs;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.BL.EntityQueryServices
{
    public partial class FollowEntityQueryService
    {
        public List<FollowEntityPM> GetEntityFollowers(string entityId,string objectTableId, int tenant)
        {
            List<FollowEntity> followers = repository.GetEntityFollowers(entityId, objectTableId, tenant);


            List<FollowEntityPM> followersPms = new List<FollowEntityPM>();

            foreach (FollowEntity follower in followers)
            {
                EntityKeys = new FollowEntityKeys() {  Id = follower.Id};
                FollowEntityPM followerPM = new FollowEntityPM();
                mapping.CustomPOCOToPM(followerPM, follower);
                mapping.POCOToPM(followerPM, follower);


                followersPms.Add(followerPM);

            }

            return followersPms;
        }
    }
}
