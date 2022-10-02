using Logitude.Server.Tools.Counters;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Logitude.Social.BL.EntityUpdateServices
{
    public partial class PostUpdateService
    {
        protected override void OnCreating(PostPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("Post", entityPM.Tenant);
               entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
               entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }

        protected override void OnUpdating(PostPM entityPM)
        {
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                
                

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }


        protected override void UpdateComposition(PostPM entityPM)
        {
            PostLikeUpdateService postLikeUpdateService = new PostLikeUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            postLikeUpdateService.UpdateMulti(entityPM.PostLikes, entityPM.DeletedPostLikes, entityPM, false);

            base.UpdateComposition(entityPM);
        }

        protected override void AfterUpdating(PostPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            //ContactRepository contactrep = new ContactRepository(entityPM.Tenant);
            //string email = HttpContext.Current.User.Identity.Name;
            //Contact currentUserContact = contactrep.GetSingleContactByEmail(email, entityPM.Tenant);

            //BrokeredMessage message = new BrokeredMessage();
            //message.Properties["PostId"] = entityPM.Id;
            //message.Properties["Email"] = currentUserContact.Email;
            //message.Properties["Tenant"] = entityPM.Tenant;


            //string taxqueueName = WebFreightEntryPoint.GetQueueByEnviroment("socialqueue");


            //QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(taxqueueName);
            //client.Send(message);


            ISocialContext socialContext = MainContext as ISocialContext;
            FollowerQueryService followerQueryService = new FollowerQueryService(socialContext);
            FollowEntityQueryService followEntityQueryService = new FollowEntityQueryService(socialContext);
            PostRepository postRepository = new PostRepository(socialContext);
            ContactRepository contactrep = new ContactRepository(entityPM.Tenant);
            UserRepository userrep = new UserRepository(entityPM.Tenant);

            string email = HttpContext.Current.User.Identity.Name;
            Contact currentUserContact = contactrep.GetSingleContactByEmail(email, entityPM.Tenant);
            User currentUser = userrep.GetSingleUserByEmail(email, entityPM.Tenant);
            if (currentUser == null)
            {
                currentUser = userrep.GetSingleUserByEmail("system@tenant" + entityPM.Tenant + ".com", entityPM.Tenant);
            }
            if (currentUserContact == null)
            {
                currentUserContact = contactrep.GetSingleContactByEmail("system@tenant" + entityPM.Tenant + ".com", entityPM.Tenant);
            }
            string postId = entityPM.ParentPostId != null ? entityPM.ParentPostId : entityPM.Id;

            System.Collections.Generic.List<FeedPM> addedFeeds = new List<FeedPM>();

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (!string.IsNullOrEmpty(entityPM.ParentPostId))
                {
                    Post parentPost = postRepository.GetSingle(entityPM.ParentPostId, entityPM.Tenant);
                    parentPost.NumberOfComments = parentPost.NumberOfComments + 1;
                    parentPost.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    postRepository.Update(parentPost);
                }

                CreateFeed(entityPM, postId, currentUser.Id, addedFeeds);
            }

            if (!entityPM.IsAutomatic) // if automatic don't build feeds for user followers
            {
                List<FollowerPM> followers = followerQueryService.GetUserFollowers(currentUser.Id, entityPM.Tenant);
                foreach (FollowerPM fol in followers)
                {
                    CreateFeed(entityPM, postId, fol.FollowerUserId, addedFeeds);
                }
            }

            if (!String.IsNullOrEmpty(entityPM.EntityId))
            {
                List<FollowEntityPM> entityFollowers = followEntityQueryService.GetEntityFollowers(entityPM.EntityId, entityPM.ObjectTableId, entityPM.Tenant);

                foreach (FollowEntityPM fol in entityFollowers)
                {
                    CreateFeed(entityPM, postId, fol.FollowerUserId, addedFeeds);
                }

            }
            
            MainContext.SaveChanges();



            base.AfterUpdating(entityPM, entityParentPM);
 
            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    try
            //    {
            //        BrokeredMessage message = new BrokeredMessage();
            //        message.Properties["PostId"] = entityPM.Id;
            //        message.Properties["Email"] = currentUserContact.Email;
            //        message.Properties["Tenant"] = entityPM.Tenant;

            //        string taxqueueName = WebFreightEntryPoint.GetQueueByEnviroment("socialqueue");


            //        QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(taxqueueName);
            //        client.Send(message);
            //    }
            //    catch (Exception e)
            //    {

            //    }
            //}
        }

        private void CreateFeed(PostPM entityPM, string postId, string userId,List<FeedPM> addedFeeds)
        {
            ISocialContext socialContext = MainContext as ISocialContext;
            FeedQueryService feedQueryService = new FeedQueryService(socialContext);
            FeedUpdateService feedUpdateService = new FeedUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
     
            if (!addedFeeds.Where(f => f.PostId == postId && f.UserId == userId).Any())
            {
                FeedPM feed = feedQueryService.GetSingle(postId, userId, false, false);

                if (feed == null)
                {
                    feed = new FeedPM()
                        {
                            PostId = postId,
                            UserId = userId,
                            Tenant = entityPM.Tenant,
                            PostDate = entityPM.CreateDate,
                            UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),//entityPM.CreateDate,
                        };

                    feed.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                }
                else
                {
                    feed.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant).AddDays(-1);

                    feed.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                }


                feedUpdateService.Update(feed, false);

                addedFeeds.Add(feed);
            }

        }


    }
}
