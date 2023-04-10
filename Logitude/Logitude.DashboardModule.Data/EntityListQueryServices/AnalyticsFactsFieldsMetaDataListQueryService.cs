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

    public partial class AnalyticsFactsFieldsMetaDataListQueryService
    {
        public IQueryable<AnalyticsFactsFieldsMetaDataList> GetIqueryableList(IQueryable<AnalyticsFactsFieldsMetaData> iQueryable)
        {
            IQueryable<AnalyticsFactsFieldsMetaDataList> query = (from a in iQueryable
                                                                  select new AnalyticsFactsFieldsMetaDataList()
                                                                  {
                                                                      Id = a.Id,
                                                                      Tenant = a.Tenant,
                                                                      AnalyticsFactsMetaDataId = a.AnalyticsFactsMetaDataId,
                                                                      DataTypeCode = a.DataTypeCode,
                                                                      CanMeasure = a.CanMeasure,
                                                                      FieldCode = a.FieldCode,
                                                                      DisplayName = a.DisplayName,
                                                                      DisplayNamePlural = a.DisplayNamePlural,
                                                                      JoinedTableName = a.JoinedTableName,
                                                                      JoinedTableKey = a.JoinedTableKey,
                                                                      CanGroup = a.CanGroup,
                                                                      JoinedTableDisplayField = a.JoinedTableDisplayField,
                                                                      AllowTenantZeroFilter = a.AllowTenantZeroFilter,
                                                                  });
            return query;
        }

        private IQueryable<AnalyticsFactsFieldsMetaData> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<AnalyticsFactsFieldsMetaData> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<AnalyticsFactsFieldsMetaData> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<AnalyticsFactsFieldsMetaData> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
