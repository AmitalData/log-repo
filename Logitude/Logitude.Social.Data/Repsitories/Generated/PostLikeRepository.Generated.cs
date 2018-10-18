 
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

namespace Logitude.Social.Data.Repsitories
{
   public partial class PostLikeRepository:IRepository<PostLike>
   {
   
        private ISocialContext currentContext;
        public PostLikeRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public PostLikeRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  PostLike GetSingle(string postid, string userid, int tenant)
        {
            return (from a in context.PostLikes
                    where a.PostId == postid && a.UserId == userid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PostLike> GetAll(int tenant)
        {
            return from a in context.PostLikes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PostLike GetSingle(EntityKeyFields entityKeys)
        {
            PostLikeKeys keys = entityKeys as PostLikeKeys;
            return (from a in context.PostLikes
                    where a.PostId == keys.PostId && a.UserId == keys.UserId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PostLike entity)
        {
            onAdd();
            context.PostLikes.Add(entity);
        }

        public void Remove(PostLike entity)
        {
            context.PostLikes.Attach(entity);
            context.PostLikes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PostLike entity)
        {
            onUpdate();
            context.PostLikes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PostLike> All()
        {
            return context.PostLikes.ToList();
        }

        private ISocialContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 