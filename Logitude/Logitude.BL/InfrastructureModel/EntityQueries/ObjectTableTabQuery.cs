using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ObjectTableTabQuery
    {
        ObjectTableTabRepository repository;
        public ObjectTableTabQuery()
        {
            repository = new ObjectTableTabRepository(); 
        }

        public ObjectTableTabQuery(int tenant)
        {
            repository = new ObjectTableTabRepository(tenant);
        }

        public ObjectTableTabQuery(ObjectTableTabRepository objectTableTabRepository )
        {
            repository = objectTableTabRepository;
        }

        public IQueryable<ObjectTableTabPM> GetObjectTableTabPMsByTenant(int tenant)
        {
            IQueryable<ObjectTableTabPM> tabs = from a in repository.context.ObjectTableTabs.Include("TabNameTextCode").Include("ObjectTable")
                                                where a.Tenant == 0//a.Tenant == tenant
                                                select new ObjectTableTabPM()
                                                {
                                                    ControlPath = a.ControlPath,
                                                    Id = a.Id,
                                                    IndexOrder = a.IndexOrder,
                                                    ObjectTableId = a.ObjectTableId,
                                                    TabNameTextCodeDefaultText = a.TabNameTextCode.DefaultText,
                                                    TabNameTextCodeId = a.TabNameTextCodeId,
                                                    Tenant = a.Tenant,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    TabNameTextCodeCode = a.TabNameTextCodeCode,
                                                    Code = a.Code,
                                                    FeatureId = a.FeatureId,
                                                    HtmlComponentName = a.HtmlComponentName,
                                                    HtmlComponentUrl = a.HtmlComponentUrl,
                                                   
                                                };
            return tabs;
        }

        public IQueryable<ObjectTableTabPM> GetObjectTableTabsByTenantAndObjectTable(string objectTableId, int tenant)
        {
            IQueryable<ObjectTableTabPM> tabs = from a in repository.context.ObjectTableTabs.Include("TabNameTextCode").Include("ObjectTable")
                                                where a.Tenant == tenant && a.ObjectTableId == objectTableId
                                                select new ObjectTableTabPM()
                                                {
                                                    ControlPath = a.ControlPath,
                                                    Id = a.Id,
                                                    IndexOrder = a.IndexOrder,
                                                    ObjectTableId = a.ObjectTableId,
                                                    TabNameTextCodeDefaultText = a.TabNameTextCode.DefaultText,
                                                    TabNameTextCodeId = a.TabNameTextCodeId,
                                                    Tenant = a.Tenant,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    TabNameTextCodeCode = a.TabNameTextCodeCode,
                                                    Code = a.Code,
                                                    FeatureId = a.FeatureId,
                                                };
            return tabs;
        }



    }
}