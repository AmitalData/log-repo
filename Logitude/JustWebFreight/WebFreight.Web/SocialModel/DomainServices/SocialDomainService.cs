
namespace WebFreight.Web.SocialModel.DomainServices
{
    using Logitude.Social.BL.EntityQueryServices;
    using Logitude.Social.Data;
    using Logitude.Social.Data.Repsitories;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    using System.Data.Entity.Core;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using WebFreight.Web.Helpers;


    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public partial class SocialDomainService : LogitudeDomainService
    {
        ISocialContext socialContext;


        // Repositories
        private FeedRepository feedRepository;
        private FollowEntityRepository followEntityRepository;
        private FollowerRepository followersRepository;
        private GroupMemberRepository groupMembersRepository;
        private GroupRepository groupRepository;
        private PostLikeRepository postLikeRepository;
        private PostRepository postRepository;



        // Queries
        private FeedQueryService feedQuery;
        private FollowEntityQueryService followEntityQuery;
        private FollowerQueryService followerQuery;
        private GroupMemberQueryService groupMemberQuery;
        private GroupQueryService groupQuery;
        private PostLikeQueryService postLikeQuery;
        private PostQueryService postQuery;



        protected override bool PersistChangeSet()
        {
            try
            {
                socialContext.SaveChanges();
            }

            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception("Sorry you can't update this record right now it's being updated by another user");
            }

            return base.PersistChangeSet();
        }
    }
}


//SocialDomainService.Feed.cs

//SocialDomainService.FollowEntity.cs
//SocialDomainService.Follower.cs


//SocialDomainService.GroupMember.cs
//SocialDomainService.Group.cs

//SocialDomainService.PostLike.cs
//SocialDomainService.Post.cs



