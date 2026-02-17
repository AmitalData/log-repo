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

    public partial class CustomsBranchListQueryService
    {
        private IQueryable<CustomsBranchList> GetIqueryableList(IQueryable<CustomsBranch> iQueryable)
        {
            IQueryable<CustomsBranchList> query = (from a in iQueryable
                                                   select new CustomsBranchList()
                                            {
                                                Id=a.Id,
                                                Code = a.Code,
                                                EnglishName = a.EnglishName,
                                                LocalName = a.LocalName,
                                                SearchFields = a.SearchFields,
                                                Inactive = a.Inactive,
                                                BankCode = a.BankCode,
                                            
                                            });
            return query;
		}

		private IQueryable<CustomsBranch> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsBranch> iQueryable)
        {
            return iQueryable;
		}
	}


}
	