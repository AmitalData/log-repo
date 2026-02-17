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

    public partial class GuaranteeConditionListQueryService
    {
	    private IQueryable<GuaranteeConditionList> GetIqueryableList(IQueryable<GuaranteeCondition> iQueryable)
        {
            IQueryable<GuaranteeConditionList> query = (from a in iQueryable.Include("ReturnCondition")

                                                        select new GuaranteeConditionList()
                                                           {
                                                              Id = a.Id,
                                                              GuaranteeAmount = a.GuaranteeAmount,
                                                              ReturnConditionCode = a.ReturnConditionCode,
                                                              GuaranteeId = a.GuaranteeId,
                                                              Tenant = a.Tenant,
                                                              ReturnConditionName = a.ReturnCondition != null ? a.ReturnCondition.LocalName : null,



                                                           });
            return query;
		}

        private IQueryable<GuaranteeCondition> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<GuaranteeCondition> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	