using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class SocialWorkerRole : WorkerEntryPoint
    {
        private bool serviceStarted = true;
        private int interval = 1;
        private int systemErrorCount = 0;
        QueueDescription queueDescription;
        QueueClient client;

        public override void Run()
        {

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        var message = client.Receive(new TimeSpan(0, 0, 30));
                        LastActivity = DateTime.UtcNow;
                        if (message != null)
                        {

                            try
                            {
                                string postId = message.Properties["PostId"].ToString();
                                string email = message.Properties["Email"].ToString();
                                tenant = (int)message.Properties["Tenant"];

                                CreatePostFeeds(postId, email, tenant);

                                message.Complete();
                                LogDoneItemInMemory();

                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", "", null);
                                if (message.Properties.Keys.Contains("PostId"))
                                {
                                    string fileId = message.Properties["PostId"].ToString();
                                    if (fileId != null)
                                    {
                                        message.Abandon();
                                    }
                                    else
                                    {
                                        message.Complete();
                                    }
                                }
                                else
                                {
                                    message.Complete();
                                }


                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null,null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        ISocialContext socialContext;
        private void CreatePostFeeds(string id, string email, int tenant)
        {
            socialContext = SocialContext.GetContext(tenant);
            FollowerQueryService followerQueryService = new FollowerQueryService(socialContext);
            FollowEntityQueryService followEntityQueryService = new FollowEntityQueryService(socialContext);
            PostQueryService postQueryService = new PostQueryService(socialContext);
            PostRepository postRepository = new PostRepository(socialContext);

            PostPM entityPM = postQueryService.GetSingle(id, false, false);
            ContactRepository contactrep = new ContactRepository(entityPM.Tenant);


            Contact currentUserContact = contactrep.GetSingleContactByEmail(email, entityPM.Tenant);

            string postId = entityPM.ParentPostId != null ? entityPM.ParentPostId : entityPM.Id;

            System.Collections.Generic.List<FeedPM> addedFeeds = new List<FeedPM>();

            //if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            //{
            //    if (!string.IsNullOrEmpty(entityPM.ParentPostId))
            //    {
            //        Post parentPost = postRepository.GetSingle(entityPM.ParentPostId, entityPM.Tenant);
            //        parentPost.NumberOfComments = parentPost.NumberOfComments + 1;
            //        parentPost.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            //        postRepository.Update(parentPost);
            //    }

            //    CreateFeed(entityPM, postId, currentUserContact.Id, addedFeeds);
            //}

            if (!entityPM.IsAutomatic) // if automatic don't build feeds for user followers
            {
                List<FollowerPM> followers = followerQueryService.GetUserFollowers(currentUserContact.Id, entityPM.Tenant);
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




            socialContext.SaveChanges();

        }

        private void CreateFeed(PostPM entityPM, string postId, string userId, List<FeedPM> addedFeeds)
        {

            FeedQueryService feedQueryService = new FeedQueryService(socialContext);
            FeedUpdateService feedUpdateService = new FeedUpdateService(socialContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);

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
                    feed.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                    feed.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                }


                feedUpdateService.Update(feed, false);

                addedFeeds.Add(feed);
            }

        }


        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "Social";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {

                string socialQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment("socialqueue");


                if (!StorageAcountDetails.NameSpaceManager.QueueExists(socialQueueName))
                {
                    queueDescription = new QueueDescription(socialQueueName);
                    queueDescription.MaxSizeInMegabytes = 5120;
                    // queueDescription.DefaultMessageTimeToLive = new TimeSpan(3, 1, 0);

                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(socialQueueName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "social worker role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}

