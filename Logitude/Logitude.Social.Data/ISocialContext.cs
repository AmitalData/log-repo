using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data; 
using Logitude.Social.Data.EntityMapping;

namespace Logitude.Social.Data
{

    public interface ISocialContext : IContext
    {
   
       	 IDbSet<ConversationHeader> ConversationHeaders { get; }
		 IDbSet<ConversationHeaderMessage> ConversationHeaderMessages { get; }
		 IDbSet<ConversationHeaderParticipant> ConversationHeaderParticipants { get; }
		 IDbSet<Feed> Feeds { get; }
		 IDbSet<FollowEntity> FollowEntities { get; }
		 IDbSet<Follower> Followers { get; }
		 IDbSet<Group> Groups { get; }
		 IDbSet<GroupMember> GroupMembers { get; }
		 IDbSet<Post> Posts { get; }
		 IDbSet<PostLike> PostLikes { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}