 
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
   public partial class FeedRepository:IRepository<Feed>
   {
   
        private ISocialContext currentContext;
        public FeedRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public FeedRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  Feed GetSingle(string postid, string userid, int tenant)
        {
            return (from a in context.Feeds
                    where a.PostId == postid && a.UserId == userid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Feed> GetAll(int tenant)
        {
            return from a in context.Feeds  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Feed GetSingle(EntityKeyFields entityKeys)
        {
            FeedKeys keys = entityKeys as FeedKeys;
            return (from a in context.Feeds
                    where a.PostId == keys.PostId && a.UserId == keys.UserId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Feed entity)
        {
            onAdd();
            context.Feeds.Add(entity);
        }

        public void Remove(Feed entity)
        {
            context.Feeds.Attach(entity);
            context.Feeds.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Feed entity)
        {
            onUpdate();
            context.Feeds.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Feed> All()
        {
            return context.Feeds.ToList();
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
	 