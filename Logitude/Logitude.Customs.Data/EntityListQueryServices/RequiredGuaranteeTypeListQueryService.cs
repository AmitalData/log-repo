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

    public partial class RequiredGuaranteeTypeListQueryService
    {
	    private IQueryable<RequiredGuaranteeTypeList> GetIqueryableList(IQueryable<RequiredGuaranteeType> iQueryable)
        {
            IQueryable<RequiredGuaranteeTypeList> query = (from a in iQueryable
                                                           select new RequiredGuaranteeTypeList()
                                                   {
                                                      GuaranteeAmount = a.GuaranteeAmount,
                                                      GuaranteeId = a.GuaranteeId,
                                                      GuaranteeTypeCode = a.GuaranteeTypeCode,
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,

                                                   });
            return query; 
		}

        private IQueryable<RequiredGuaranteeType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<RequiredGuaranteeType> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
	}


}
	