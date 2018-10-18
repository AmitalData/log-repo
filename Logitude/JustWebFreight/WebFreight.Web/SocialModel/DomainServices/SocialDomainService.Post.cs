using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityLists;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.SocialModel.DomainServices
{
    public partial class SocialDomainService
    {
        public PostPM GetSinglePostPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            socialContext = SocialContext.GetContext(tenant);
            postQuery = new PostQueryService(socialContext);
            PostPM post = postQuery.GetSingle(id, false, false);
            return post;
        }

        public PostList GetSinglePostList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            PostListQueryService listService = new PostListQueryService(socialContext);
            return listService.GetSingle(id);
        }


        public List<PostPM> GetPostPMs(byte[] filters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            MemoryStream memorystream = new MemoryStream(filters);
            XmlSerializer serializer = new XmlSerializer(typeof(PostFilters));
            PostFilters postFilters = (PostFilters)serializer.Deserialize(memorystream);

            PostQueryService service = new PostQueryService(socialContext);
            return service.GetPostPMsByFilter(postFilters, true, tenant);
        }

        public List<PostList> GetMainScreenPosts(string userId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            PostListQueryService listService = new PostListQueryService(socialContext);
            return listService.GetMainScreenPosts(userId, tenant);
        }

        public List<PostList> GetPostLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            PostListQueryService listService = new PostListQueryService(socialContext);
            return listService.GetList(tenant);
        }

        public List<PostList> GetPostsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            PostListQueryService listService = new PostListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetPostFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            PostListQueryService queryService = new PostListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertPost(PostPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);
           
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }


            PostUpdateService service = new PostUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (PostLikePM postcomm in entityPm.PostLikes)
            {
                postcomm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }

            service.Update(entityPm, true);


         

        }

        public void UpdatePost(PostPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Post", "UPDATE", entityPm.Tenant);

            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", entityPm.Tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }


            PostUpdateService service = new PostUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetPostLikeChangeSet(entityPm);
            service.Update(entityPm, true);

          
        }

        private void SetPostLikeChangeSet(PostPM entityPm)
        {
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactrep = new ContactRepository(entityPm.Tenant);
            Contact currentUserContact = contactrep.GetSingleContactByEmailAndTenant(email, entityPm.Tenant);

            postLikeQuery = new PostLikeQueryService(socialContext);



            List<PostLikePM> PostLikechangeset = ChangeSet.GetAssociatedChanges(entityPm, d => d.PostLikes).Cast<PostLikePM>().ToList();
            foreach (PostLikePM itemPM in PostLikechangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                             
                            PostLikePM currentItemPM = entityPm.PostLikes.Where(d => d.PostId == itemPM.PostId && d.UserId == currentUserContact.Id).FirstOrDefault();
                            if (currentItemPM != null)
                            {
                                currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            }
                           
                            
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            PostLikePM currentItemPM = entityPm.PostLikes.Where(d => d.PostId == itemPM.PostId && d.UserId == currentUserContact.Id).FirstOrDefault();
                            if (currentItemPM != null)
                            {
                                currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            }
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            PostLikePM currentItemPM = new PostLikePM() { ChangeSetOp = ChangeSetOperation.Delete, PostId = itemPM.PostId, UserId = itemPM.UserId };

                            if (currentItemPM != null)
                            {
                                entityPm.DeletedPostLikes.Add(currentItemPM);
                                currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            }
                            break;
                        }
                    default:
                        {
                      
                            PostLikePM currentItemPM = entityPm.PostLikes.Where(d => d.PostId == itemPM.PostId).FirstOrDefault();
                            if (currentItemPM != null)
                            {
                                currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            }
                            break;

                        }
                }
            }
        }

        [Invoke]
        public int Getlikesreceived(string createdById, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            PostLikeListQueryService listService = new PostLikeListQueryService(socialContext);
            return listService.Userlikesreceived(createdById, tenant);
        }


        [Invoke]
        public int GetCountPostPMsQuery(byte[] filters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "SOCIAL", tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            MemoryStream memorystream = new MemoryStream(filters);
            XmlSerializer serializer = new XmlSerializer(typeof(PostFilters));
            PostFilters postFilters = (PostFilters)serializer.Deserialize(memorystream);

            PostQueryService service = new PostQueryService(socialContext);
            return service.GetCountPostPMsByFilter(postFilters, true, tenant);
        }


    }
}