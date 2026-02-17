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

    public partial class FollowEntityListQueryService
    {
	    private IQueryable<FollowEntityList> GetIqueryableList(IQueryable<FollowEntity> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<FollowEntity> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<FollowEntity> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

        public List<FollowEntityList> GetListUserFollowEntity(string entityid, string idobjecttable, int tenant)
        {
            List<FollowEntity> followEntity = new List<FollowEntity>();

            followEntity = (from a in context.FollowEntities.Include("User").Include("User.Contact") where a.EntityId == entityid && a.ObjectTableId == idobjecttable && a.Tenant == tenant select a).ToList();
            //foreach (FollowEntity item in followEntity)
            //{

            //    List<User> user = (from a in context. where a.Id == item.Id select a).ToList();
            //    posts.AddRange(feedPosts);
            //}


            List<FollowEntityList> result = (from a in followEntity
                                             select new FollowEntityList()
                                             {

                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 ObjectTableId = a.ObjectTableId,
                                                 EntityId = a.EntityId,
                                                 FollowerUserId = a.FollowerUserId,
                                                 CreateDate = a.CreateDate,
                                                 IsCancelled = a.IsCancelled,
                                                 CancelledDate = a.CancelledDate,
                                                 FollowerName = a.User.Contact.EnglishName,
                                                    
 

                                             }).ToList();
            return result;


        }

        private IQueryable<FollowEntity> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<FollowEntity> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	