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

    public partial class ConversationHeaderParticipantListQueryService
    {
	    private IQueryable<ConversationHeaderParticipantList> GetIqueryableList(IQueryable<ConversationHeaderParticipant> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<ConversationHeaderParticipant> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ConversationHeaderParticipant> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}
        private IQueryable<ConversationHeaderParticipant> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ConversationHeaderParticipant> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	