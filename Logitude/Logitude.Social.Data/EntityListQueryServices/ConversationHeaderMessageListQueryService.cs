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

    public partial class ConversationHeaderMessageListQueryService
    {
	    private IQueryable<ConversationHeaderMessageList> GetIqueryableList(IQueryable<ConversationHeaderMessage> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<ConversationHeaderMessage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ConversationHeaderMessage> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}
        private IQueryable<ConversationHeaderMessage> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ConversationHeaderMessage> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	