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

    public partial class ConsignmentInternalTransitionListQueryService
    {
	    private IQueryable<ConsignmentInternalTransitionList> GetIqueryableList(IQueryable<ConsignmentInternalTransition> iQueryable)
        {
            IQueryable<ConsignmentInternalTransitionList> query = (from a in iQueryable

                                                                   select new ConsignmentInternalTransitionList()
                                                        {
                                                            ConsignmentNumber = a.ConsignmentNumber,
                                                            DeclarationId = a.DeclarationId,
                                                            SiteCode = a.SiteCode,
                                                            Tenant = a.Tenant,
                                                            



                                                        });
            return query;
		}

        private IQueryable<ConsignmentInternalTransition> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ConsignmentInternalTransition> iQueryable, int tenant)
        {
            return iQueryable;
        }


	}


}
	