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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class CashBookTypeListQueryService
    {
        private IQueryable<CashBookTypeList> GetIqueryableList(IQueryable<CashBookType> iQueryable)
        {
            IQueryable<CashBookTypeList> query = (from a in iQueryable
                                                  select new CashBookTypeList()
                                                  {
                                                      Code = a.Code,
                                                      LocalName = a.LocalName,
                                                      EnglishName = a.EnglishName,
                                                      Inactive = a.Inactive,
                                                      SearchFields = a.SearchFields,
                                                  });
            return query;
        }

        private IQueryable<CashBookType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CashBookType> iQueryable)
        {
            return iQueryable;
        }

        private IQueryable<CashBookType> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<CashBookType> iQueryable)
        {
            return iQueryable;
        }

    }


}
	