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

using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.EntityLists;

namespace Logitude.Social.Data.EntityListQueryServices
{
    public partial class PostListQueryService
    {
        private IQueryable<PostList> GetIqueryableList(IQueryable<Post> iQueryable)
        {
            IQueryable<PostList> query = (from a in iQueryable
                                          select new PostList()
                                                       {
                                                           BodyText = a.BodyText,
                                                           CreateDate = a.CreateDate,
                                                           CreatedById = a.CreatedById,
                                                           EntityId = a.EntityId,
                                                           GroupId = a.GroupId,
                                                           Id = a.Id,
                                                           IsCancelled = a.IsCancelled,
                                                           IsPrivate = a.IsPrivate,
                                                           NumberOfLikes = a.NumberOfLikes,
                                                           ObjectTableId = a.ObjectTableId,
                                                           ParentPostId = a.ParentPostId,
                                                           Tenant = a.Tenant,
                                                           CreatedByUserName = a.User.Contact.EnglishName,




                                                       });
            return query;
        }

		private IQueryable<Post> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Post> iQueryable,int tenant)
        {
            return iQueryable;
		}

        public List<PostList> GetMainScreenPosts(string userId, int tenant)
        {
            List<Post> posts = new List<Post>();
            //List<Feed> feeds = new List<Feed>();

            //IQueryable<Follower> Followers = from a in context.Followers where a.Tenant == tenant && a.FollowerUserId == userId select a ;

            //foreach (Follower item in Followers)
            //{
            List<Feed> userFeeds = (from a in context.Feeds where a.UserId == userId select a).OrderBy(f => f.PostDate).ToList();

            //    feeds.AddRange(userFeeds);
            //}

            foreach (Feed item in userFeeds)
            {
                List<Post> feedPosts = (from a in context.Posts.Include("User").Include("User.Contact") where a.Id == item.PostId select a).ToList();
                posts.AddRange(feedPosts);
            }

            List<Post> userPosts = (from a in context.Posts.Include("User").Include("User.Contact") where a.CreatedById == userId select a).ToList();
           
            posts = posts.Concat(userPosts).OrderByDescending(p => p.CreateDate).ToList();

            List<PostList> result = (from a in posts
                                     select new PostList()
                                     {
                                         BodyText = a.BodyText,
                                         CreateDate = a.CreateDate,
                                         CreatedById = a.CreatedById,
                                         EntityId = a.EntityId,
                                         GroupId = a.GroupId,
                                         Id = a.Id,
                                         IsCancelled = a.IsCancelled,
                                         IsPrivate = a.IsPrivate,
                                         NumberOfLikes = a.NumberOfLikes,
                                         ObjectTableId = a.ObjectTableId,
                                         ParentPostId = a.ParentPostId,
                                         Tenant = a.Tenant,
                                         CreatedByUserName = a.User.Contact.EnglishName,
                                     }).ToList();
            return result;
        }

        private IQueryable<Post> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Post> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	