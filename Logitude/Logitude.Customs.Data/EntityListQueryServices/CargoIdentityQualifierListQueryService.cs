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

    public partial class CargoIdentityQualifierListQueryService
    {
	    private IQueryable<CargoIdentityQualifierList> GetIqueryableList(IQueryable<CargoIdentityQualifier> iQueryable)
        {
            IQueryable<CargoIdentityQualifierList> query = (from a in iQueryable
                                                            select new CargoIdentityQualifierList()
                                                         {
                                                             Code = a.Code,
                                                             EnglishName = a.EnglishName,
                                                             LocalName = a.LocalName,
                                                             SearchFields = a.SearchFields,
                                                             InActive = a.InActive

                                                         });
            return query;
		}

		private IQueryable<CargoIdentityQualifier> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoIdentityQualifier> iQueryable)
        {
            return iQueryable;
		}
	}


}
	