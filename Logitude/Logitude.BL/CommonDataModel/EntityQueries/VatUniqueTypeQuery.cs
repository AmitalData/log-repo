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
    public class VatUniqueTypeQuery
    {
        VatUniqueTypeRepository repository;

        public VatUniqueTypeQuery(int tenant)
        {
            repository = new VatUniqueTypeRepository(tenant);
        }
        public VatUniqueTypeQuery(VatUniqueTypeRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<VatUniqueTypeList> GetIQueryableEntityList(IQueryable<VatUniqueType> iQueryable)
        {

            IQueryable<VatUniqueTypeList> result = from f in iQueryable
                                                      select new VatUniqueTypeList()
                                                      {
                                                          Code = f.Code,
                                                          Name = f.Name,
                                                          ViewOrder = f.ViewOrder,
                                                          SearchFields = f.SearchFields,
                                                      };
            return result;
        }
    }
}
