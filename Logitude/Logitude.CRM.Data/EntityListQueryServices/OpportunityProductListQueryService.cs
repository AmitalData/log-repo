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

    public partial class OpportunityProductListQueryService
    {
	    private IQueryable<OpportunityProductList> GetIqueryableList(IQueryable<OpportunityProduct> iQueryable)
        {
            IQueryable<OpportunityProductList> query = (from a in iQueryable
                                                        select new OpportunityProductList()
                                                            {
                                                               OpportunityId = a.OpportunityId,
                                                               OpportunityProductTypeCode = a.OpportunityProductType == null ? "" : a.OpportunityProductType.Code,
                                                               Tenant = a.Tenant,
                                                               Notes = a.Notes,
                                                               ChargeableWeight = a.ChargeableWeight,
                                                               TEU = a.TEU,
                                                               NumberOfShipments = a.NumberOfShipments, 
                                                               OpportunityProductTypeName = a.OpportunityProductType == null ? "" : a.OpportunityProductType.Name,
                                                            });
            return query;
		}

        private IQueryable<OpportunityProduct> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<OpportunityProduct> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<OpportunityProduct> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<OpportunityProduct> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	