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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class OpportunityCompetitorListQueryService
    {
	    private IQueryable<OpportunityCompetitorList> GetIqueryableList(IQueryable<OpportunityCompetitor> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<OpportunityCompetitor> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OpportunityCompetitor> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

        private IQueryable<OpportunityCompetitor> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<OpportunityCompetitor> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	