using Logitude.Server.Tools;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.BL.EntityUpdateServices
{
    public partial class PostLikeUpdateService : EntityUpdateService<PostLike, PostLikePM, PostPM>
    {

        protected override void OnCreating(PostLikePM entityPM, PostPM entityParentPM)
        {
            if (entityParentPM != null)
            {
                entityPM.PostId = entityParentPM.Id;
               
            }

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
