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

    public partial class ClaimsRelatedEntsExpDeclarListQueryService
    {
	    private IQueryable<ClaimsRelatedEntsExpDeclarList> GetIqueryableList(IQueryable<ClaimsRelatedEntsExpDeclar> iQueryable)
        {
		IQueryable<ClaimsRelatedEntsExpDeclarList> query = (from a in iQueryable
                                            select new ClaimsRelatedEntsExpDeclarList()
											{
                                                ClaimId = a.ClaimId,
                                                Tenant = a.Tenant,
					                            CounterKey = a.CounterKey,
                                                ExportDeclarationNumber = a.ExportDeclarationNumber,
		                    	            });
            return query;
		}

		private IQueryable<ClaimsRelatedEntsExpDeclar> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClaimsRelatedEntsExpDeclar> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	