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

    public partial class CustomsCollateralsConditionListQueryService
    {
	    private IQueryable<CustomsCollateralsConditionList> GetIqueryableList(IQueryable<CustomsCollateralsCondition> iQueryable)
        {
            IQueryable<CustomsCollateralsConditionList> query = (from a in iQueryable
                                                                 select new CustomsCollateralsConditionList()
                                                              {
                                                                  CustomsCollateralId = a.CustomsCollateralId,
                                                                  ConditionCode = a.ConditionCode,
                                                                  RequestedAmount = a.RequestedAmount,
                                                                  Tenant = a.Tenant,
                                                              });
            return query;
		}

        private IQueryable<CustomsCollateralsCondition> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsCollateralsCondition> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
	}


}
	