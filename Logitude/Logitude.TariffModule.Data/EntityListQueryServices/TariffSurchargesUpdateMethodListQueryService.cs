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

using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityLists;

namespace Logitude.TariffModule.Data.EntityListQueryServices
{

    public partial class TariffSurchargesUpdateMethodListQueryService
    {
        private IQueryable<TariffSurchargesUpdateMethodList> GetIqueryableList(IQueryable<TariffSurchargesUpdateMethod> iQueryable)
        {
            IQueryable<TariffSurchargesUpdateMethodList> query = (from a in iQueryable
                                                                  select new TariffSurchargesUpdateMethodList()
                                                                  {

                                                                      Code = a.Code,

                                                                      Name = a.Name,

                                                                      SearchFields = a.SearchFields,

                                                                  });
            return query;
        }

        private IQueryable<TariffSurchargesUpdateMethod> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<TariffSurchargesUpdateMethod> iQueryable)
        {
            return iQueryable;
        }
        private IQueryable<TariffSurchargesUpdateMethod> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<TariffSurchargesUpdateMethod> iQueryable)
        {
            return iQueryable;
        }

    }
}
	