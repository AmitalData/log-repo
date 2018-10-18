using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class SATInterfaceQuery
    {

        SATInterfaceRepository repository;
        public SATInterfaceQuery()
        {
            repository = new SATInterfaceRepository();
        }


        public SATInterfaceQuery(SATInterfaceRepository SATInterfaceRepository)
        {
            repository = SATInterfaceRepository;
        }

        public SATInterfaceQuery(int tenant)
        {
            repository = new SATInterfaceRepository(tenant);
        }

        public IQueryable<SATInterfaceList> GetIQueryableEntityList(IQueryable<SATInterface> iQueryable)
        {
            IQueryable<SATInterfaceList> result = from entity in iQueryable
                                                         select new SATInterfaceList()
                                                         {
                                                            Code = entity.Code,
                                                            Name = entity.Name,
                                                            SearchFields = entity.SearchFields,
                                                         };
            return result;
        }
    }
}
