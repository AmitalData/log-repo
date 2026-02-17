 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
namespace Logitude.Social.Data.Repsitories
{
   public partial class PostRepository:IRepository<Post>
   {

       public IQueryable<Post> GetFollowedPostsForUserId(string userId, int tenant)
       {
            IQueryable<Post> userfeedsPosts = (from a in context.Feeds.Include("Post")//.Include("Post.User").Include("Post.User.Contact")
                                               where a.UserId == userId && a.Tenant == tenant && a.Post.ParentPostId == null && a.Post.IsCancelled == false
                                               select a.Post);


           return userfeedsPosts;
   }

       public IQueryable<Post> GetPostsByEntityId(PostFilters postFilters, int tenant)
       {

           IQueryable<Post> posts = null;


           //List<Feed> userFeeds = (from a in context.Feeds where a.UserId == postFilters.UserId && a.Tenant == tenant && a.IsCancelled == false select a).OrderByDescending(f => f.UpdateDate).ToList();

           //FollowEntityRepository followEntityRepository = new FollowEntityRepository(this.context);
           // FollowEntity followEntity = followEntityRepository.GetFollowEntityByUserId(postFilters.UserId, postFilters.EntityId, postFilters.ObjectTableId, tenant);

           //if (followEntity != null)
           //{
           // first get auto posts
           posts = (from a in context.Posts.Include("User").Include("User.Contact")
                    where a.EntityId == postFilters.EntityId && a.ObjectTableId == postFilters.ObjectTableId && a.ParentPostId == null && a.IsCancelled == false && a.Tenant == tenant
                    select a);

           ////second get manual posts for the users i am following
           //foreach (Feed item in userFeeds)
           //{
           //    List<Post> feedPosts = (from a in context.Posts.Include("User").Include("User.Contact")
           //                            where a.Id == item.PostId && a.ParentPostId == null && a.IsCancelled == false && a.Tenant == tenant && a.IsAutomatic == false && a.EntityId == postFilters.EntityId && a.ObjectTableId == postFilters.ObjectTableId
           //                            select a).ToList();

           //    posts = posts.Concat(feedPosts);
           //}
           //}
           //else
           //{
           //    // get the posts created by me only
           //    posts = (from a in context.Posts.Include("User").Include("User.Contact")
           //             where a.CreatedById == postFilters.UserId && a.EntityId == postFilters.EntityId && a.ObjectTableId == postFilters.ObjectTableId && a.ParentPostId == null && a.IsCancelled == false && a.Tenant == tenant
           //             select a);
           //}


           return posts;
       }

        public IQueryable<Post> GetCountPostsByEntityId(PostFilters postFilters, int tenant)
        {
            IQueryable<Post> posts = null;
            posts = (from a in context.Posts
                    where a.EntityId == postFilters.EntityId && a.ObjectTableId == postFilters.ObjectTableId && a.ParentPostId == null && a.IsCancelled == false && a.Tenant == tenant
                    select a);

        return posts;
   }

       public IQueryable<Post> GetToUserPosts(string userId, int tenant)
       {
           // created by me and has comments
           IQueryable<Post> posts = (from a in context.Posts
                               where a.CreatedById == userId && a.ParentPostId == null && a.IsCancelled == false && a.Tenant == tenant && a.NumberOfComments > 0
                               select a);


           return posts;
       }



       public IQueryable<Post> GetAllPosts(int tenant)
       {
           IQueryable<Post> posts = (from a in context.Posts
                                     where a.ParentPostId == null && a.IsCancelled == false && a.Tenant == tenant && (a.IsAutomatic == false || (a.IsAutomatic == true && a.NumberOfComments > 0))//&& a.IsPrivate == false
                                     select a);

           return posts;
       }





       public List<Post> GetUserPosts(string userId, int tenant)
       {
           List<Post> posts = (from a in context.Posts.Include("User").Include("User.Contact")
                               where a.CreatedById == userId && a.ParentPostId == null && a.IsCancelled == false && a.Tenant == tenant
                               select a).OrderByDescending(f => f.UpdateDate).ToList();




           return posts;
       }

     


       
        
		public List<Post> GetMulti(EntityKeyFields entityKeys)
        {

            PostKeys postKeys = entityKeys as PostKeys;

            return (from a in context.Posts
                    where a.ParentPostId == postKeys.Id 
                    select a).ToList();
        }

        public int GetPostNumberOfComments(string id, int tenant)
        {
            return context.Posts.Where(p => p.ParentPostId == id && p.Tenant == tenant && p.IsCancelled==false).Count();
        }
        public int GetPostNumberOfLikes(string id, int tenant)
        {
            return context.PostLikes.Where(p => p.PostId == id && p.Tenant == tenant).Count();
        }



       public string GetDeflutColor(string id , int tenant)
       {

           string color="";
             ContactRepository contactRepository = new ContactRepository(tenant);
           Contact contact = contactRepository.GetSingleContact(id, tenant);

           if (contact != null)
           {
               ColorIndexRepository colorIndexRepository = new ColorIndexRepository(tenant);
               color =   colorIndexRepository.GetSingleHasColor(contact.IndexColor);
           }

           return color;
       }

       public IQueryable<Contact> GetAllContact(int tenant)
       {

          ContactRepository contactRepository = new ContactRepository(tenant);
          IQueryable<Contact> contact = contactRepository.GetActiveContacts(tenant);


          return contact;
       }
       

   }

}
   