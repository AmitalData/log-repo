using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebFreight.Web.Controllers.DigitalPortal.Models;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public partial class QuoteQuery
    {
        #region Global Search 
        public IQueryable<QuoteList> GetByFilters(GeneralFilters newFilters)
        {
            var tenant = newFilters.Tenant;

            var filters = new ApiQueryFilters()
            {
                Filter1Value = newFilters.CardId,
                Filter2Value = newFilters.CardType
            };

            var queryOperations = new QueryOperations()
            {
                ObjectTableName = "Quote",
                PageIndex = newFilters.PageIndex,
                PageSize = newFilters.PageSize,
                QuerySection = "Quotes",
                SortByColumnName = newFilters.SortBy,
                SortDirectin = newFilters.SortDirection,
                QueryFilterItems = new List<QueryFilterItem>(),
            };

            string partnerTypeName = string.Empty;

            if (newFilters.CardType == "CS")
            {
                partnerTypeName = "CustomerId";
            }
            else if (newFilters.CardType == "AG")
            {
                partnerTypeName = "AgentId";
            }

            if (!string.IsNullOrEmpty(newFilters.CardId))
            {
                queryOperations.SetFilter(partnerTypeName, newFilters.CardId, false, "InList", null, false);
            }

            var quoteObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Quote", tenant);

            foreach (var filter in newFilters.AdditionalFilters)
            {
                var field = quoteObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                if (field != null)
                {
                    string valuestring1 = filter.FieldValue?.ToString();
                    object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                    string valuestring2 = filter.FieldValue2?.ToString();
                    object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                    string valuestring3 = filter.FieldValue3?.ToString();
                    object value3 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring3);
                    queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                }
                else
                {
                    queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.FieldValue3, filter.DisplayInList);
                }
            }

            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
            ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);
            
            TreeFilterQueryArgs treeFilterQueryArgs = new TreeFilterQueryArgs()
            {
                AdditionalTreeFilter = filters.TreeFilters,
                ObjectTableName = "Quote",
                ParentEntityId = filters.ParentEntityId,
                ParentObjectTableName = filters.ParentObjectTableName,
                Tenant = tenant,
                ParentEntity = filters.ParentEntity
            };

            var MyContext = QuotesContext.GetContext(tenant);
            var quoteRepository = new QuoteRepository(MyContext);
            var entityPocos = quoteRepository.GetQuotes(tenant);
            var quoteQuery = new QuoteQuery(quoteRepository);

            var nonListQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList()
            };

            var listQueryOperation = new QueryOperations
            {
                QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList()
            };

            var genericFilter = new Simplog.Server.Infrastructure.Helpers.GenericFilter();
            var customfilters = new QuoteCustomFilter(tenant);
            entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);
            var quoteBusinessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            entityPocos  = quoteBusinessUnitFilter.RunFilter(entityPocos);
            entityPocos = genericFilter.GetFilteredQuery<Quote>(nonListQueryOperation, entityPocos);
            var quoteStageRepository = new QuoteStageRepository(tenant);
            var quotes_Created = quoteStageRepository.GetQuoteStageIdByCode("QTCR", tenant);
            var quotes_Draft = quoteStageRepository.GetQuoteStageIdByCode("QTDR", tenant);
            entityPocos = entityPocos.Where(d => d.StageId != quotes_Created && d.StageId != quotes_Draft);

            var skippedEntities = queryOperations.PageIndex;
            var entityLists = quoteQuery.GetIQueryableEntityList(entityPocos);
            entityLists = genericFilter.GetFilteredQuery<QuoteList>(listQueryOperation, entityLists);
            entityLists = new TreeFilterQueryService().Apply<QuoteList>(entityLists, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteList).GetProperty(queryOperations.SortByColumnName);
                var objectField = quoteObjectFields.FirstOrDefault(a => a.FieldName == queryOperations.SortByColumnName);
                var sortClass = new Simplog.Server.Infrastructure.Helpers.GenericSort();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        entityLists = sortClass.GetSorterQuery<QuoteList, string>(queryOperations, entityLists);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                            case "lookup":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, string>(queryOperations, entityLists);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, double>(queryOperations, entityLists);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, DateTime>(queryOperations, entityLists);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, int>(queryOperations, entityLists);
                                    break;
                                }
                            case "boolean":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, bool>(queryOperations, entityLists);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    entityLists = sortClass.GetSorterQuery<QuoteList, decimal>(queryOperations, entityLists);
                                    break;
                                }
                            default:
                                {
                                    entityLists = entityLists.OrderBy(d => d.Id);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                entityLists = entityLists.OrderBy(d => d.Id);
            }

            return entityLists;
        }

        #endregion
    }
}
