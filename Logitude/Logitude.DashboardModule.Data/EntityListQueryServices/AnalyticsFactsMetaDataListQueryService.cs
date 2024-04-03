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

using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityLists;

namespace Logitude.DashboardModule.Data.EntityListQueryServices
{

    public partial class AnalyticsFactsMetaDataListQueryService
    {
        public IQueryable<AnalyticsFactsMetaDataList> GetIqueryableList(IQueryable<AnalyticsFactsMetaData> iQueryable)
        {
            IQueryable<AnalyticsFactsMetaDataList> query = (from a in iQueryable
                                                            select new AnalyticsFactsMetaDataList()
                                                            {
                                                                Id = a.Id,
                                                                Name = a.Name,
                                                                TableName = a.TableName,
                                                                Tenant = a.Tenant,
                                                            });
            return query;
        }

        private IQueryable<AnalyticsFactsMetaData> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<AnalyticsFactsMetaData> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<AnalyticsFactsMetaData> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<AnalyticsFactsMetaData> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
