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

    public partial class OpportunityProductLocationListQueryService
    {
	    private IQueryable<OpportunityProductLocationList> GetIqueryableList(IQueryable<OpportunityProductLocation> iQueryable)
        {
            IQueryable<OpportunityProductLocationList> query = (from a in iQueryable
                                                                select new OpportunityProductLocationList()
                                                    {
                                                        OpportunityId = a.OpportunityId,
                                                        OpportunityProductTypeCode = a.OpportunityProductType == null ? "" : a.OpportunityProductType.Code,
                                                        LineNumber = a.LineNumber,
                                                        Tenant = a.Tenant, CountryId = a.CountryId,
                                                    });
            return query;
		}

        private IQueryable<OpportunityProductLocation> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<OpportunityProductLocation> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<OpportunityProductLocation> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<OpportunityProductLocation> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	