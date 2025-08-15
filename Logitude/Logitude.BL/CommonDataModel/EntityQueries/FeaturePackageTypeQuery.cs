using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class FeaturePackageTypeQuery
    {
        FeaturePackageTypeRepository repository;



        public FeaturePackageTypeQuery(int tenant)
        {
            repository = new FeaturePackageTypeRepository(tenant);
        }

        public FeaturePackageTypeQuery(FeaturePackageTypeRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<FeaturePackageTypeList> GetIQueryableEntityList(IQueryable<FeaturePackageType> iQueryable)
        {
            var result = from a in iQueryable
                         select new FeaturePackageTypeList()
                         {
                             Code = a.Code,
                             Name = a.Name,
                             SearchFields = a.SearchFields,
                         };

            return result;
        }
    }
}
