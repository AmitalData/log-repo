using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class QueryQuery
    {
         QueryRepository repository;

        public QueryQuery()
        {
            repository = new QueryRepository(); 
        }

        public QueryQuery(int tenant)
        {
            repository = new QueryRepository(tenant);
        }

        public QueryQuery(QueryRepository queryRepository)
        {
            repository = queryRepository;
        }

        public QueryPM GetSingleQueryPM(string Code, int tenant)
        {
            QueryPM result =
            (from a in repository.context.Queries.Include("ObjectTable").Include("QueryGroup").Include("NameTextCode").Include("SharedByUser")
             where a.Code == Code && (a.Tenant == tenant || a.Tenant == 0)
             select new QueryPM()
             {
                 Code = a.Code,
                 DisplayCount = a.DisplayCount,
                 Id = a.Id,
                 IndexOrder = a.IndexOrder,
                 QuerySection = a.QuerySection,
                 ObjectTableId = a.ObjectTableId,
                 ObjectTableName = a.ObjectTable.Name,
                 OriginalQueryId = a.OriginalQueryId,
                 OriginalQueryCode = a.OriginalQueryCode,

                 SystemLevel = a.SystemLevel,
                 Tenant = a.Tenant,
                 TenantLevel = a.TenantLevel,
                 UserId = a.UserId,
                 ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                 IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                 QueryGroupCode = a.QueryGroupCode,
                 QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                 NameTextCodeId = a.NameTextCodeId,
                 NameTextCodeCode = a.NameTextCodeCode,
                 DefaultSortColumn = a.DefaultSortColumn,
                 DefaultSortDirection = a.DefaultSortDirection,
                 SpotlightDataTemplate = a.SpotlightDataTemplate,
                 Agent = a.Agent,
                 Customer = a.Customer,
                 Internal = a.Internal,
                 FeatureId = a.FeatureId,
                 EditWizardName = a.EditWizardName,
                 Perspective = a.Perspective,
                 IsHiddenFromView = a.IsHiddenFromView,
                 IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,               
                 NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
                 EditWizardComponentPath = a.EditWizardComponentPath,
                 SharedWithAll = a.SharedWithAll,
                 SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                 SharedByUserId = a.SharedByUserId,
                 SharedByUserName = a.SharedByUser == null ? null : a.SharedByUser.Contact.EnglishName,
                 SharedByUserEmail = a.SharedByUser == null ? null : a.SharedByUser.Contact.Email,
                 SpotlightModeActivated = a.SpotlightModeActivated,
             }).FirstOrDefault();

            if (result != null)
            {
                SharedUserQueryQuery sharedUserQueryQuery = new SharedUserQueryQuery(tenant);
                result.SharedUserQueries = sharedUserQueryQuery.GetSharedUserQueriesForQuery(result.Id, tenant).ToList();
            }

            return result;
        }

        public QueryPM GetSingleQueryPM(string Code)
        {
            QueryPM result =
            (from a in repository.context.Queries.Include("ObjectTable").Include("QueryGroup").Include("NameTextCode")
             where a.Code == Code
             select new QueryPM()
             {
                 Code = a.Code,
                 DisplayCount = a.DisplayCount,
                 Id = a.Id,
                 IndexOrder = a.IndexOrder,
                 QuerySection = a.QuerySection,
                 ObjectTableId = a.ObjectTableId,
                 ObjectTableName = a.ObjectTable.Name,
                 OriginalQueryId = a.OriginalQueryId,
                 OriginalQueryCode = a.OriginalQueryCode,

                 SystemLevel = a.SystemLevel,
                 Tenant = a.Tenant,
                 TenantLevel = a.TenantLevel,
                 UserId = a.UserId,
                 ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                 IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                 QueryGroupCode = a.QueryGroupCode,
                 QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                 NameTextCodeId = a.NameTextCodeId,
                 NameTextCodeCode = a.NameTextCodeCode,
                 DefaultSortColumn = a.DefaultSortColumn,
                 DefaultSortDirection = a.DefaultSortDirection,
                 SpotlightDataTemplate = a.SpotlightDataTemplate,
                 Agent = a.Agent,
                 Customer = a.Customer,
                 Internal = a.Internal,
                 FeatureId = a.FeatureId,
                 EditWizardName = a.EditWizardName,
                 Perspective = a.Perspective,
                 IsHiddenFromView = a.IsHiddenFromView,
                 IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                 NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
                 EditWizardComponentPath = a.EditWizardComponentPath,
                 SharedWithAll = a.SharedWithAll,
                 SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                 SharedByUserId = a.SharedByUserId,
             }).FirstOrDefault();

            //if (result != null)
            //{
            //    if (!string.IsNullOrEmpty(result.NameTextCodeCode))
            //    {
            //        result.QueryLabel = TranslateTextsClass.Translate(result.NameTextCodeCode, result.Tenant);
            //    }

            //    else
            //    {
            //        result.QueryLabel = result.Code;
            //    }
            //}

            return result;

        }

        public IQueryable<QueryPM> GetQueryPMsByTenant(int tenant)
        {
            List<QueryPM> queries = (from a in repository.context.Queries.Include("ObjectTable").Include("QueryGroup").Include("NameTextCode")
                                     where a.Tenant == tenant
                                     select new QueryPM()
                                     {
                                         Code = a.Code,
                                         DisplayCount = a.DisplayCount,
                                         Id = a.Id,
                                         IndexOrder = a.IndexOrder,
                                         QuerySection = a.QuerySection,
                                         ObjectTableId = a.ObjectTableId,
                                         ObjectTableName = a.ObjectTable.Name,
                                         OriginalQueryId = a.OriginalQueryId,
                                         OriginalQueryCode = a.OriginalQueryCode,

                                         SystemLevel = a.SystemLevel,
                                         Tenant = a.Tenant,
                                         TenantLevel = a.TenantLevel,
                                         UserId = a.UserId,
                                         ObjectTableIsNewWizard = a.ObjectTable.IsNewWizard,
                                         ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                                         IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                                         QueryGroupCode = a.QueryGroupCode,
                                         QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                                         NameTextCodeId = a.NameTextCodeId,
                                         NameTextCodeCode = a.NameTextCodeCode,
                                         DefaultSortColumn = a.DefaultSortColumn,
                                         DefaultSortDirection = a.DefaultSortDirection,
                                         SpotlightDataTemplate = a.SpotlightDataTemplate,
                                         Agent = a.Agent,
                                         Customer = a.Customer,
                                         Internal = a.Internal,
                                         FeatureId = a.FeatureId,
                                         EditWizardName = a.EditWizardName,
                                         Perspective = a.Perspective,
                                         IsHiddenFromView = a.IsHiddenFromView,
                                         IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                                         NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
                                         EditWizardComponentPath = a.EditWizardComponentPath,
                                         SharedWithAll = a.SharedWithAll,
                                         SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                                         SharedByUserId = a.SharedByUserId,
                                         SpotlightModeActivated = a.SpotlightModeActivated,

                                     }).ToList();

            //foreach (QueryPM item in queries)
            //{
            //    if (!string.IsNullOrEmpty(item.NameTextCodeCode))
            //    {
            //        item.QueryLabel = TranslateTextsClass.Translate(item.NameTextCodeCode, item.Tenant);
            //    }

            //    else
            //    {
            //        item.QueryLabel = item.Code;
            //    }
            //}

            return queries.AsQueryable().OrderBy(d => d.IndexOrder);
        }

        public List<QueryPM> GetQueries(int tenant, string userid)
        {
            List<QueryPM> queries = ( from a in repository.context.Queries.Include("ObjectTable").Include("QueryGroup").Include("NameTextCode")
                   where (a.Tenant == tenant && a.UserId == userid) || a.Tenant == 0
                   select new QueryPM()
                   {
                       Code = a.Code,
                       DisplayCount = a.DisplayCount,
                       Id = a.Id,
                       IndexOrder = a.IndexOrder,
                       QuerySection = a.QuerySection,
                       ObjectTableId = a.ObjectTableId,
                       ObjectTableName = a.ObjectTable.Name,
                       OriginalQueryId = a.OriginalQueryId,
                       OriginalQueryCode = a.OriginalQueryCode,

                       SystemLevel = a.SystemLevel,
                       Tenant = a.Tenant,
                       TenantLevel = a.TenantLevel,
                       UserId = a.UserId,
                       ObjectTableIsNewWizard = a.ObjectTable.IsNewWizard,
                       ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                       IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                       QueryGroupCode = a.QueryGroupCode,
                       QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                       NameTextCodeId = a.NameTextCodeId,
                       NameTextCodeCode = a.NameTextCodeCode,
                       DefaultSortColumn = a.DefaultSortColumn,
                       DefaultSortDirection = a.DefaultSortDirection,
                       SpotlightDataTemplate = a.SpotlightDataTemplate,
                       Agent = a.Agent,
                       Customer = a.Customer,
                       Internal = a.Internal,
                       FeatureId = a.FeatureId,
                       EditWizardName = a.EditWizardName,
                       Perspective = a.Perspective,
                       IsHiddenFromView = a.IsHiddenFromView,
                       IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                       NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
                       EditWizardComponentPath = a.EditWizardComponentPath,
                       SharedWithAll = a.SharedWithAll,
                       SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                       SharedByUserId = a.SharedByUserId,
                       SpotlightModeActivated = a.SpotlightModeActivated,
                   }).ToList();
            
            return queries;
        }

        public List<QueryPM> GetQueries_Login(int tenant, string userid)
        {
            List<QueryPM> queries = (from a in repository.context.Queries.Include("ObjectTable").Include("QueryGroup").Include("NameTextCode")
                                     where (a.Tenant == tenant && a.UserId == userid) || a.Tenant == 0 || a.SharedWithAll || a.SharedWithSpecificUsers
                                     select new QueryPM()
                                     {
                                         Code = a.Code,
                                         DisplayCount = a.DisplayCount,
                                         Id = a.Id,
                                         IndexOrder = a.IndexOrder,
                                         QuerySection = a.QuerySection,
                                         ObjectTableId = a.ObjectTableId,
                                         ObjectTableName = a.ObjectTable.Name,
                                         OriginalQueryId = a.OriginalQueryId,
                                         OriginalQueryCode = a.OriginalQueryCode,

                                         SystemLevel = a.SystemLevel,
                                         Tenant = a.Tenant,
                                         TenantLevel = a.TenantLevel,
                                         UserId = a.UserId,
                                         ObjectTableIsNewWizard = a.ObjectTable.IsNewWizard,
                                         ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                                         IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                                         QueryGroupCode = a.QueryGroupCode,
                                         QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                                         NameTextCodeId = a.NameTextCodeId,
                                         NameTextCodeCode = a.NameTextCodeCode,
                                         DefaultSortColumn = a.DefaultSortColumn,
                                         DefaultSortDirection = a.DefaultSortDirection,
                                         SpotlightDataTemplate = a.SpotlightDataTemplate,
                                         Agent = a.Agent,
                                         Customer = a.Customer,
                                         Internal = a.Internal,
                                         FeatureId = a.FeatureId,
                                         EditWizardName = a.EditWizardName,
                                         Perspective = a.Perspective,
                                         IsHiddenFromView = a.IsHiddenFromView,
                                         IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                                         NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
                                         EditWizardComponentPath = a.EditWizardComponentPath,
                                         SharedWithAll = a.SharedWithAll,
                                         SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                                         SharedByUserId = a.SharedByUserId,
                                         SpotlightModeActivated = a.SpotlightModeActivated,
                                     }).ToList();

            List<QueryPM> myResult = new List<QueryPM>();
            List<SharedUserQuery> sharedUserQueries = repository.context.SharedUserQueries.Where(d => d.Tenant == tenant).ToList();

            foreach (QueryPM item in queries)
            {
                if (item.SharedWithSpecificUsers)
                {
                    if (sharedUserQueries.Where(d => d.QueryCode == item.Id && d.UserId == userid).Any())
                    {
                        myResult.Add(item);
                    }
                    
                    else if(item.SharedByUserId == userid)
                    {
                        myResult.Add(item);
                    }
                }

                else
                {
                    myResult.Add(item);
                }
            }

            return myResult;
        }
        public IQueryable<QueryPM> GetQueryPMsByTenantSystemLevel(int tenant)
        {
            List<QueryPM> queries = (from a in repository.context.Queries.Include("ObjectTable").Include("QueryGroup").Include("NameTextCode")
                                     where a.Tenant == tenant && a.SystemLevel == true && a.UserId == null
                                     select new QueryPM()
                                     {
                                         Code = a.Code,
                                         DisplayCount = a.DisplayCount,
                                         Id = a.Id,
                                         IndexOrder = a.IndexOrder,
                                         QuerySection = a.QuerySection,
                                         ObjectTableId = a.ObjectTableId,
                                         ObjectTableName = a.ObjectTable.Name,
                                         OriginalQueryId = a.OriginalQueryId,
                                         OriginalQueryCode = a.OriginalQueryCode,

                                         SystemLevel = a.SystemLevel,
                                         Tenant = a.Tenant,
                                         TenantLevel = a.TenantLevel,
                                         UserId = a.UserId,
                                         ObjectTableIsNewWizard = a.ObjectTable.IsNewWizard,
                                         ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                                         IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                                         QueryGroupCode = a.QueryGroupCode,
                                         QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                                         NameTextCodeId = a.NameTextCodeId,
                                         NameTextCodeCode = a.NameTextCodeCode,
                                         DefaultSortColumn = a.DefaultSortColumn,
                                         DefaultSortDirection = a.DefaultSortDirection,
                                         SpotlightDataTemplate = a.SpotlightDataTemplate,
                                         Agent = a.Agent,
                                         Customer = a.Customer,
                                         Internal = a.Internal,
                                         FeatureId = a.FeatureId,
                                         EditWizardName = a.EditWizardName,
                                         Perspective = a.Perspective,
                                         IsHiddenFromView = a.IsHiddenFromView,
                                         IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                                         NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
                                         EditWizardComponentPath = a.EditWizardComponentPath,
                                         SharedWithAll = a.SharedWithAll,
                                         SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                                         SharedByUserId = a.SharedByUserId,
                                         SpotlightModeActivated = a.SpotlightModeActivated,

                                     }).ToList();

            //foreach (QueryPM item in queries)
            //{
            //    if (!string.IsNullOrEmpty(item.NameTextCodeCode))
            //    {
            //        item.QueryLabel = TranslateTextsClass.Translate(item.NameTextCodeCode, item.Tenant);
            //    }

            //    else
            //    {
            //        item.QueryLabel = item.Code;
            //    }
            //}

            return queries.AsQueryable().OrderBy(d => d.IndexOrder);
        }

        public QueryPM GetQueryByNameTenant(int tenant, string name)
        {
            QueryPM result = (from a in repository.context.Queries.Include("ObjectTable").Include("QueryGroup").Include("NameTextCode")
                              where a.Code == name && a.Tenant == tenant
                              select new QueryPM()
                              {
                                  Code = a.Code,
                                  DisplayCount = a.DisplayCount,
                                  Id = a.Id,
                                  IndexOrder = a.IndexOrder,
                                  QuerySection = a.QuerySection,
                                  ObjectTableId = a.ObjectTableId,
                                  ObjectTableName = a.ObjectTable.Name,
                                  OriginalQueryId = a.OriginalQueryId,
                                  OriginalQueryCode = a.OriginalQueryCode,

                                  SystemLevel = a.SystemLevel,
                                  Tenant = a.Tenant,
                                  TenantLevel = a.TenantLevel,
                                  UserId = a.UserId,
                                  ObjectTableIsNewWizard = a.ObjectTable.IsNewWizard,
                                  ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                                  IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                                  QueryGroupCode = a.QueryGroupCode,
                                  QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                                  NameTextCodeId = a.NameTextCodeId,
                                  NameTextCodeCode = a.NameTextCodeCode,
                                  DefaultSortColumn = a.DefaultSortColumn,
                                  DefaultSortDirection = a.DefaultSortDirection,
                                  SpotlightDataTemplate = a.SpotlightDataTemplate,
                                  Agent = a.Agent,
                                  Customer = a.Customer,
                                  Internal = a.Internal,
                                  FeatureId = a.FeatureId,
                                  EditWizardName = a.EditWizardName,
                                  Perspective = a.Perspective,
                                  IsHiddenFromView = a.IsHiddenFromView,
                                  IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                                  NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
                                  EditWizardComponentPath = a.EditWizardComponentPath,
                                  SharedWithAll = a.SharedWithAll,
                                  SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                                  SharedByUserId = a.SharedByUserId,
                                  SpotlightModeActivated = a.SpotlightModeActivated,

                              }).FirstOrDefault();


            //if (result != null)
            //{
            //    if (!string.IsNullOrEmpty(result.NameTextCodeCode))
            //    {
            //        result.QueryLabel = TranslateTextsClass.Translate(result.NameTextCodeCode, result.Tenant);
            //    }

            //    else
            //    {
            //        result.QueryLabel = result.Code;
            //    }
            //}

            return result;

        }

        public IQueryable<QueryPM> GetQueryByTenantAndUser(int tenant, string userId)
        {
            List<QueryPM> query = null;
            if (!string.IsNullOrEmpty(userId))
            {
                query =
                    (from a in repository.context.Queries.Include("ObjectTable").Include("QueryGroup").Include("NameTextCode")
                     where a.UserId == userId && a.Tenant == tenant
                     select new QueryPM()
                     {
                         Code = a.Code,
                         DisplayCount = a.DisplayCount,
                         Id = a.Id,
                         IndexOrder = a.IndexOrder,
                         QuerySection = a.QuerySection,
                         ObjectTableId = a.ObjectTableId,
                         ObjectTableName = a.ObjectTable.Name,
                         OriginalQueryId = a.OriginalQueryId,
                         OriginalQueryCode = a.OriginalQueryCode,

                         SystemLevel = a.SystemLevel,
                         Tenant = a.Tenant,
                         TenantLevel = a.TenantLevel,
                         UserId = a.UserId,
                         ObjectTableIsNewWizard = a.ObjectTable.IsNewWizard,
                         ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                         IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                         QueryGroupCode = a.QueryGroupCode,
                         QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                         NameTextCodeId = a.NameTextCodeId,
                         NameTextCodeCode = a.NameTextCodeCode,
                         DefaultSortColumn = a.DefaultSortColumn,
                         DefaultSortDirection = a.DefaultSortDirection,
                         SpotlightDataTemplate = a.SpotlightDataTemplate,
                         Agent = a.Agent,
                         Customer = a.Customer,
                         Internal = a.Internal,
                         FeatureId = a.FeatureId,
                         EditWizardName = a.EditWizardName,
                         Perspective = a.Perspective,
                         IsHiddenFromView = a.IsHiddenFromView,
                         IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                         NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
                         EditWizardComponentPath = a.EditWizardComponentPath,
                         SharedWithAll = a.SharedWithAll,
                         SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                         SharedByUserId = a.SharedByUserId,
                         SpotlightModeActivated = a.SpotlightModeActivated,

                     }).ToList();

            }
            else
            {
                query = (from a in repository.context.Queries.Include("ObjectTable").Include("QueryGroup").Include("NameTextCode")
                         where a.Tenant == tenant
                         select new QueryPM()
                         {
                             Code = a.Code,
                             DisplayCount = a.DisplayCount,
                             Id = a.Id,
                             IndexOrder = a.IndexOrder,
                             QuerySection = a.QuerySection,
                             ObjectTableId = a.ObjectTableId,
                             ObjectTableName = a.ObjectTable.Name,
                             OriginalQueryId = a.OriginalQueryId,
                             OriginalQueryCode = a.OriginalQueryCode,

                             SystemLevel = a.SystemLevel,
                             Tenant = a.Tenant,
                             TenantLevel = a.TenantLevel,
                             UserId = a.UserId,
                             ObjectTableIsNewWizard = a.ObjectTable.IsNewWizard,
                             ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                             IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                             QueryGroupCode = a.QueryGroupCode,
                             QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                             NameTextCodeId = a.NameTextCodeId,
                             NameTextCodeCode = a.NameTextCodeCode,
                             DefaultSortColumn = a.DefaultSortColumn,
                             DefaultSortDirection = a.DefaultSortDirection,
                             SpotlightDataTemplate = a.SpotlightDataTemplate,
                             Agent = a.Agent,
                             Customer = a.Customer,
                             Internal = a.Internal,
                             FeatureId = a.FeatureId,
                             EditWizardName = a.EditWizardName,
                             Perspective = a.Perspective,
                             IsHiddenFromView = a.IsHiddenFromView,
                             IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                             NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
                             EditWizardComponentPath = a.EditWizardComponentPath,
                             SharedWithAll = a.SharedWithAll,
                             SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                             SharedByUserId = a.SharedByUserId,
                             SpotlightModeActivated = a.SpotlightModeActivated,

                         }).ToList();

            }


            //foreach (QueryPM item in query)
            //{
            //    if (!string.IsNullOrEmpty(item.NameTextCodeCode))
            //    {
            //        item.QueryLabel = TranslateTextsClass.Translate(item.NameTextCodeCode, item.Tenant);
            //    }

            //    else
            //    {
            //        item.QueryLabel = item.Code;
            //    }
            //}

            return query.AsQueryable().OrderBy(d => d.IndexOrder);

        }
        
        public List<QueryPM> GetQueriesByObjectTableAndUserId(string userid, string objectTableId ,int tenant)
        {
            List<QueryPM> queries = (from a in repository.context.Queries.Include("NameTextCode")
                                     where (a.Tenant == tenant && a.UserId == userid && a.ObjectTableId == objectTableId) 
                                     select new QueryPM()
                                     {
                                         Code = a.Code,
                                         DisplayCount = a.DisplayCount,
                                         Id = a.Id,
                                         IndexOrder = a.IndexOrder,
                                         QuerySection = a.QuerySection,
                                         ObjectTableId = a.ObjectTableId,
                                         ObjectTableName = a.ObjectTable.Name,
                                         OriginalQueryId = a.OriginalQueryId,
                                         OriginalQueryCode = a.OriginalQueryCode,

                                         SystemLevel = a.SystemLevel,
                                         Tenant = a.Tenant,
                                         TenantLevel = a.TenantLevel,
                                         UserId = a.UserId,
                                         IsAddNewEntityEnabled = a.IsAddNewEntityEnabled,
                                         QueryGroupCode = a.QueryGroupCode,
                                         NameTextCodeId = a.NameTextCodeId,
                                         NameTextCodeCode = a.NameTextCodeCode,
                                         DefaultSortColumn = a.DefaultSortColumn,
                                         DefaultSortDirection = a.DefaultSortDirection,
                                         SpotlightDataTemplate = a.SpotlightDataTemplate,
                                         Agent = a.Agent,
                                         Customer = a.Customer,
                                         Internal = a.Internal,
                                         FeatureId = a.FeatureId,
                                         EditWizardName = a.EditWizardName,
                                         Perspective = a.Perspective,
                                         IsHiddenFromView = a.IsHiddenFromView,
                                         IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                                         EditWizardComponentPath = a.EditWizardComponentPath,
                                         SharedWithAll = a.SharedWithAll,
                                         SharedWithSpecificUsers = a.SharedWithSpecificUsers,
                                         SharedByUserId = a.SharedByUserId,
                                         SpotlightModeActivated = a.SpotlightModeActivated,

                                     }).ToList();

      

            return queries;

        }
    }
}