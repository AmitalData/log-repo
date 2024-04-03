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
    public class ConfirmationNumberStatusQuery
    {
        ConfirmationNumberStatusRepository repository;

        public ConfirmationNumberStatusQuery()
        {
            repository = new ConfirmationNumberStatusRepository(); 
        }

        public ConfirmationNumberStatusQuery(int tenant)
        {
            repository = new ConfirmationNumberStatusRepository(tenant);
        }

        public ConfirmationNumberStatusQuery(ConfirmationNumberStatusRepository arInvoiceStatusRepository)
        {
            repository = arInvoiceStatusRepository;
        }

      
        public IQueryable<ConfirmationNumberStatusList> GetIQueryableEntityList(IQueryable<ConfirmationNumberStatus> iQueryable)
        {
            IQueryable<ConfirmationNumberStatusList> result = from entity in iQueryable
                                                     select new ConfirmationNumberStatusList()
                                                     {
                                                         Code = entity.Code,
                                                         LocalName = entity.LocalName,
                                                         Name = entity.Name,
                                                         InActive = entity.InActive,
                                                     };
            return result;
        }

    }
}