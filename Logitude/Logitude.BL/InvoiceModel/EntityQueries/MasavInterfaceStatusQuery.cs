using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;


namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class MasavInterfaceStatusQuery
    {
        MasavInterfaceStatusRepository repository;
        public MasavInterfaceStatusQuery()
        {
            repository = new MasavInterfaceStatusRepository();
        }


        public MasavInterfaceStatusQuery(int tenant)
        {
            repository = new MasavInterfaceStatusRepository(tenant);
        }

        public MasavInterfaceStatusQuery(MasavInterfaceStatusRepository MasavInterfaceRepository)
        {
            repository = MasavInterfaceRepository;
        }

       
        public IQueryable<MasavInterfaceStatusList> GetIQueryableEntityList(IQueryable<MasavInterfaceStatus> iQueryable)
        {
            IQueryable<MasavInterfaceStatusList> result = from entity in iQueryable
                                                     select new MasavInterfaceStatusList()
                                                     {
                                                         
                                                     };

            return result;
        }
    }
}