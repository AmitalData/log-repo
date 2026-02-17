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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class NotificationReplyListQueryService
    {
	    private IQueryable<NotificationReplyList> GetIqueryableList(IQueryable<NotificationReply> iQueryable)
        {
		IQueryable<NotificationReplyList> query = (from a in iQueryable
                                            select new NotificationReplyList()
											{
                     
					                          NotificationId = a.NotificationId,
					
					                          Line = a.Line,
					
					                          Tenant = a.Tenant,
					
					                          ResponseToCustoms = a.ResponseToCustoms,
					
					                          RepliedByUserId = a.RepliedByUserId,
					
					                          ReplyDateTime = a.ReplyDateTime,
					
		                    	            });
            return query;
		}

		private IQueryable<NotificationReply> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<NotificationReply> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	