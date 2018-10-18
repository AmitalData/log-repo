 
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
   public partial class FollowerRepository:IRepository<Follower>
   {
   
        private ISocialContext currentContext;
        public FollowerRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public FollowerRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  Follower GetSingle(string followeeuserid, string followeruserid, int tenant)
        {
            return (from a in context.Followers
                    where a.FolloweeUserId == followeeuserid && a.FollowerUserId == followeruserid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Follower> GetAll(int tenant)
        {
            return from a in context.Followers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Follower GetSingle(EntityKeyFields entityKeys)
        {
            FollowerKeys keys = entityKeys as FollowerKeys;
            return (from a in context.Followers
                    where a.FolloweeUserId == keys.FolloweeUserId && a.FollowerUserId == keys.FollowerUserId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Follower entity)
        {
            onAdd();
            context.Followers.Add(entity);
        }

        public void Remove(Follower entity)
        {
            context.Followers.Attach(entity);
            context.Followers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Follower entity)
        {
            onUpdate();
            context.Followers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Follower> All()
        {
            return context.Followers.ToList();
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
	 