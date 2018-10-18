 
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
   public partial class FollowEntityRepository:IRepository<FollowEntity>
   {
        
		public List<FollowEntity> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public IQueryable<FollowEntity> GetFollowEntitiesByUserId(string userId, int tenant)
        {
            return (from a in context.FollowEntities where a.FollowerUserId == userId && a.Tenant == tenant select a);
        }

        public List<FollowEntity> GetEntityFollowers(string entityId,string objecttableId, int tenant)
        {
            List<FollowEntity> followers = (from a in context.FollowEntities
                                        where a.EntityId == entityId && a.ObjectTableId == objecttableId&& a.Tenant == tenant
                                        select a).ToList();

            return followers;
        }


        public FollowEntity GetFollowEntityByUserId(string userId, string entityId, string objecttableId, int tenant)
        {
            return (from a in context.FollowEntities where a.FollowerUserId == userId && a.Tenant == tenant  && a.EntityId == entityId && a.ObjectTableId == objecttableId select a).FirstOrDefault();
        }
   }

}
   