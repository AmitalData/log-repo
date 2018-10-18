using Logitude.Server.Tools;
using Logitude.Social.BL.EntityDataMappings;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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

namespace Logitude.Social.BL.EntityQueryServices
{
    public partial class PostQueryService : EntityQueryService<Post, PostKeys, PostPM, object, PostKeys>
    {
        public List<PostPM> GetPostPMsByFilter(PostFilters postFilters, bool getcomposition, int tenant)
        {

             
            PostRepository postRepository = this.Repository as PostRepository;

            List<Post> posts = new List<Post>();
            IQueryable<Post> postsquery = null;
            //All , User, Auto
            if (postFilters.SearchByEntity)
            {
                postsquery = postRepository.GetPostsByEntityId(postFilters, tenant);
            }
            else
            {
                switch (postFilters.QueryName)
                {
                    case "Following":
                        postsquery = postRepository.GetFollowedPostsForUserId(postFilters.UserId, tenant);
                        break;

                    case "ToMe":
                        postsquery = postRepository.GetToUserPosts(postFilters.UserId, tenant);
                        break;

                    case "All":
                        postsquery = postRepository.GetAllPosts(tenant);
                        break;
                    
                    default:
                        postsquery = postRepository.GetFollowedPostsForUserId(postFilters.UserId, tenant);
                        break;

                }
            }

            switch (postFilters.SubQueryName)
            {
                case "All":
                    break;
                case "User":
                    postsquery = postsquery.Where(p => p.IsAutomatic == false);
                    break;
                case "Auto":
                    postsquery = postsquery.Where(p => p.IsAutomatic == true);
                    break;
            }

  
            posts = postsquery.OrderByDescending(p => p.UpdateDate).Skip(postFilters.PageIndex).Take(postFilters.PageSize).ToList();


            List<string> contactIds = new List<string>();
            foreach (Post item in posts)
            {
                if (!string.IsNullOrEmpty(item.CreatedById)) contactIds.Add(item.CreatedById);
            }
            List<Contact> contactLists = null;
            if (contactIds.Count > 0)
            {

          
                ContactRepository contactRepository = new ContactRepository(tenant);
                contactLists = contactRepository.GetContactListsByListids(contactIds, tenant).ToList();
            }

            List<PostPM> postPms = new List<PostPM>();
            ColorIndexRepository colorIndexRepository = new ColorIndexRepository(tenant);
            IQueryable<ColorIndex> ColorIndexList = colorIndexRepository.GetColorIndexs();

            foreach (Post post in posts)
            {
                EntityKeys = new PostKeys() { Id = post.Id };
                PostPM postPM = new PostPM();
                mapping.CustomPOCOToPM(postPM, post);
                mapping.POCOToPM(postPM, post);

                postPM.NumberOfComments = postRepository.GetPostNumberOfComments(post.Id, tenant);
                postPM.NumberOfLikes = postRepository.GetPostNumberOfLikes(post.Id, tenant);

                if (contactLists != null && !string.IsNullOrEmpty(post.CreatedById))
                {
                    Contact contact = contactLists.Where(d => d.Id == post.CreatedById).FirstOrDefault();

                    if (contact != null)
                    {
                        postPM.CreatedByUserName = contact.EnglishName;
                        postPM.DefaultColor = ColorIndexList.Where(d => d.IndexNumber == contact.IndexColor).Select(s => s.Color).FirstOrDefault();
                        postPM.UserImageDetailId = contact.ImageDetailId;
                    }
                }
                if (getcomposition)
                {
                    postPM.PostComments = this.GetMulti(new PostKeys()
                     {
                         Id = post.Id
                     }, false);
                }

                foreach (PostPM item in postPM.PostComments)
                {
                    item.DefaultColor = ColorIndexList.Where(d => d.IndexNumber == item.IndexColor).Select(s => s.Color).FirstOrDefault();

                }
                
                this.GetComposition(EntityKeys, postPM);
             

                postPms.Add(postPM);

            }

            return postPms.OrderByDescending(p => p.UpdateDate).ToList();

            
        }


        public int GetCountPostPMsByFilter(PostFilters postFilters, bool getcomposition, int tenant)
        {
            PostRepository postRepository = this.Repository as PostRepository;
            int CountPost = 0;
            List<Post> posts = new List<Post>();
            IQueryable<Post> postsquery = null;
            //All , User, Auto
            if (postFilters.SearchByEntity)
            {
                postsquery = postRepository.GetCountPostsByEntityId(postFilters, tenant);
            }
            else
            {
                switch (postFilters.QueryName)
                {
                    case "Following":
                        postsquery = postRepository.GetFollowedPostsForUserId(postFilters.UserId, tenant);
                        break;

                    case "ToMe":
                        postsquery = postRepository.GetToUserPosts(postFilters.UserId, tenant);
                        break;

                    case "All":
                        postsquery = postRepository.GetAllPosts(tenant);
                        break;

                    default:
                        postsquery = postRepository.GetFollowedPostsForUserId(postFilters.UserId, tenant);
                        break;

                }
            }

            switch (postFilters.SubQueryName)
            {
                case "All":
                    break;
                case "User":
                    postsquery = postsquery.Where(p => p.IsAutomatic == false);
                    break;
                case "Auto":
                    postsquery = postsquery.Where(p => p.IsAutomatic == true);
                    break;
            }

            CountPost = postsquery.Count();


            return CountPost;


        }

        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, PostPM entityPM)
        {
          

            ISocialContext context = MainContext as SocialContext;
            PostKeys postKeys = entityKeys as PostKeys;
            PostLikeQueryService postLikeQueryService = new PostLikeQueryService(context);
            entityPM.PostLikes = postLikeQueryService.GetMulti(postKeys, true);
            base.GetComposition(entityKeys, entityPM);
           
        }



        //public List<PostPM> GetPMsList(QueryOperations queryOperations, int tenant)
        //{
        //    GenericFilter filter = new GenericFilter();
        //    GenericSort sortClass = new GenericSort();

        //    IQueryable<Post> iQueryable = (from a in context.Posts

        //                                   where a.Tenant == tenant
        //                                   select a);

        //    //iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);

        //    QueryOperations nonListQueryOperation = new QueryOperations();
        //    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
        //    QueryOperations listQueryOperation = new QueryOperations();
        //    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

        //    iQueryable = filter.GetFilteredQuery<Post>(nonListQueryOperation, iQueryable);

        //    int skippedPorts = queryOperations.PageIndex;

        //    IQueryable<PostPM> query2 = GetIqueryablePMsList(iQueryable);

        //    query2 = filter.GetFilteredQuery<PostPM>(listQueryOperation, query2);

        //    if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
        //    {
        //        PropertyInfo propInfo = typeof(PostPM).GetProperty(queryOperations.SortByColumnName);
        //        List<ObjectField> PostObjectFields = ObjectFieldsRepository.GetObjectFieldsByObjectTableName("Post", tenant).ToList();

        //        ObjectField objectField = (from a in PostObjectFields
        //                                   where a.FieldName == queryOperations.SortByColumnName
        //                                   select a).FirstOrDefault();

        //        if (objectField != null)
        //        {
        //            switch (objectField.DataTypeCode.ToLower())
        //            {
        //                case "ntext":
        //                case "text":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<PostPM, string>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "sigdouble":
        //                case "double":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<PostPM, double>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "date":
        //                case "datetime":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<PostPM, DateTime>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "unsinteger":
        //                case "integer":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<PostPM, int>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "boolean":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<PostPM, bool>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "unsdecimal":
        //                case "decimal":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<PostPM, decimal>(queryOperations, query2);
        //                        break;
        //                    }
        //                default:
        //                    {
        //                        query2 = query2.OrderByDescending(d => d.Id);
        //                        break;
        //                    }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        query2 = query2.OrderByDescending(d => d.Id);
        //    }
        //    if (!queryOperations.GetAll)
        //    {
        //        query2 = query2.Skip(skippedPorts);
        //        query2 = query2.Take(queryOperations.PageSize);
        //    }
        //    return query2.ToList();


        //}


        //public int GetPMsListCount(QueryOperations queryOperations, int tenant)
        //{
        //    GenericFilter filter = new GenericFilter();
        //    GenericSort sortClass = new GenericSort();

        //    IQueryable<Post> iQueryable = (from a in context.Posts
        //                                   where a.Tenant == tenant
        //                                   select a);

        //    //iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);

        //    QueryOperations nonListQueryOperation = new QueryOperations();
        //    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
        //    QueryOperations listQueryOperation = new QueryOperations();
        //    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

        //    iQueryable = filter.GetFilteredQuery<Post>(nonListQueryOperation, iQueryable);

        //    IQueryable<PostPM> query2 = GetIqueryablePMsList(iQueryable);

        //    query2 = filter.GetFilteredQuery<PostPM>(listQueryOperation, query2);
        //    int count = query2.Count();
        //    return count;
        //}

        //private IQueryable<PostPM> GetIqueryablePMsList(IQueryable<Post> iQueryable)
        //{
        //    IQueryable<PostPM> query = (from a in iQueryable
        //                                select new PostPM()
        //                                  {
        //                                      BodyText = a.BodyText,
        //                                      CreateDate = a.CreateDate,
        //                                      CreatedById = a.CreatedById,
        //                                      EntityId = a.EntityId,
        //                                      GroupId = a.GroupId,
        //                                      Id = a.Id,
        //                                      IsCancelled = a.IsCancelled,
        //                                      IsPrivate = a.IsPrivate,
        //                                      NumberOfLikes = a.NumberOfLikes,
        //                                      ObjectTableId = a.ObjectTableId,
        //                                      ParentPostId = a.ParentPostId,
        //                                      Tenant = a.Tenant,
        //                                      CreatedByUserName = a.User.Contact.EnglishName,




        //                                  });
        //    return query;
        //}
     
    }
}
