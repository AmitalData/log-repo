	using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

    public partial class TariffProductListQueryService
    {
        private IQueryable<TariffProductList> GetIqueryableList(IQueryable<TariffProduct> iQueryable)
        {
            IQueryable<TariffProductList> query = (from a in iQueryable
                                                   select new TariffProductList()
                                                   {

                                                       Id = a.Id,

                                                       Tenant = a.Tenant,

                                                       Code = a.Code,

                                                       Name = a.Name,

                                                       LocalName = a.LocalName,

                                                       Inactive = a.Inactive,

                                                       SearchFields = a.SearchFields,

                                                   });
            return query;
        }

        private IQueryable<TariffProduct> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<TariffProduct> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<TariffProduct> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<TariffProduct> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	