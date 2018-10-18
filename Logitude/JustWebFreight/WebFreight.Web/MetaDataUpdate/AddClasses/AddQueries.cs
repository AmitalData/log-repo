using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddQueries
    {
        public static Query AddQuery(QueryDetails queryDetails, QueryRepository queryRepository, Dictionary<string, Query> tenantQueries)
        {
            if (tenantQueries.Keys.Contains(queryDetails.Code + queryDetails.ObjectTableId))
            {
                Query query = tenantQueries[queryDetails.Code + queryDetails.ObjectTableId];
                query.DisplayCount = queryDetails.DisplayCount;
                query.IndexOrder = queryDetails.IndexOrder;
                query.IsAddNewEntityEnabled = queryDetails.IsAddNewEntityEnabled;
                query.OriginalQueryId = queryDetails.OriginalQueryId;
                query.QueryGroupCode = queryDetails.QueryGroupCode;
                query.QuerySection = queryDetails.QuerySection;
                query.SystemLevel = queryDetails.SystemLevel;
                query.Tenant = queryDetails.Tenant;
                query.TenantLevel = queryDetails.TenantLevel;
                query.UserId = queryDetails.UserId;
                query.DefaultSortDirection = queryDetails.DefaultSortDirection;
                query.DefaultSortColumn = queryDetails.DefaultSortName;
                query.NameTextCodeId = queryDetails.NameTextCodeId;
                query.SpotlightDataTemplate = queryDetails.SpotlightDataTemplate;
                query.Customer = queryDetails.Customer;
                query.Agent = queryDetails.Agent;
                query.Internal = queryDetails.Internal;
                query.EditWizardName = queryDetails.EditWizardName;
                query.EditWizardComponentPath = queryDetails.EditWizardComponentPath;
                query.FeatureId = queryDetails.FeatureId;
                query.Perspective = queryDetails.Perspective;
                query.IsHiddenFromView = queryDetails.IsHiddenFromView;
                query.IsNewFromTenantZeroOnly = queryDetails.IsNewFromTenantZeroOnly;
                queryRepository.Update(query);
                return query;
            }
            else
            {
                Query newQuery = new Query()
                {
                    UserId = queryDetails.UserId,
                    TenantLevel = queryDetails.TenantLevel,
                    Tenant = queryDetails.Tenant,
                    SystemLevel = queryDetails.SystemLevel,
                    QuerySection = queryDetails.QuerySection,
                    QueryGroupCode = queryDetails.QueryGroupCode,
                    OriginalQueryId = queryDetails.OriginalQueryId,
                    ObjectTableId = queryDetails.ObjectTableId,
                    IsAddNewEntityEnabled = queryDetails.IsAddNewEntityEnabled,
                    IndexOrder = queryDetails.IndexOrder,
                    DisplayCount = queryDetails.DisplayCount,
                    Code = queryDetails.Code,
                    DefaultSortDirection = queryDetails.DefaultSortDirection,
                    DefaultSortColumn = queryDetails.DefaultSortName,
                    Id = IdCounter.GetNumber("Query", queryDetails.Tenant).ToString(),
                    NameTextCodeId = queryDetails.NameTextCodeId,
                    SpotlightDataTemplate = queryDetails.SpotlightDataTemplate,
                    Agent = queryDetails.Agent,
                    Customer = queryDetails.Customer,
                    Internal = queryDetails.Internal,
                    FeatureId = queryDetails.FeatureId,
                    EditWizardName = queryDetails.EditWizardName,
                    EditWizardComponentPath = queryDetails.EditWizardComponentPath,
                    Perspective = queryDetails.Perspective,
                    IsHiddenFromView = queryDetails.IsHiddenFromView,
                    IsNewFromTenantZeroOnly = queryDetails.IsNewFromTenantZeroOnly,
                }; ;
                queryRepository.Add(newQuery);
                return newQuery;
            }
        }

        public static QueryColumn AddQueryColumn(QueryColumnDetails queryColumnDetails, QueryColumnRepository queryColumnRepository, Dictionary<string, QueryColumn> tenantQueryColumn)
        {
            if (tenantQueryColumn.Keys.Contains(queryColumnDetails.QueryId + queryColumnDetails.ObjectFieldId))
            {
                QueryColumn queryColumn = tenantQueryColumn[queryColumnDetails.QueryId + queryColumnDetails.ObjectFieldId];
                queryColumn.ColumnWidth = queryColumnDetails.ColumnWidth;
                queryColumn.IndexOrder = queryColumnDetails.IndexOrder;
                queryColumn.Tenant = queryColumnDetails.Tenant;
                queryColumnRepository.Update(queryColumn);
                return queryColumn;
            }

            else
            {
                QueryColumn newQureyColumn = new QueryColumn()
                {
                    Tenant = queryColumnDetails.Tenant,
                    ObjectFieldId = queryColumnDetails.ObjectFieldId,
                    IndexOrder = queryColumnDetails.IndexOrder,
                    ColumnWidth = queryColumnDetails.ColumnWidth,
                    Id = IdCounter.GetNumber("QueryColumn",queryColumnDetails.Tenant).ToString(),
                    QueryId = queryColumnDetails.QueryId,
                };
                queryColumnRepository.Add(newQureyColumn);
                return newQureyColumn;
            }
        }

       

        public static AdvancedQueryFilter AddAdvancedQueryFilter(AdvancedFilterDetails advancedQueryFilterDetails, AdvancedQueryFilterRepository advancedQueryFilterRepository, Dictionary<string, AdvancedQueryFilter> tenantAdvancedQueryFilter)
        {
            if (tenantAdvancedQueryFilter.Keys.Contains(advancedQueryFilterDetails.QueryId + advancedQueryFilterDetails.ObjectFieldId))
            {
                AdvancedQueryFilter advancedQueryFilter = tenantAdvancedQueryFilter[advancedQueryFilterDetails.QueryId + advancedQueryFilterDetails.ObjectFieldId];

                advancedQueryFilter.IndexOrder = advancedQueryFilterDetails.IndexOrder;
                advancedQueryFilter.IsPredefined = advancedQueryFilterDetails.IsPredefined;
                advancedQueryFilter.Operator = advancedQueryFilterDetails.Operator;
                advancedQueryFilter.PredefinedValue = advancedQueryFilterDetails.PredefinedValue;
                advancedQueryFilter.PredefinedValue2 = advancedQueryFilterDetails.PredefinedValue2;
                advancedQueryFilter.Tenant = advancedQueryFilterDetails.Tenant;
                advancedQueryFilterRepository.Update(advancedQueryFilter);
                return advancedQueryFilter;
            }
            else
            {
                AdvancedQueryFilter newAdvancedQueryFilter = new AdvancedQueryFilter()
                {
                    Tenant = advancedQueryFilterDetails.Tenant,
                    ObjectFieldId = advancedQueryFilterDetails.ObjectFieldId,
                    IndexOrder = advancedQueryFilterDetails.IndexOrder,
                    IsPredefined = advancedQueryFilterDetails.IsPredefined,
                    Operator = advancedQueryFilterDetails.Operator,
                    PredefinedValue = advancedQueryFilterDetails.PredefinedValue,
                    PredefinedValue2 = advancedQueryFilterDetails.PredefinedValue2,
                    Id = IdCounter.GetNumber("AdvancedQueryFilter",advancedQueryFilterDetails.Tenant).ToString(),
                    QueryId = advancedQueryFilterDetails.QueryId,
                };
                advancedQueryFilterRepository.Add(newAdvancedQueryFilter);
                return newAdvancedQueryFilter;
            }
        }
    }
}