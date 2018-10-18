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

    public partial class CollateralsRequestFileCondListQueryService
    {
        private IQueryable<CollateralsRequestFileCondList> GetIqueryableList(IQueryable<CollateralsRequestFileCond> iQueryable)
        {
            IQueryable<CollateralsRequestFileCondList> query = (from a in iQueryable
                                                                     select new CollateralsRequestFileCondList()
                                                             {
                                                                RequestedAmount= a.RequestedAmount,
                                                                ConditionCode = a.ConditionCode,
                                                                CustomsCollateralId = a.CustomsCollateralId,
                                                                Tenant = a.Tenant,
                                                                

                                                             });
            return query;
		}

        private IQueryable<CollateralsRequestFileCond> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CollateralsRequestFileCond> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	