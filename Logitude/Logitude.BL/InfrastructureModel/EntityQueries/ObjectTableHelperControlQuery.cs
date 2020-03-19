using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ObjectTableHelperControlQuery
    {
        ObjectTableHelperControlRepository repository;
        public ObjectTableHelperControlQuery()
        {
            repository = new ObjectTableHelperControlRepository(); 
        }

        public ObjectTableHelperControlQuery(int tenant)
        {
            repository = new ObjectTableHelperControlRepository(tenant);
        }

        public ObjectTableHelperControlQuery(ObjectTableHelperControlRepository objectTableHelperControlRepository)
        {
            repository = objectTableHelperControlRepository;
        }

        public IQueryable<ObjectTableHelperControlPM> GetObjectTableHelperControlPMsByTenant(int tenant)
        {
            var helpercontrols = from a in repository.context.ObjectTableHelperControls.Include("ObjectTable")
                                 where a.Tenant == 0 //a.tenant==tenant
                                 select new ObjectTableHelperControlPM()
                                 {
                                     ControlPath = a.ControlPath,
                                     Id = a.Id,
                                     ObjectTableId = a.ObjectTableId,
                                     ObjectTableName = a.ObjectTable.Name,
                                     Tenant = a.Tenant,
                                     Code = a.Code,
                                     FeatureId = a.FeatureId,
                                     FeatureUniqeCode = a.FeatureUniqeCode
                                 };
            return helpercontrols;
        }

        public IQueryable<ObjectTableHelperControlPM> GetObjectTableHelperControlsByTenantAndObjectTable(string objectTableId, int tenant)
        {
            IQueryable<ObjectTableHelperControlPM> tabs = from a in repository.context.ObjectTableHelperControls.Include("ObjectTable")
                                                          where a.Tenant == tenant && a.ObjectTableId == objectTableId
                                                          select new ObjectTableHelperControlPM()
                                                          {
                                                              ControlPath = a.ControlPath,
                                                              Id = a.Id,
                                                              ObjectTableId = a.ObjectTableId,
                                                              ObjectTableName = a.ObjectTable.Name,
                                                              Tenant = a.Tenant,
                                                              Code = a.Code,
                                                              FeatureId = a.FeatureId,
                                                              FeatureUniqeCode = a.FeatureUniqeCode
                                                          };
            return tabs;
        }



    }
}