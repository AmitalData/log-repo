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

    public partial class TariffSurchargesUpdateListQueryService
    {
        private IQueryable<TariffSurchargesUpdateList> GetIqueryableList(IQueryable<TariffSurchargesUpdate> iQueryable)
        {
            IQueryable<TariffSurchargesUpdateList> query = (from a in iQueryable
                                                            select new TariffSurchargesUpdateList()
                                                            {

                                                                Id = a.Id,

                                                                Tenant = a.Tenant,

                                                                CreateDate = a.CreateDate,

                                                                CreatedByUserId = a.CreatedByUserId,

                                                            });
            return query;
        }

        private IQueryable<TariffSurchargesUpdate> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<TariffSurchargesUpdate> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<TariffSurchargesUpdate> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<TariffSurchargesUpdate> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	