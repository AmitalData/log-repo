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

    public partial class PostLikeListQueryService
    {
	    private IQueryable<PostLikeList> GetIqueryableList(IQueryable<PostLike> iQueryable)
        {
            IQueryable<PostLikeList> query = (from a in iQueryable
                                              select new PostLikeList()
                                                {
                                                  
                                                    PostId = a.PostId,
                                                    UserId =a.UserId,
                                                    Tenant = a.Tenant,
                                                });
            return query;
		

		}

		private IQueryable<PostLike> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<PostLike> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

        public int Userlikesreceived(string createdById, int tenant)
        {
            return (from a in context.PostLikes.Include("Post") where a.Post.CreatedById == createdById && a.Tenant == tenant && a.UserId != createdById select a).Count();
 
        }
        private IQueryable<PostLike> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<PostLike> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	