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

    public partial class TicketStageListQueryService
    {
	    private IQueryable<TicketStageList> GetIqueryableList(IQueryable<TicketStage> iQueryable)
        {
            IQueryable<TicketStageList> query = (from a in iQueryable
                                                select new TicketStageList()
                                                {
                                                    Id = a.Id,
                                                    Tenant = a.Tenant,
                                                    Code = a.Code,
                                                    Name = a.Name,
                                                    Inactive = a.Inactive,
                                                    SearchFields = a.SearchFields,
                                                });
            return query;
		}

		private IQueryable<TicketStage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TicketStage> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<TicketStage> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TicketStage> iQueryable,int tenant)
        {
			return iQueryable;
		}
	}


}
	