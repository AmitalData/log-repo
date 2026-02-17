using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class FeaturePackageTypeQuery
    {
        FeaturePackageTypeRepository repository;

        public FeaturePackageTypeQuery()
        {
            repository = new FeaturePackageTypeRepository(); 
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
