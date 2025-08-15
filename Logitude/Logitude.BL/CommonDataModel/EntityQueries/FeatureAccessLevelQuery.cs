using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class FeatureAccessLevelQuery
    {
        FeatureAccessLevelRepository repository;



        public FeatureAccessLevelQuery(int tenant)
        {
            repository = new FeatureAccessLevelRepository(tenant);
        }

        public FeatureAccessLevelQuery(FeatureAccessLevelRepository myRepository)
        {
            repository = myRepository;
        }

        public IQueryable<FeatureAccessLevelList> GetIQueryableEntityList(IQueryable<FeatureAccessLevel> iQueryable)
        {
            IQueryable<FeatureAccessLevelList> result = from entityPoco in iQueryable
                                                        select new FeatureAccessLevelList()
                                                        {
                                                            Code = entityPoco.Code,
                                                            Name = entityPoco.Name,
                                                            SearchFields = entityPoco.SearchFields,
                                                        };

            return result;
        }
    }
}
