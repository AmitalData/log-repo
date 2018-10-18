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

    public partial class CustomsVerificationStatusTypeListQueryService
    {
	    private IQueryable<CustomsVerificationStatusTypeList> GetIqueryableList(IQueryable<CustomsVerificationStatusType> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<CustomsVerificationStatusType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsVerificationStatusType> iQueryable)
        {
			throw new NotImplementedException();
		}
	}


}
	