 
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
   public partial class PostRepository:IRepository<Post>
   {
   
        private ISocialContext currentContext;
        public PostRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public PostRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  Post GetSingle(string id, int tenant)
        {
            return (from a in context.Posts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Post> GetAll(int tenant)
        {
            return from a in context.Posts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Post GetSingle(EntityKeyFields entityKeys)
        {
            PostKeys keys = entityKeys as PostKeys;
            return (from a in context.Posts
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Post entity)
        {
            onAdd();
            context.Posts.Add(entity);
        }

        public void Remove(Post entity)
        {
            context.Posts.Attach(entity);
            context.Posts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Post entity)
        {
            onUpdate();
            context.Posts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Post> All()
        {
            return context.Posts.ToList();
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
	 