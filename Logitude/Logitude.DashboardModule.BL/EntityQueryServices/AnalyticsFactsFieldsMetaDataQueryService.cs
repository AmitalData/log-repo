using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.EntityQueryServices
{
    public partial class AnalyticsFactsFieldsMetaDataQueryService
    {

        public List<AnalyticsFactsFieldsMetaDataPM> GetAllByAnalyticsFactsMetaDataId(string analyticsFactsMetaDataId)
        {
            return context.AnalyticsFactsFieldsMetaDatas.Where(x => x.AnalyticsFactsMetaDataId == analyticsFactsMetaDataId).Select(MapPocoToPM()).ToList();
        }

        public List<AnalyticsFactsFieldsMetaDataPM> GetPresetFilters()
        {
            return context.AnalyticsFactsFieldsMetaDatas.Where(x => x.CommonFilterCode != null && x.CommonFilterCode != "").Select(MapPocoToPM()).ToList();
        }

        private static Expression<Func<AnalyticsFactsFieldsMetaData, AnalyticsFactsFieldsMetaDataPM>> MapPocoToPM()
        {
            return x => new AnalyticsFactsFieldsMetaDataPM
            {
                Id = x.Id,
                Tenant = x.Tenant,
                AnalyticsFactsMetaDataId = x.AnalyticsFactsMetaDataId,
                DataTypeCode = x.DataTypeCode,
                CanMeasure = x.CanMeasure,
                CanGroup = x.CanGroup,
                FieldCode = x.FieldCode,
                DisplayName = x.DisplayName,
                DisplayNamePlural = x.DisplayNamePlural,
                JoinedTableName = x.JoinedTableName,
                JoinedTableKey = x.JoinedTableKey,
                JoinedTableDisplayField = x.JoinedTableDisplayField,
                CommonFilterCode = x.CommonFilterCode,
                AllowTenantZeroFilter = x.AllowTenantZeroFilter
            };
        }
    }
}
