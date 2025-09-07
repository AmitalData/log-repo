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
    public class VatUniquePartnerTypeQuery
    {
        VatUniquePartnerTypeRepository repository;


        public VatUniquePartnerTypeQuery(int tenant)
        {
            repository = new VatUniquePartnerTypeRepository(tenant);
        }
        public VatUniquePartnerTypeQuery(VatUniquePartnerTypeRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<VatUniquePartnerTypeList> GetIQueryableEntityList(IQueryable<VatUniquePartnerType> iQueryable)
        {

            IQueryable<VatUniquePartnerTypeList> result = from f in iQueryable
                                                   select new VatUniquePartnerTypeList()
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
