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

    public partial class OpportunityTypeListQueryService
    {
	    private IQueryable<OpportunityTypeList> GetIqueryableList(IQueryable<OpportunityType> iQueryable)
        {
            IQueryable<OpportunityTypeList> query = (from a in iQueryable
                                                     select new OpportunityTypeList()
                                                  {
                                                      Code = a.Code,
                                                      Name = a.Name,
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      InActive = a.InActive,
                                                      SearchFields = a.SearchFields,
                                                  });
            return query;
		}

        private IQueryable<OpportunityType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<OpportunityType> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<OpportunityType> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<OpportunityType> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}
}
	